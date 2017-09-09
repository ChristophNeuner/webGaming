using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace webGaming.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class PaintingTogetherMiddleware
    {
        private readonly RequestDelegate _next;
        private List<WebSocket> _sockets;

        public PaintingTogetherMiddleware(RequestDelegate next)
        {
            _next = next;
            _sockets = new List<WebSocket>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path == "/wsPaintingTogether")
            {
                if (httpContext.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
                    if (!(_sockets.Contains(webSocket)))
                        _sockets.Add(webSocket);
                            
                    await Broadcast(httpContext, webSocket);
                }
                else
                {
                    httpContext.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }

        private async Task Broadcast(HttpContext context, WebSocket webSocket)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var token = CancellationToken.None;
                var buffer = new ArraySegment<Byte>(new Byte[4096]);
                var received = await webSocket.ReceiveAsync(buffer, token);

                switch (received.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var request = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
                        await SendToWebSockets(_sockets, request, webSocket);
                        break;
                }
            }

            //var buffer = new byte[1024 * 4];
            //WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            //while (!result.CloseStatus.HasValue)
            //{
            //    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

            //    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            //}
            //await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private async Task SendToWebSockets(List<WebSocket> sockets, string message, WebSocket sendingSocket)
        {
            foreach(WebSocket socket in sockets)
            {
                if(socket != sendingSocket && socket.State == WebSocketState.Open)
                {
                    var token = CancellationToken.None;
                    var type = WebSocketMessageType.Text;
                    var data = Encoding.UTF8.GetBytes(message);
                    var buffer = new ArraySegment<Byte>(data);
                    await socket.SendAsync(buffer, type, true, token);
                }
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class PaintingTogetherMiddlewareExtensions
    {
        public static IApplicationBuilder UsePaintingTogetherMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PaintingTogetherMiddleware>();
        }
    }
}
