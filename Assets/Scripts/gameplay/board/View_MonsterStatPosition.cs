using battle.define;
using Gameplay.card;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Gameplay.board
{
    public class View_MonsterStatPosition : MonoBehaviour
    {
        [SerializeField] private TMP_Text atkText;
        [SerializeField] private TMP_Text defText;
        [SerializeField] private Color highlight;
        [SerializeField] private Color unHighlight;
        
        [ShowInInspector, HideInEditorMode] public string Guid { get; private set; }

        private void SetAttack(int atk)
        {
            atkText.SetText($"{atk}");
        }

        private void SetDefense(int def)
        {
            defText.SetText($"{def}");
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

            State_Stats.onAdd += OnChanged;
        }



        private void OnDisable()
        {
            State_Stats.onAdd -= OnChanged;
        }

        private void OnChanged(Team arg1, BuffStat arg2)
        {
            Refresh();
        }
        
        private void Refresh()
        {
            // var atk = State_Stats.AttackOf(Guid);
            
            var atk = DueCardQuery.GetViewInfo(Guid).atk;
            SetAttack(atk);
            
            // var def = State_Stats.DefOf(Guid);
            var def = DueCardQuery.GetViewInfo(Guid).def;
            SetDefense(def);
        }
    }
}