using System;
using gameplay.present;
using UnityEngine;

public class PresentHandler_Zone_MainMonster : MonoBehaviour
{
    public static Action onClear;

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
        onClear?.Invoke();
    }
}