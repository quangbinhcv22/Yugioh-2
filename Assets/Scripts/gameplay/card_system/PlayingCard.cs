using System;

[Serializable]
public class CardDefine
{
    public string passcode;
}


[Serializable]
public struct PlayingCard
{
    public string guid;
    public string passcode;

    public static readonly PlayingCard Empty = default;

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(guid);
    }
}


public enum CardFace
{
    Up = 1,
    Down = 2,
}