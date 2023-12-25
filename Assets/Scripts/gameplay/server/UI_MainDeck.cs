using battle.define;
using event_name;
using gameplay.manager;
using TigerForge;
using TMPro;
using UnityEngine;

public class UI_MainDeck : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private TMP_Text amount;

    private void OnEnable()
    {
        EventManager.StartListening(EventName.Gameplay.StartGame, UpdateView);
        EventManager.StartListening(EventName.Gameplay.DrawDefault, UpdateView);
        EventManager.StartListening(EventName.Gameplay.Draw, UpdateView);

        DueManager.onSync_DeckCard += UpdateView;

        UpdateView();
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.Gameplay.StartGame, UpdateView);
        EventManager.StopListening(EventName.Gameplay.DrawDefault, UpdateView);
        EventManager.StopListening(EventName.Gameplay.Draw, UpdateView);
        
        DueManager.onSync_DeckCard -= UpdateView;
    }

    private void UpdateView()
    {
        var currentAmount = Client_DueManager.GetPlayer(team).zone.mainDeck.remain;
        amount.SetText($"{currentAmount}");
    }
}