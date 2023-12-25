using System;
using System.Collections.Generic;
using System.Linq;
using event_name;
using Gameplay.card.ui;
using Networks;
using Sirenix.OdinInspector;
using TigerForge;
using UnityEngine;

[Serializable]
public class Zone_InHand
{
    private const int Capacity = 6;
    [ShowInInspector] public List<CardSpace> spaces;

    public Zone_InHand()
    {
        spaces = new List<CardSpace>(Capacity);
    }

    public int Amount => spaces.Count;
    public int OverAmount => (int)MathF.Max(0, Amount - DueConstant.maxCardInHand);
    public bool IsOver => Amount > DueConstant.maxCardInHand;


    // public void Import(List<string> cardIds)
    // {
    //     cards.Clear();
    //     
    //     foreach (var id in cardIds)
    //     {
    //         cards.Add(DueCardQuery.CreateMonster(id));
    //     }
    // }

    public void Add(ServerCard card)
    {
        spaces.Add(new CardSpace()
        {
            card = card,
        });
    }

    public void Sync(int playerIndex, List<SyncTemp_HandCard> cards)
    {
        foreach (var tempCard in cards)
        {
            if (spaces.All(space => space.card.Guid != tempCard.id.ToString()))
            {
                Server_DueManager.main.Draw(playerIndex, new List<ServerCard>() { tempCard.To_ServerCard() });
            }
        }
    }

    public void SyncAmony(int playerIndex, int amount)
    {
        var missing = amount - spaces.Count;

        if (missing > 0)
        {
            for (int i = 0; i < missing; i++)
            {
                Server_DueManager.main.Draw(1, new() { ServerCard.NewAnonymous() });
            }
        }
        else if (missing < 0)
        {
            var removeCards = spaces.Take(Mathf.Abs(missing));
            spaces.RemoveRange(0, Mathf.Abs(missing));

            foreach (var removeCard in removeCards)
            {
                DueNotifier.Notify_InHandRemove(playerIndex, removeCard.card.Guid);
            }
        }
    }

    public int SearchIndex(string cardGuid)
    {
        for (var i = 0; i < spaces.Count; i++)
        {
            if (spaces[i].card.Guid == cardGuid) return i;
        }

        return CardLocation.NotFound;
    }

    // public UsedCard Take(string cardGuild)
    // {
    //     var space = spaces.First(s => s.card.Guid == cardGuild);
    //     spaces.Remove(space);
    //
    //     return space.card;
    // }

    public ServerCard Get(string cardGuild)
    {
        var space = spaces.First(s => s.card.Guid == cardGuild);
        return space.card;
    }

    public void Clear()
    {
        spaces.Clear();
    }

    public ServerCard Release(string cardGuid, ServerCard card)
    {
        if (spaces.Any(s => s.card.Guid == cardGuid))
        {
            var space = spaces.First(s => s.card.Guid == cardGuid);
            spaces.Remove(space);

            return space.card;
        }
        else
        {
            spaces.RemoveAt(0);

            DueCardQuery.InitCard(card);
            return card;
        }
    }
}