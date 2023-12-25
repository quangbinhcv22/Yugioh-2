using battle.define;
using Gameplay.card.ui;
using Networks;
using UnityEngine;

namespace gameplay.server
{
    public class UI_ZoneField : MonoBehaviour
    {
        [SerializeField] public UI_Card card;
        [SerializeField] private Team team;

        private string CurrentGuid;


        private void OnEnable()
        {
            Networks.Network.Event.Fighting.SetField += SetField;
            Networks.Network.Event.Fighting.DestroyField += DestroyField;
        }


        private void OnDisable()
        {
            Networks.Network.Event.Fighting.SetField -= SetField;
            Networks.Network.Event.Fighting.DestroyField -= DestroyField;
        }


        private void SetField(Event_SetField data)
        {
            if (data.team != team) return;

            CurrentGuid = data.serverCard.Guid;
            
            card.gameObject.SetActive(true);
            card.Binding(data.serverCard.Guid);
        }

        private void DestroyField(Event_DestroyField data)
        {
            if (data.team != team || CurrentGuid != data.guid) return;
            Clear();
        }


        private void Clear()
        {
            card.gameObject.SetActive(false);
        }
    }
}