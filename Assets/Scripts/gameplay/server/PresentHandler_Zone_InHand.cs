using System;
using gameplay.present;
using UnityEngine;

[DefaultExecutionOrder(int.MinValue + 1)]
public class PresentHandler_Zone_InHand : Singleton<PresentHandler_Zone_InHand>
{
    public static Action onClear;

    protected override void OnEnable()
    {
        PresentHandler_NewRound.onResetRound += OnResetRound;
    }

    private void OnDisable()
    {
        PresentHandler_NewRound.onResetRound -= OnResetRound;
    }

    private void OnResetRound()
    {
        onClear?.Invoke();
    }
}

public class PresentHandler_Zone_MainDeck : MonoBehaviour
{
    public void OnEnable()
    {
        PresentHandler_NewRound.onResetRound += OnResetRound;
    }

    private void OnDisable()
    {
        PresentHandler_NewRound.onResetRound -= OnResetRound;
    }

    private void OnResetRound()
    {
    }
}

public class PresentHandler_Zone_Graveyard : MonoBehaviour
{
    public void OnEnable()
    {
        PresentHandler_NewRound.onResetRound += OnResetRound;
    }

    private void OnDisable()
    {
        PresentHandler_NewRound.onResetRound -= OnResetRound;
    }

    private void OnResetRound()
    {
    }
}