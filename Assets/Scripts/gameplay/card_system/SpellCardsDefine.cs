using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "CardDefine/Spell", fileName = "define_card_spell")]
public class SpellCardsDefine : ScriptableObject
{
    [TableList] public List<CardDefine_Spell> defines;
}

[Serializable]
public class CardDefine_Spell : CardDefine
{
    public SpellCardType type;
}

public enum SpellCardType
{
    Normal = 1,
    Continuous = 2,
    Equip = 3,
    QuickPlay = 4,
    Field = 5,
    Ritual = 6,
}