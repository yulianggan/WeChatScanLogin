#!/bin/bash

# ========================================
# Claw.cloud VPS 一键部署脚本
# 项目: 微信扫码登录
# ========================================

set -e

echo "=========================================="
echo "  微信扫码登录 - 一键部署脚本"
echo "=========================================="

# 1. 更新系统
echo "[1/5] 更新系统..."
apt update && apt upgrade -y

# 2. 安装 Docker
echo "[2/5] 安装 Docker..."
if ! command -v docker &> /dev/null; then
    curl -fsSL https://get.docker.com | sh
    systemctl enable docker
    systemctl start docker
fi

# 3. 安装 Docker Compose
echo "[3/5] 安装 Docker Compose..."
if ! command -v docker-compose &> /dev/null; then
    apt install -y docker-compose
fi

# 4. 创建必要目录
echo "[4/5] 创建目录..."
mkdir -p /opt/wechat-scan-login
mkdir -p /opt/wechat-scan-login/ssl

# 5. 提示上传文件
echo "[5/5] 准备完成！"
echo ""
echo "=========================================="
echo "  接下来请执行以下操作："
echo "=========================================="
echo ""
echo "1. 将项目文件上传到服务器:"
echo "   scp -r Sys.Hub.Web.Entry/* root@你的服务器IP:/opt/wechat-scan-login/"
echo ""
echo "2. SSH 登录服务器并启动:"
echo "   cd /opt/wechat-scan-login"
echo "   docker-compose up -d --build"
echo ""
echo "3. 查看日志:"
echo "   docker-compose logs -f"
echo ""
echo "4. 配置 Cloudflare DNS:"
echo "   A 记录: kornza.com -> 你的服务器IP"
echo ""
echo "=========================================="
