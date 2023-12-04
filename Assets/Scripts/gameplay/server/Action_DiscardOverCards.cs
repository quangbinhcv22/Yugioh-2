using battle.define;
using Networks;
using UnityEngine;

public class Action_DiscardOverCards : MonoBehaviour
{
    public void OnEnable()
    {
        Networks.Network.Event.Fighting.onPhase += OnPhase;
    }
    
    public void OnDisable()
    {
        Networks.Network.Event.Fighting.onPhase -= OnPhase;
    }

    private void OnPhase(Response_OnPhase data)
    {
        if (!Networks.Network.Query.Fighting.IsOwnTurn) return;
        if (Networks.Network.Query.Fighting.CurrentPhase is not Phase.Draw) return;
    }
}