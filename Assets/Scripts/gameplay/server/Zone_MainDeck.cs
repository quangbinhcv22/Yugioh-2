using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.card.@enum;
using Networks;
using UnityEngine;

[Serializable]
public class Zone_MainDeck
{
    public int remain;

    public void Take(int amount)
    {
        remain -= amount;
    }

    public void Init()
    {
        remain = 40;
    }
}