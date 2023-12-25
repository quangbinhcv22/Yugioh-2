using System;
using System.Collections.Generic;
using battle.define;
using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using CardConfig = Networks.CardConfig;
using Network = Networks.Network;

namespace fake
{
    public class Tool_CardToSelectPanel : MonoBehaviour, IEnhancedScrollerDelegate
    {
        public EnhancedScroller scroller;
        public Tool_CardToSelectCell prefab;
        [FormerlySerializedAs("size")] public float cellSize = 100f;

        public List<CardConfig> configs;
        public CardType filterType = CardType.Monster;

        [Button] public Button btnMonster;
        public Button btnSpell;


        private void Awake()
        {
            btnMonster.onClick.AddListener(() => SwitchFilter(CardType.Monster));
            btnSpell.onClick.AddListener(() => SwitchFilter(CardType.Spell));
        }

        public void OnEnable()
        {
            Reload();
        }

        private void SwitchFilter(CardType type)
        {
            filterType = type;
            Reload();
        }

        private void Reload()
        {
            btnMonster.interactable = filterType is not CardType.Monster;
            btnSpell.interactable = filterType is not CardType.Spell;
            
            configs = Network.Query.Config.Get_CardByType(filterType);
            scroller.Delegate = this;
        }


        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return configs.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return cellSize;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cell = (Tool_CardToSelectCell)scroller.GetCellView(prefab);
            cell.SetData(configs[dataIndex]);
            
            return cell;
        }
    }
}