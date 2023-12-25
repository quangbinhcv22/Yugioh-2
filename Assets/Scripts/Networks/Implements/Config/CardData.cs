using System;
using System.Collections.Generic;
using System.Linq;
using battle.define;
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


    [Serializable]
    public struct CardConfig
    {
        public string code;
        public string name;
        public string type;
        public string desc;
        public int rarity;

        public string monsterType;
        public string monsterAttribute;
        public int level;
        public int atk;
        public int def;

        public string spellType;
        public List<Effect> effects;
    }
    
    [Serializable]
    public struct Effect
    {
        public string targetCardType;
        public string type;
        public string targetCardPosition;
        public int value;
        public List<string> targetMonsterTypes;
    }
}