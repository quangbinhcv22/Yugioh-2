using System;
using Gameplay.board;
using Networks;

[Serializable]
public class Zone_Field
{
    public CardSpace space;
}

public class CardSpace
{
    public ServerCard card;
}

public class CardSpace_Combat
{
    public ServerCard card;
    public MonsterPosition position;
    public int attack;
    public int defense;
}