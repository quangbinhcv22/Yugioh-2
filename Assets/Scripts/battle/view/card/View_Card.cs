using System;
using battle.query;
using UnityEngine;

namespace battle.view.card
{
    public class View_Card : MonoBehaviour
    {
        // hướng, lật, stat text phụ thuộc vào:
        // - có dữ liệu không?
        // - bài bản thân hay đối thủ
        // - bị lật: bị tấn công, phép thuật
        // - hiện tại/quá khứ từng là công


        public string Guid { get; set; }

        public void Binding(string cardGuid)
        {
            Guid = cardGuid;
        }

        public void ViewOnly(string cardGuid)
        {
            throw new NotImplementedException();
        }


        public void ChangeFace(CardFace face)
        {
        }


        private void OnEnable()
        {
            DueQuery_UI.OnActive(this);

            Mechanism_InteractCard.refreshInteract += RefreshInteract;
        }

        private void OnDisable()
        {
            DueQuery_UI.OnInactive(this);

            Mechanism_InteractCard.refreshInteract -= RefreshInteract;
        }


        private void RefreshInteract()
        {
            throw new NotImplementedException();
        }
    }
}