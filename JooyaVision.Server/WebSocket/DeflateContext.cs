using vtortola.WebSockets;

namespace JooyaVision.Server
{
    public sealed class DeflateContext : IWebSocketMessageExtensionContext
    {
        public WebSocketMessageReadStream ExtendReader(WebSocketMessageReadStream message)
        {
            return message.Flags.Rsv1 ? new DeflateReadStream(message) : message;
        }

        public WebSocketMessageWriteStream ExtendWriter(WebSocketMessageWriteStream message)
        {
            message.ExtensionFlags.Rsv1 = true;
            return new DeflateWriteStream(message);
        }
    }
}
