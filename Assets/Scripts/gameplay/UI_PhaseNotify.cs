using System;
using battle.define;
using event_name;
using Networks;
using TigerForge;
using UnityEngine;
using UX;
using Network = Networks.Network;

namespace Gameplay
{
    public class UI_PhaseNotify : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.StartListening(EventName.Gameplay.ToTurn, OnToTurn);

            Network.Event.Fighting.onPhase += OnPhase;
        }

        private void OnPhase(Response_OnPhase data)
        {
            var phase = Network.Query.Fighting.CurrentPhase;
            if (phase is Phase.Unset) return;

            var content = phase switch
            {
                Phase.Draw => "Draw Phase",
                Phase.Standby => "Standby Phase",
                Phase.Main1 => "Main Phase 1",
                Phase.Battle => "Battle Phase",
                Phase.Main2 => "Main Phase 2",
                Phase.End => "End Phase",
                _ => throw new ArgumentOutOfRangeException()
            };

            NotifyText.Notify(content);
        }


        private void OnDisable()
        {
            EventManager.StopListening(EventName.Gameplay.ToTurn, OnToTurn);
            Network.Event.Fighting.onPhase -= OnPhase;
        }


        private void OnToTurn()
        {
            var ownTurn = Networks.Network.Query.Fighting.IsOwnTurn;

            var color = ownTurn ? Color.green : Color.red;
            var content = ownTurn ? "Your Phase" : "Opponent's Phase";

            NotifyText.SetColor(color);
            NotifyText.Notify(content);
        }
    }
}