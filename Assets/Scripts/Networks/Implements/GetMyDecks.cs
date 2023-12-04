using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Cached
        {
            public static List<Deck> decks;
        }
        
        public static partial class Request
        {
            public static void GetMyDecks()
            {
                Send(MessageID.GET_MY_DECKS);
            }
        }

        public static partial class HandleResponse
        {
            public static void GetMyDecks(JArray data)
            {
                var decks = data.Select(j => j.ToObject<Deck>()).ToList();
                Network.Cached.decks = decks;
            }
        }
    }

    public struct Deck
    {
        public string name;
        public long id;
        public List<string> cardCodes;
    }
}