using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.card.@enum;
using Networks;
using UnityEngine;

[Serializable]
public class Zone_MainDeck
{
    public List<ServerCard> cards = new();

    public List<ServerCard> Take(int amount)
    {
        amount = Mathf.Min(amount, cards.Count);
        var tookCards = cards.Take(amount).ToList();
        
        cards.RemoveRange(0, amount);


        return tookCards;
    }

    // public void Import(List<string> cardIds)
    // {
    //     cards.Clear();
    //     
    //     foreach (var id in cardIds)
    //     {
    //         cards.Add(DueCardQuery.CreateMonster(id));
    //     }
    // }
    public void Init()
    {
        for (int i = 0; i < 40; i++)
        {
            cards.Add(new());
        }
    }
}