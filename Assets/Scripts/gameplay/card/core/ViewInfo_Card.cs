using System;
using System.Linq;
using battle.define;
using Gameplay.card.@enum;
using UnityEngine;
using Random = UnityEngine.Random;
using TextGenerator = fake.TextGenerator;

namespace Gameplay.card.core
{
    // [Serializable]
    // public class ViewInfo_Card
    // {
    //     public string id;
    //     public string displayName;
    //     public string lore;
    // }


    // [Serializable]
    // public class ViewInfo_MonsterCard : ViewInfo_Card
    // {
    //     public int level;
    //     public MonsterAttribute attribute;
    //     public MonsterTypes types;
    //     public CardFrame frame;
    //     public int attack;
    //     public int defense;
    //
    //
    //
    //     public static ViewInfo_MonsterCard GetFake(string id)
    //     {
    //         var randomWeightLevel = Random.Range(0, 100f);
    //         var level = randomWeightLevel switch
    //         {
    //             <= 65 => Random.Range(1, 4 + 1),
    //             <= 90 => Random.Range(5, 6 + 1),
    //             _ => Random.Range(7, 12 + 1),
    //         };
    //
    //         
    //         var randomTypes = Enum.GetValues(typeof(MonsterTypes)).Cast<MonsterTypes>()
    //             .OrderBy(t => Random.Range(0, 1f)).Take(Random.Range(1, 4));
    //         var types = MonsterTypes.None;
    //         foreach (var type in randomTypes) types |= type;
    //
    //         return new ViewInfo_MonsterCard()
    //         {
    //             id = id,
    //
    //             displayName = TextGenerator.GetName(),
    //             lore = TextGenerator.GetParagraph(),
    //
    //             level = level,
    //             attribute = (MonsterAttribute)Random.Range(1, 8),
    //             types = types,
    //             // frame = (CardFrame)Random.Range(1, 10),
    //             frame = level >= 8 ? CardFrame.Black : CardFrame.Yellow,
    //
    //             attack = (int)(Random.Range(10, 30) * Mathf.Pow(1.15f, level)) * 100,
    //             defense = (int)(Random.Range(10, 30) * Mathf.Pow(1.15f, level)) * 100,
    //         };
    //     }
}