using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.card.ui;
using Networks;
using Sirenix.OdinInspector;

[Serializable]
public class Zone_SpellTrap
{
    private const int Capacity = 5;
    [ShowInInspector] public readonly List<CardSpace> spaces;

    public Zone_SpellTrap()
    {
        spaces = new(Capacity);
        for (int i = 0; i < Capacity; i++) spaces.Add(new());
    }
    
    public int Set(ServerCard card)
    {
        var index = EmptyIndex;
        
        var space = spaces[index];
        space.card = card;

        return index;
    }

    public int EmptyIndex => spaces.FindIndex(space => space.card == null);

    public ServerCard Take(string guild)
    {
        var card = Get(guild).card;

        var index = SearchIndex(guild);
        spaces[index] = new ();

        return card;
    }
    
    public CardSpace Get(string cardGuild)
    {
        return spaces.FirstOrDefault(s => s.card != null && s.card.Guid == cardGuild);
    }
    
    public int SearchIndex(string cardGuid)
    {
        for (var i = 0; i < spaces.Count; i++)
        {
            if (spaces[i].card != null && spaces[i].card.Guid == cardGuid) return i;
        }

        return CardLocation.NotFound;
    }
}