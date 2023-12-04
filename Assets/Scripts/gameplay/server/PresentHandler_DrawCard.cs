using System;
using event_name;
using TigerForge;

public class PresentHandler_DrawCard : Singleton<PresentHandler_DrawCard>
{
    public Action onCheckOver;
    
    
    protected override void OnEnable()
    {
        base.OnEnable();

        EventManager.StartListening(EventName.Gameplay.Draw, OnDraw);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.Gameplay.Draw, OnDraw);
    }

    private void OnDraw()
    {
        if (!Networks.Network.Query.Fighting.IsOwnTurn) return;
        onCheckOver?.Invoke();
    }
}