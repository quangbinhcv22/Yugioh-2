using Newtonsoft.Json.Linq;
using QBPlugins.ScreenFlow;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Cached
        {
            public static string disconnectReason;
        }

        public static partial class HandleResponse
        {
            public static void Disconnect(JObject data)
            {
                var response = data.ToObject<Response_Disconnect>();
                Network.Cached.disconnectReason = response.content;
                
                ScreenManager.Open(ScreenKey.Disconnect);
            }
        }
    }
    
    public struct Response_Disconnect
    {
        public string content;
    }
}