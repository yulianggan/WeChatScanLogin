#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
微信扫码登录 Python 客户端

功能：
1. 获取小程序二维码
2. 在终端显示二维码
3. 通过 WebSocket 监听扫码状态
4. 返回登录成功后的用户信息
"""

import requests
import websocket
import json
import threading
import time
import sys
import os

try:
    import qrcode
except ImportError:
    pass

class WeChatScanLogin:
    def __init__(self, base_url="https://api.kornza.com"):
        """
        初始化微信扫码登录客户端
        
        Args:
            base_url: API 服务器地址
        """
        self.base_url = base_url.rstrip('/')
        self.api_url = f"{self.base_url}/api/WeChatService"
        self.ws_url = self.base_url.replace("https://", "wss://").replace("http://", "ws://")
        
        self.qr_code_url = None
        self.web_page_key = None
        self.mini_program_key = None
        
        self.user_info = None
        self.login_status = None  # None, "Scan", "Login", "Cancel"
        self.is_connected = False
        self.ws = None
        
    def get_qr_code(self):
        """获取小程序二维码"""
        try:
            response = requests.post(
                f"{self.api_url}/GetWeChatQrCode",
                headers={"Content-Type": "application/json"},
                timeout=30
            )
            data = response.json()
            
            if data.get("statusCode") == 200 and data.get("data", {}).get("code") == 200:
                qr_data = data["data"]["data"]
                self.qr_code_url = f"{self.base_url}{qr_data['imageUrl']}"
                self.web_page_key = qr_data["webPageKey"]
                self.mini_program_key = qr_data["miniProgramKey"]
                return True
            else:
                print(f"获取二维码失败: {data}")
                return False
        except Exception as e:
            print(f"请求二维码接口失败: {e}")
            return False
    
    def display_qr_code(self):
        """在终端显示二维码"""
        if not self.qr_code_url:
            print("请先获取二维码")
            return
        
        try:
            response = requests.get(self.qr_code_url, timeout=10)
            
            # 保存二维码图片
            qr_file = "qrcode.jpg"
            with open(qr_file, "wb") as f:
                f.write(response.content)
            
            print("\n" + "=" * 50)
            print("请使用微信扫描下方二维码登录")
            print("=" * 50 + "\n")
            
            # 尝试在终端显示二维码
            try:
                from PIL import Image
                import io
                
                # 加载图片
                img = Image.open(io.BytesIO(response.content))
                img = img.convert('L')  # 转为灰度
                
                # 调整大小（终端字符宽高比约为 2:1）
                width = 60
                height = int(width * img.height / img.width / 2)
                img = img.resize((width, height))
                
                # 转换为 ASCII 字符
                pixels = list(img.getdata())
                chars = []
                for i, pixel in enumerate(pixels):
                    if i % width == 0 and i != 0:
                        chars.append('\n')
                    # 深色用实心块，浅色用空格
                    chars.append('██' if pixel < 128 else '  ')
                
                print(''.join(chars))
                print()
                
            except ImportError:
                print("提示: pip install Pillow 可在终端显示二维码")
            
            # 尝试自动打开图片
            import subprocess
            import platform
            
            system = platform.system()
            try:
                if system == "Darwin":  # macOS
                    subprocess.Popen(["open", qr_file], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
                elif system == "Windows":
                    subprocess.Popen(["start", qr_file], shell=True, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
                elif system == "Linux":
                    subprocess.Popen(["xdg-open", qr_file], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
            except:
                pass
            
            print(f"二维码图片已保存到: {qr_file}")
            print(f"或访问: {self.qr_code_url}")
            print("\n等待扫码中...\n")
            
        except Exception as e:
            print(f"显示二维码失败: {e}")
            print(f"请手动访问: {self.qr_code_url}")
    
    def _on_ws_message(self, ws, message):
        """WebSocket 消息回调"""
        print(f"[DEBUG] 收到消息: {message}")
        try:
            data = json.loads(message)
            status = data.get("Status", data.get("status"))
            msg = data.get("Msg", data.get("msg"))
            
            if status == "Scan":
                self.login_status = "Scan"
                print("✓ 已扫码，等待确认登录...")
                
            elif status == "Login":
                self.login_status = "Login"
                if msg:
                    if isinstance(msg, str):
                        try:
                            self.user_info = json.loads(msg)
                        except:
                            self.user_info = {"raw": msg}
                    else:
                        self.user_info = msg
                print("✓ 登录成功！")
                ws.close()
                
            elif status == "Calcel" or status == "Cancel":
                self.login_status = "Cancel"
                print("✗ 用户取消登录")
                ws.close()
                
        except Exception as e:
            print(f"处理消息失败: {e}, 消息: {message}")
    
    def _on_ws_error(self, ws, error):
        """WebSocket 错误回调"""
        print(f"WebSocket 错误: {error}")
    
    def _on_ws_close(self, ws, close_status_code, close_msg):
        """WebSocket 关闭回调"""
        self.is_connected = False
        print("WebSocket 连接关闭")
    
    def _on_ws_open(self, ws):
        """WebSocket 连接成功回调"""
        self.is_connected = True
        print("✓ WebSocket 连接成功，等待扫码...")
        print(f"[DEBUG] ClientID: {self.web_page_key}")
        
        # 启动心跳线程
        def heartbeat():
            while self.is_connected:
                try:
                    if ws.sock and ws.sock.connected:
                        ws.send(json.dumps({"Action": "heartbeat", "Msg": "ping"}))
                except:
                    break
                time.sleep(30)
        
        threading.Thread(target=heartbeat, daemon=True).start()
    
    def start_websocket(self, timeout=300):
        """启动 WebSocket 监听"""
        if not self.web_page_key:
            print("请先获取二维码")
            return False
        
        ws_url = f"{self.ws_url}/ws?ClientID={self.web_page_key}"
        
        self.ws = websocket.WebSocketApp(
            ws_url,
            on_open=self._on_ws_open,
            on_message=self._on_ws_message,
            on_error=self._on_ws_error,
            on_close=self._on_ws_close
        )
        
        ws_thread = threading.Thread(
            target=lambda: self.ws.run_forever(ping_interval=30, ping_timeout=10)
        )
        ws_thread.daemon = True
        ws_thread.start()
        
        # 等待登录结果
        start_time = time.time()
        while time.time() - start_time < timeout:
            if self.login_status == "Login":
                return True
            elif self.login_status == "Cancel":
                return False
            time.sleep(0.5)
        
        if self.ws:
            self.ws.close()
        return False
    
    def login(self, timeout=300):
        """执行扫码登录流程"""
        print("=" * 50)
        print("微信扫码登录")
        print("=" * 50)
        
        print("\n[1/3] 获取二维码...")
        if not self.get_qr_code():
            return None
        print(f"✓ 二维码获取成功")
        
        print("\n[2/3] 显示二维码...")
        self.display_qr_code()
        
        print("[3/3] 启动扫码监听...")
        if self.start_websocket(timeout):
            return self.user_info
        
        return None

def main():
    client = WeChatScanLogin(base_url="https://api.kornza.com")
    user_info = client.login(timeout=300)
    
    if user_info:
        print("\n" + "=" * 50)
        print("登录成功！用户信息如下：")
        print("=" * 50)
        print(json.dumps(user_info, ensure_ascii=False, indent=2))
        print("=" * 50)
        return user_info
    else:
        print("\n登录失败或已取消")
        return None

if __name__ == "__main__":
    result = main()
    sys.exit(0 if result else 1)
