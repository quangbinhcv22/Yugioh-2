using System;
using Gameplay.card.ui;
using Sirenix.OdinInspector;

[Serializable]
public class BoardZone
{
    [ShowInInspector] public Zone_InHand inHand = new();
    public Zone_MainMonster mainMonster = new();
    public Zone_SpellTrap spellTrap = new();
    public Zone_Field field = new();
    public Graveyard graveyard = new();
    public Zone_ExtraDeck extraDeck = new();
    public Zone_MainDeck mainDeck = new();
    public Zone_Ban ban = new();
    
    
    public void ClearAll()
    {
        inHand.Clear();
        mainMonster.Clear();
        graveyard.Clear();

        // spellTrap.Clear();
        // field.Clear();
        // extraDeck.Clear();
        // ban.Clear();
    }

    public CardLocation Locate(string cardGuid)
    {
        var inHandIndex = inHand.SearchIndex(cardGuid);
        if(inHandIndex != CardLocation.NotFound) return new CardLocation()
        {
            index = inHandIndex,
            zoneType = BoardZoneType.InHand,
        };
        
        var mainMonsterIndex = mainMonster.SearchIndex(cardGuid);
        if(mainMonsterIndex != CardLocation.NotFound) return new CardLocation()
        {
            index = mainMonsterIndex,
            zoneType = BoardZoneType.MainMonster,
        };
        
        
        var fieldIndex = field.SearchIndex(cardGuid);
        if(fieldIndex != CardLocation.NotFound) return new CardLocation()
        {
            index = fieldIndex,
            zoneType = BoardZoneType.Field,
        };

        return new()
        {
            playerIndex = CardLocation.NotFound,
            index = CardLocation.NotFound,
            zoneType = BoardZoneType.Unknown,
        };
    }
}

public enum BoardZoneType
{
    Unknown,
    InHand,
    MainMonster,
    SpellTrap,
    Field,
    Graveyard,
    ExtraDeck,
    MainDeck,
    Ban
}