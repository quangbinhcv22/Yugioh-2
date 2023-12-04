using battle.define;
using UnityEngine;

public class UI_ResultRound : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private int index;

    [Space] [SerializeField] private GameObject winSignal;
    [SerializeField] private GameObject loseSignal;

    private void OnEnable()
    {
        UpdateView();
    }
    
    private void UpdateView()
    {
        var player = Client_DueManager.GetPlayer(team);
        var result = player.history[index];

        winSignal.SetActive(result is RoundResult.Win);
        loseSignal.SetActive(result is RoundResult.Lose);
    }
}