using battle.define;
using event_name;
using TigerForge;
using TMPro;
using UnityEngine;
using Network = Networks.Network;

public class UI_PlayerName : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private TMP_Text amount;
    
    private void OnEnable()
    {
        EventManager.StartListening(EventName.Gameplay.StartGame, UpdateView);
        UpdateView();
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.Gameplay.StartGame, UpdateView);
    }

    private void UpdateView()
    {
        var info = Network.Query.Fighting.GetTeam(team);
        
        var value = info.displayName;
        amount.SetText($"{value}");
    }
}