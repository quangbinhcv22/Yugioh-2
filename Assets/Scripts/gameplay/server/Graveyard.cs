using System;
using System.Collections.Generic;
using Networks;

[Serializable]
public class Graveyard
{
    public List<ServerCard> cards = new();

    public void Add(ServerCard dieCard)
    {
        cards.Add(dieCard);
    }

    public void Clear()
    {
        cards.Clear();
    }
}