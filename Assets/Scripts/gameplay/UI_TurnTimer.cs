using System;
using event_name;
using Networks;
using TigerForge;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(int.MinValue + 1)]
public class UI_TurnTimer : MonoBehaviour
{
    // private static TurnTimer Timer => PresentHandler_Timer.Current.timer;
    public Color selfColor;
    public Color opponentColor;

    [SerializeField] private TMP_Text text;


    private void OnEnable()
    {
        // Timer.OnStar += StartCounting;
        // Timer.OnStop += StopCounting;

        // EventManager.StartListening(EventName.Gameplay.ToTurn, OnTurn);
        
        Networks.Network.Event.Fighting.startRound += StartRound;
    }

    private void StartRound(Response_StartRound obj)
    {
        StopCounting();
        StartCounting();
    }
    
    private void OnDisable()
    {
        // Timer.OnStar -= StartCounting;
        // Timer.OnStop -= StartCounting;

        Networks.Network.Event.Fighting.startRound -= StartRound;
        StopCounting();

        // EventManager.StopListening(EventName.Gameplay.ToTurn, OnTurn);
    }


    public void StartCounting()
    {
        StopCounting();
        InvokeRepeating(nameof(Counting), 0f, 1f);
    }

    public void StopCounting()
    {
        CancelInvoke(nameof(Counting));
    }

    private void Counting()
    {
        var remaining = Networks.Network.Cached.Fighting.endRoundTime - DateTime.Now;
        UpdateView(remaining);

        if (remaining.Seconds <= 0)
        {
            StopCounting();
            UpdateView(TimeSpan.Zero);
        }
    }

    private void UpdateView(TimeSpan remaining)
    {
        if (remaining.Seconds <= 0)
        {
            text.SetText("-");
        }
        else
        {
            text.SetText(remaining.ToString(@"mm\:ss"));
        }
    }
}