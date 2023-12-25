using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

namespace fake
{
    public class ToolPanel_SelectingPresetCard : MonoBehaviour, IEnhancedScrollerDelegate
    {
        [SerializeField] public EnhancedScroller scroller;
        [SerializeField] public UICard_FixSize prefab;
        [SerializeField] public float cellSize = 100f;


        private List<string> _code;

        public void Reload(List<string> code)
        {
            _code = code;
            scroller.Delegate = this;
        }
        

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _code.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return cellSize;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cell = (UICard_FixSize)scroller.GetCellView(prefab);
            cell.main.ViewOnly_ByCode(_code[dataIndex]);

            return cell;
        }
    }
}