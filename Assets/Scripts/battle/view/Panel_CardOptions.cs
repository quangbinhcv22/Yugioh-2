using System;
using System.Linq;
using battle.mechanism.interact_card.by_task;
using battle.network;
using battle.query;
using Gameplay.board;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace battle.view
{
    public class Panel_CardOptions : Singleton<Panel_CardOptions>
    {
        [Space] [SerializeField] private Button btn_SummonAttack;
        [SerializeField] private Button btn_SummonDefense;
        [SerializeField] private Button btn_ChangePosition;

        [Space] [SerializeField] private TMP_Text txtReason;
        [Space] [SerializeField] private Vector3 offset = new(0, 200f, 0);


        public static string ForGuid { get; private set; }


        private void Awake()
        {
            btn_SummonAttack.onClick.AddListener(() => OnClick_Summon(MonsterPosition.Attack));
            btn_SummonDefense.onClick.AddListener(() => OnClick_Summon(MonsterPosition.Defense));
            btn_ChangePosition.onClick.AddListener(OnClick_ChangePosition);
        }


        public static Panel_CardOptions ShowAt(string cardGuid)
        {
            Current.Show(cardGuid);
            return Current;
        }

        private void Show(string cardGuid)
        {
            gameObject.SetActive(true);

            txtReason.gameObject.SetActive(false);
            btn_SummonAttack.gameObject.SetActive(false);
            btn_SummonDefense.gameObject.SetActive(false);
            btn_ChangePosition.gameObject.SetActive(false);


            var uiSlot = DueQuery_UI.Get(cardGuid);
            transform.position = uiSlot.transform.position + offset;

            ForGuid = cardGuid;
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }


        public void UseText(string content)
        {
            txtReason.gameObject.SetActive(true);
            txtReason.SetText(content);
        }

        public void UseOptions_Custom(params CardOption[] options)
        {
            btn_SummonAttack.gameObject.SetActive(options.Contains(CardOption.SummonAttack));
            btn_SummonDefense.gameObject.SetActive(options.Contains(CardOption.SummonDefense));
            btn_ChangePosition.gameObject.SetActive(options.Contains(CardOption.ChangePosition));
        }

        public void UseOptions_NormalSummon()
        {
            btn_SummonAttack.gameObject.SetActive(true);
            btn_SummonDefense.gameObject.SetActive(true);
        }
        
        public void UseOptions_ChangePosition()
        {
            btn_ChangePosition.gameObject.SetActive(true);
        }


        private void OnClick_Summon(MonsterPosition position)
        {
            // if (DueQuery_Card.RequiredTribute(ForGuid))
            // {
            //     var selectTask = new Mechanism_InteractCard_Task_SummonTribute(ForGuid, position);
            //     Mechanism_InteractCard.StartTask(selectTask);
            // }
            // else
            // {
            //     DueNetwork.Request_NormalSummon(ForGuid, position);
            // }

            Hide();
        }

        private void OnClick_ChangePosition()
        {
            throw new NotImplementedException();
            Hide();
        }
    }

    public enum CardOption
    {
        SummonAttack,
        SummonDefense,
        ChangePosition,
        Active,
        Set,
    }
}