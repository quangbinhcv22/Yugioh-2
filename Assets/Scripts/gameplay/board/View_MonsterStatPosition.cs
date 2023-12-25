using battle.define;
using Gameplay.card;
using Networks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Gameplay.board
{
    public class View_MonsterStatPosition : MonoBehaviour
    {
        public static readonly Color normalColor = Color.white;
        public static readonly Color buffColor = Color.green;
        public static readonly Color deBuffColor = Color.red;

        public static Color GetColor(BuffState state)
        {
            if (state == BuffState.Normal) return normalColor;
            if (state == BuffState.Buff) return buffColor;
            if (state == BuffState.DeBuff) return deBuffColor;

            return normalColor;
        }

        [SerializeField] private TMP_Text atkText;
        [SerializeField] private TMP_Text defText;
        [SerializeField] private Color highlight;
        [SerializeField] private Color unHighlight;

        [ShowInInspector, HideInEditorMode] public string Guid { get; private set; }

        private void SetAttack(int atk)
        {
            atkText.SetText($"{atk}");
        }

        private void SetBuffState_Attack(BuffState state)
        {
            atkText.color = GetColor(state);
        }

        private void SetDefense(int def)
        {
            defText.SetText($"{def}");
        }

        private void SetBuffState_Defense(BuffState state)
        {
            defText.color = GetColor(state);
        }


        [Button]
        public void SetPosition(MonsterPosition position)
        {
            atkText.color = position is MonsterPosition.Attack ? highlight : unHighlight;
            defText.color = position is MonsterPosition.Defense ? highlight : unHighlight;
        }

        public void SetGuid(string cardGuild)
        {
            Guid = cardGuild;

            var info = DueCardQuery.GetBattleInfo(Guid);
            Refresh();
            SetPosition(info.position);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }


        private void OnEnable()
        {
            transform.eulerAngles = Vector3.zero;

            State_Stats.onChanged += OnChanged;

            Refresh();
        }


        private void OnDisable()
        {
            State_Stats.onChanged -= OnChanged;
        }


        private void OnChanged(CardEffect _)
        {
            Refresh();
        }

        private void Refresh()
        {
            // var atk = State_Stats.AttackOf(Guid);

            var atk = State_Stats.AttackOf(Guid);
            SetAttack(atk);

            SetBuffState_Attack(State_Stats.BuffState_Attack(Guid));

            // var def = State_Stats.DefOf(Guid);
            var def = State_Stats.DefenseOf(Guid);
            SetDefense(def);

            SetBuffState_Defense(State_Stats.BuffState_Defense(Guid));
        }
    }
}