using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Request
        {
            public static class Config
            {
                public static void GetCardDataVersion()
                {
                    PlayerPrefs.DeleteAll();
                    
                    Send(MessageID.GET_CARD_DATA_VERSION);
                }

                public static void GetCardData()
                {
                    Send(MessageID.GET_CARD_DATA);
                }
            }
        }

        public static partial class HandleResponse
        {
            public static class Config
            {
                const string CardVersionKey = "card_version_key";
                public const string CardDataKey = "card_data_key";

                public static void GetCardDataVersion(string data)
                {
                    if (PlayerPrefs.HasKey(CardVersionKey))
                    {
                        var version = PlayerPrefs.GetString(CardVersionKey);
                        if (version == data)
                        {
                            return;
                        }
                    }

                    PlayerPrefs.SetString(CardVersionKey, data);
                    Request.Config.GetCardData();
                }

                public static void GetCardData(JArray data)
                {
                    var cardConfigs = data.Select(j => j.ToObject<CardConfig>()).ToList();
                    var json = JsonConvert.SerializeObject(cardConfigs);

                    PlayerPrefs.SetString(CardDataKey, json);
                }
            }
        }
    }
    //
    //
    // public class CardData
    // {
    //     
    // }

    public struct Response_GetCardData
    {
    }

    [Serializable]
    public class CardConfig
    {
        public string code;
        public int def;
        public int level;
        public string monsterType;
        public string name;
        public int atk;
        public string type;
        public string monsterAttribute;
        public string desc;
        public int rarity;
    }
}