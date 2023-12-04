using EnhancedUI.EnhancedScroller;
using Gameplay.card.ui;
using UnityEngine;

public class Cell_GraveyardCard : EnhancedScrollerCellView
{
    [SerializeField] private UI_Card card;
    
    public void SetData(string guid)
    {
        card.Binding(guid);
    }
}