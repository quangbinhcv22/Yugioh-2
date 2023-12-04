using System.Collections.Generic;
using battle.define;
using EnhancedUI.EnhancedScroller;
using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Graveyard : Singleton<Screen_Graveyard>, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] private Cell_GraveyardCard cellPrefab;
    [SerializeField] private float cellSize = 100f;

    // [Space] [SerializeField] private TMP_Text countTxt;

    [Space] [SerializeField] private Button backdrop;
    // [SerializeField] private Button closeBtn;

    private void Awake()
    {
        backdrop.onClick.AddListener(Hide);
        // closeBtn.onClick.AddListener(Hide);
    }


    public void Show(Team team)
    {
        gameObject.SetActive(true);

        _cards = Client_DueManager.GetPlayer(team).zone.graveyard.cards;
        ReloadView();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    private void ReloadView()
    {
        scroller.Delegate = this;
        // countTxt.SetText($"Total: {_cards.Count}");
    }


    private List<ServerCard> _cards = new();


    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _cards.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cell = (Cell_GraveyardCard)scroller.GetCellView(cellPrefab);
        cell.SetData(_cards[dataIndex].Guid);

        return cell;
    }
}