using System;
using battle.define;
using event_name;
using Networks;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Network = Networks.Network;

namespace Gameplay
{
    public class PhaseButton : MonoBehaviour
    {
        [Space] [SerializeField] private Phase phase;

        [Space] [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;

        [Space] [SerializeField] private Image image;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite highlightSprite;


        private void OnEnable()
        {
            SetInteractable(false);
            // AsButton(false);
            Highlight(false);

            EventManager.StartListening(EventName.Gameplay.ToTurn, OnToTurn);

            Networks.Network.Event.Fighting.onPhase += OnPhase;

            EventManager.StartListening(EventName.Gameplay.UI_RefreshPhase, RefreshStatus);

            // PhaseManager.current.onChanged += Refresh;
            Init();
            Refresh();

            button.onClick.AddListener(RequestChange);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventName.Gameplay.ToTurn, OnToTurn);
            Networks.Network.Event.Fighting.onPhase -= OnPhase;
            EventManager.StopListening(EventName.Gameplay.UI_RefreshPhase, RefreshStatus);


            // PhaseManager.current.onChanged -= Refresh;
            button.onClick.RemoveListener(RequestChange);
        }


        private void OnPhase(Response_OnPhase obj)
        {
            RefreshStatus();
        }


        private void OnToTurn()
        {
            // var @event = EventManager.GetData(EventName.Gameplay.ToTurn) as Event_ToTurn;

            var ownTurn = Networks.Network.Query.Fighting.IsOwnTurn;
            text.color = ownTurn ? Color.green : Color.red;
        }

        private void RefreshStatus()
        {
            var status = Client_DueManager.StatusOf(phase);

            var asButton = false;
            var interactable = false;

            switch (status)
            {
                case PhaseStatus.Highlight:
                    asButton = false;
                    interactable = true;
                    break;
                case PhaseStatus.Enable:
                    asButton = true;
                    interactable = true;
                    break;
                case PhaseStatus.Disable:
                    asButton = true;
                    interactable = false;
                    break;
            }

            Highlight(status is PhaseStatus.Highlight);
            AsButton(asButton);
            SetInteractable(interactable);
        }


        private void Init()
        {
            Highlight(false);

            text.SetText(phase switch
            {
                Phase.Draw => "D\nP",
                Phase.Standby => "S\nP",
                Phase.Main1 => "M\n1",
                Phase.Battle => "B\nP",
                Phase.Main2 => "M\n2",
                Phase.End => "E\nP",
                _ => String.Empty
            });
        }

        private void Refresh()
        {
            // var status = PhaseManager.current.status[phase];
            //
            // SetCurrent(manager.currentPhase == phase);
            // AsButton(status.asButton);
            // SetInteractable(status.interactable);
        }

        private void Highlight(bool isHighlight)
        {
            image.sprite = isHighlight ? highlightSprite : normalSprite;
        }

        private void AsButton(bool asButton)
        {
            button.enabled = asButton;
        }

        private void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }

        private void RequestChange()
        {
            Network.Request.Fighting.SwitchPhase(phase);
            // Server_PhaseManager.main.SwitchPhase(phase);
        }
    }
}