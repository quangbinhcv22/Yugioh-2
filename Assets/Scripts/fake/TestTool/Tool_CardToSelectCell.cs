using battle.define;
using EnhancedUI.EnhancedScroller;
using Gameplay.card.ui;
using TMPro;
using UnityEngine.UI;
using CardConfig = Networks.CardConfig;

namespace fake
{
    public class Tool_CardToSelectCell : EnhancedScrollerCellView
    {
        public UI_Card card;
        public TMP_Text txtMonsterType;
        public TMP_Text txtMonsterStat;
        public TMP_Text txtLore;
        public Button btnAdd;

        public string code;


        private void Awake()
        {
            btnAdd.onClick.AddListener(RequestAdd);
        }

        public void SetData(CardConfig config)
        {
            code = config.code;
            
            card.ViewOnly_ByCode(config.code);

            var isMonster = config.type == CardType.Monster.ToString().ToUpper();
            var isSpell = config.type == CardType.Spell.ToString().ToUpper();
            txtMonsterType.gameObject.SetActive(isMonster);
            txtMonsterStat.gameObject.SetActive(isMonster);
            txtLore.gameObject.SetActive(isSpell);

            if (isMonster)
            {
                txtMonsterType.SetText($"{config.monsterType} - {config.monsterAttribute}");
                txtMonsterStat.SetText($"ATK: {config.atk} / DEF: {config.def}");
            }
            else if (isSpell)
            {
                txtLore.SetText(config.desc);
            }
        }

        private void RequestAdd()
        {
            ToolPanel_PresetCard.singleton.AddFromPreset(code);
        }
    }
}