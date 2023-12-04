using System;
using System.Collections.Generic;
using battle.define;
using battle.mechanism.interact_card.by_phase;
using UnityEngine;

public class Mechanism_InteractCard : MonoBehaviour
{
    public static Action refreshInteract;

    public static void RefreshInteract()
    {
        refreshInteract?.Invoke();
    }


    private static IMechanism_InteractCard Mechanism => _mechanismTask ?? _mechanismPhase ?? MechanismDefault;
    private static readonly IMechanism_InteractCard MechanismDefault = new Mechanism_InteractCard_Default();

    private static IMechanism_InteractCard _mechanismPhase;
    private static IMechanism_InteractCard _mechanismTask;


    private static readonly Dictionary<Phase, IMechanism_InteractCard> MechanismPhases = new();

    private static void OnPhase(Phase phase)
    {
        _mechanismPhase = MechanismPhases.ContainsKey(phase) ? MechanismPhases[phase] : MechanismDefault;
        RefreshInteract();
    }


    public static void StartTask(IMechanism_InteractCard task)
    {
        _mechanismTask = task;
        RefreshInteract();
    }

    public static void StopTask()
    {
        _mechanismTask = null;
        RefreshInteract();
    }


    public static bool IsEnable(string cardGuid)
    {
        return Mechanism.InEnable(cardGuid);
    }
}