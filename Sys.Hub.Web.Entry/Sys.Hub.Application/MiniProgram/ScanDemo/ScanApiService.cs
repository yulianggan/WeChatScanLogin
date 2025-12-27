using Furion.DynamicApiController;
using Furion.JsonSerialization;
using Furion.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace Sys.Hub.Application.MiniProgram
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/5/3 16:28:49 
    /// 描    述 ：  在线扫码测试Demo接口
    /// </summary>
    [ApiDescriptionSettings("在线扫码测试Demo接口", Name = "在线扫码测试Demo接口", Description = "在线扫码测试Demo接口", Order = 2)]
    [Route("api/ScanService")]
    public class ScanApiService : IDynamicApiController
    {
        HttpContext _httpContext;
        public ScanApiService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// 建立webSoket连接
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Route("/ws")]
        public async Task Get(string ClientID)
        {
            if (_httpContext.Request.Path == "/ws")
            {
                if (_httpContext.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await _httpContext.WebSockets.AcceptWebSocketAsync();
                    ClientID = string.IsNullOrEmpty(ClientID) ? Guid.NewGuid().ToString() : ClientID;
                    var wsClient = new WebsocketClient
                    {
                        ID = ClientID,
                        WebSocket = webSocket
                    };
                    try
                    {
                        await Handle(wsClient);
                    }
                    catch (Exception ex)
                    {
                        await _httpContext.Response.WriteAsync("closed");
                    }
                }
                else
                {
                    _httpContext.Response.StatusCode = 404;
                }
            }
        }

        private async Task Handle(WebsocketClient WebSocket)
        {
            WebsocketClientCollection.Add(WebSocket);
            WebSocketReceiveResult result = null;
            do
            {
                var buffer = new byte[1024 * 1];
                result = await WebSocket.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text && !result.CloseStatus.HasValue)
                {
                    var msgString = Encoding.UTF8.GetString(buffer);
                    var message = JsonConvert.DeserializeObject<Message>(msgString);
                    MessageRoute(message);
                }
            }
            while (!result.CloseStatus.HasValue);
            WebsocketClientCollection.Remove(WebSocket);
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="MessageEntity"></param>
        private void MessageRoute(Message MessageEntity)
        {
            if (MessageEntity == null)
                return;
            
            var targetId = !string.IsNullOrEmpty(MessageEntity.ReceiveID) ? MessageEntity.ReceiveID : MessageEntity.SendClientId;
            Console.WriteLine($"[WebSocket] 消息路由 - Action: {MessageEntity.Action}, ReceiveID: {MessageEntity.ReceiveID}, SendClientId: {MessageEntity.SendClientId}, TargetID: {targetId}");
            Console.WriteLine($"[WebSocket] 当前连接数: {WebsocketClientCollection.Count()}");
            
            var client = WebsocketClientCollection.Get(targetId);
            
            if (client == null)
            {
                Console.WriteLine($"[WebSocket] 警告: 未找到目标客户端 {targetId}");
                // 打印所有当前连接的客户端ID
                WebsocketClientCollection.PrintAllClients();
                return;
            }
            
            switch (MessageEntity.Action)
            {
                case "Calcel":
                case "Scan":
                case "Login":
                    Console.WriteLine($"[WebSocket] 发送消息到客户端 {targetId}: Action={MessageEntity.Action}");
                    client.SendMessageAsync(JSON.Serialize(new { Status = MessageEntity.Action, Msg = MessageEntity.Msg }));
                    break;
                default:
                    break;
            }
        }
    }


    public class WebsocketClientCollection
    {
        private static List<WebsocketClient> _clients = new List<WebsocketClient>();

        public static void Add(WebsocketClient client)
        {
            Console.WriteLine($"[WebSocket] 添加客户端: {client.ID}");
            _clients.Add(client);
        }

        public static void Remove(WebsocketClient client)
        {
            Console.WriteLine($"[WebSocket] 移除客户端: {client.ID}");
            _clients.Remove(client);
        }

        public static WebsocketClient Get(string clientId)
        {
            var client = _clients.FirstOrDefault(c => c.ID == clientId);
            Console.WriteLine($"[WebSocket] 查找客户端 {clientId}: {(client != null ? "找到" : "未找到")}");
            return client;
        }
        
        public static int Count()
        {
            return _clients.Count;
        }
        
        public static void PrintAllClients()
        {
            Console.WriteLine($"[WebSocket] 所有连接的客户端:");
            foreach (var c in _clients)
            {
                Console.WriteLine($"  - {c.ID}");
            }
        }

        public static List<WebsocketClient> GetRoomClients(string roomNo)
        {
            var client = _clients.Where(c => c.RoomNo == roomNo);
            return client.ToList();
        }
    }
    public class Message
    {
        /// <summary>
        /// 连接池ID
        /// </summary>
        public string SendClientId { get; set; }

        /// <summary>
        /// 动作
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 接受信息者ClientID
        /// </summary>
        public string? ReceiveID { get; set; }
    }

    /// <summary>
    /// WebsocketClient
    /// </summary>
    public class WebsocketClient
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        public string ID { get; set; }

        public WebSocket WebSocket { get; set; }



        public string RoomNo { get; set; }

        public Task SendMessageAsync(string message)
        {
            var msg = Encoding.UTF8.GetBytes(message);
            return WebSocket.SendAsync(new ArraySegment<byte>(msg, 0, msg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
