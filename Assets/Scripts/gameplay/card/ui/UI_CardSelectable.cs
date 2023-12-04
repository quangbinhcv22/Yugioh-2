using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.card.ui
{
    public class UI_CardSelectable : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private GameObject highlightSignal;
        [SerializeField] private GameObject lockOverlay;

        [Button]
        public void SetStatus(ButtonStates status)
        {
            var isLight = status.HasFlag(ButtonStates.LightUI);
            var isInteractable = !status.HasFlag(ButtonStates.Lock);
            var isHighlight = status.HasFlag(ButtonStates.Highlight);

            if (highlightSignal) highlightSignal.SetActive(isHighlight);
            if (lockOverlay) lockOverlay.SetActive(!isLight);
            if (button) button.interactable = isInteractable;
        }
    }

    [Flags]
    public enum ButtonStates
    {
        Default = 0,
        LightUI = 1 << 0,
        Lock = 1 << 1,
        Highlight = 1 << 2,
    }
}