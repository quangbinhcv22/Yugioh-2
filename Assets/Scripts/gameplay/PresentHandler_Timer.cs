using battle.define;
using event_name;
using Gameplay;
using Networks;
using TigerForge;
using UnityEngine;
using Network = Networks.Network;

[DefaultExecutionOrder(int.MinValue)]
public class PresentHandler_Timer : Singleton<PresentHandler_Timer>
{
    public TurnTimer timer;


    protected override void OnEnable()
    {
        base.OnEnable();

        EventManager.StartListening(EventName.Gameplay.ToTurn, OnToTurn);
        
        
        Network.Event.Fighting.onPhase += OnPhase;
    }

    private void OnPhase(Response_OnPhase obj)
    {
        OnToPhase();
    }


    private void OnDisable()
    {
        EventManager.StopListening(EventName.Gameplay.ToTurn, OnToTurn);
        Network.Event.Fighting.onPhase -= OnPhase;
    }

    private void OnToTurn()
    {
        var startTime = Server_PhaseManager.main.PhaseStart;
        timer.Start(startTime, DueConstant.secondsPerTurn);
    }

    private void OnToPhase()
    {
        if (Network.Query.Fighting.CurrentPhase is Phase.End)
        {
            timer.Stop();
        }
    }
}