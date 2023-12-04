using battle.define;
using event_name;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Graveyard : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private TMP_Text amount;
    [SerializeField] private Button button;


    private void Awake()
    {
        button.onClick.AddListener(OpenScreen);
    }
    
    private void OpenScreen()
    {
        Screen_Graveyard.Current.Show(team);
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventName.Gameplay.StartGame, UpdateView);
        EventManager.StartListening(EventName.Gameplay.ToGraveyard, UpdateView);

        UpdateView();
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.Gameplay.StartGame, UpdateView);
        EventManager.StopListening(EventName.Gameplay.ToGraveyard, UpdateView);
    }

    private void UpdateView()
    {
        var currentAmount = Client_DueManager.GetPlayer(team).zone.graveyard.cards.Count;
        amount.SetText($"{currentAmount}");
    }
}