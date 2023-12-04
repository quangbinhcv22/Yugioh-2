using System.Collections.Generic;
using battle.define;
using Gameplay;
using Gameplay.card.core;

namespace QBPlugins.Game.Core
{
    public class SpellCard_Query
    {
        private static readonly Dictionary<string, SpellCard> SpellCards = new();

        public static SpellCard InstantiateSpell(Team team, string guid)
        {
            // var id = DueCardQuery.GetViewInfo(guid).id;
            //
            // var card = Create(id);
            // card.team = team;
            //
            // SpellCards.Add(guid, card);
            
            return null;
        }
        
        
        // public static readonly List<ViewInfo_Card> defines = new()
        // {
        //     new()
        //     {
        //         id = "sp01",
        //         displayName = "SALAMANDRA",
        //         lore = "Trang bị cho quái thú hệ Lửa (Fire), tăng 700 ATK",
        //     },
        //
        //     // new()
        //     // {
        //     //     id = "sp02",
        //     //     displayName = "GUST FAN",
        //     //     lore = "Trang bị cho quái thú hệ Gió (wind), tăng 400  ATK và giảm 200 DEF.",
        //     // },
        //     //
        //     // new()
        //     // {
        //     //     id = "sp03",
        //     //     displayName = "BEAST FANGS",
        //     //     lore = "Lá bài tộc Quái Thú (Beast) tăng 300  ATK và DEF.",
        //     // },
        //     //
        //     //
        //     // new()
        //     // {
        //     //     id = "sp04",
        //     //     displayName = "MAGE POWER",
        //     //     lore =
        //     //         "Lá bài quái thú nào được trang bị sẽ tăng 500 ATK và DEF cho mỗi Bài Phép/Bẫy mà bạn điều khiển.",
        //     // },
        //     //
        //     // new()
        //     // {
        //     //     id = "sp05",
        //     //     displayName = "MACHINE CONVERSION FACTORY",
        //     //     lore = "Trang bị cho quái thú tộc Máy Móc (Machine). Tăng 300 ATK và DEF.",
        //     // },
        // };

        // private static SpellCard Create(string id)
        // {
        //     return FactoryGeneric.Get<SpellCard>("sp", id);
        // }
        //
        // public static string GetRandom()
        // {
        //     return defines[UnityEngine.Random.Range(0, defines.Count)].id;
        // }
    }
}