using System;
using UnityEngine;

public class Present_PlayerInfo : MonoBehaviour
{
    public static Action onEssential;

    public static PlayerInfo self;
    public static PlayerInfo opponent;


    public class PlayerInfo
    {
        public string name;
        public int avatarID;
    }


    private void OnEnable()
    {
        Gameplay_Event.OnLoad_EssentialData += OnLoad_EssentialData;
    }

    private void OnDisable()
    {
        Gameplay_Event.OnLoad_EssentialData -= OnLoad_EssentialData;
    }


    private void OnLoad_EssentialData()
    {
        FetchData();
        onEssential?.Invoke();
    }

    private void FetchData()
    {
        //self
        //opponent
    }
}