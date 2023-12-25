using System.Collections.Generic;
using System.Linq;
using fake;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Request
        {
            public static partial class Fighting
            {
                public static void TestingSetCardDeckBeforeStartGame()
                {
                    var presetCards = ToolPanel_PresetCard.GetSavedPreset();

                    if (presetCards.Any())
                    {
                        Request_TESTING_SET_CARD_DECK_BEFORE_START_GAME testing = new()
                        {
                            deckCardCodes = presetCards,
                        };

                        Send(MessageID.TESTING_SET_CARD_DECK_BEFORE_START_GAME, testing);
                    }
                }

                public static void Testing_EndGame()
                {
                    Send(MessageID.TESTING_END_ALL_GAME_SESSIONS);
                }
            }

            public static partial class Fighting
            {
                // public static void TestingSetCardDeckBeforeStartGame(JObject data)
                // {
                //     
                // }
            }
        }

        public struct Request_TESTING_SET_CARD_DECK_BEFORE_START_GAME
        {
            public List<string> deckCardCodes;
        }

        public struct Response_TESTING_SET_CARD_DECK_BEFORE_START_GAME
        {
            public string status;
        }
    }
}