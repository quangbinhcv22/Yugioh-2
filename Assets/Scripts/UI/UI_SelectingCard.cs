using System.Collections.Generic;
using event_name;
using Gameplay.card.ui;
using TigerForge;
using UnityEngine;

namespace UI
{
    public class UI_SelectingCard : MonoBehaviour
    {
        private static UI_SelectingCard _main;

        [Header("Card")] [SerializeField] private UI_Card cardSmall;
        [SerializeField] private UI_Card cardInfo;

        [Space] [SerializeField] private List<GameObject> validActives;
        [SerializeField] private List<GameObject> nullActives;

        private void OnEnable()
        {
            EventManager.StartListening(EventName.SelectCard, OnSelect);
            UpdateDefault();
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventName.SelectCard, OnSelect);
        }


        private void OnSelect()
        {
            var selectedCard = EventManager.GetData(EventName.SelectCard) as UI_Card;
            var locate = DueCardQuery.Locate(selectedCard.Guid);

            if (Client_DueManager.IsSelf(locate.playerIndex))
            {
                UpdateView(selectedCard.Guid);
            }
            else if (selectedCard.IsBack)
            {
                UpdateView(selectedCard.Guid);
            }
            else
            {
                UpdateDefault();
            }
        }

        private void UpdateView(string guid)
        {
            validActives.ForEach(a => a.SetActive(true));
            nullActives.ForEach(a => a.SetActive(false));

            cardSmall.ShowBack();

            cardSmall.ViewOnly(guid);
            cardInfo.ViewOnly(guid);
        }

        private void UpdateDefault()
        {
            validActives.ForEach(a => a.SetActive(false));
            nullActives.ForEach(a => a.SetActive(true));

            cardSmall.ShowHide();
        }
    }
}