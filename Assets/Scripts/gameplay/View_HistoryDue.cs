using System.Collections.Generic;
using battle.define;
using event_name;
using TigerForge;
using UnityEngine;
using UnityEngine.UI;

public class View_HistoryDue : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private Image[] slots;

    [SerializeField] private Transform currentHighlight;
    
    [Space] [SerializeField] private Sprite unsetSprite;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;


    private void OnEnable()
    {
        EventManager.StartListening(EventName.Gameplay.Present.UpdateRound, On_UpdateRound);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.Gameplay.Present.UpdateRound, On_UpdateRound);
    }

    private void On_UpdateRound()
    {
        var history = Client_DueManager.GetPlayer(team).history;
        SetHistory(history);
    }


    public void SetHistory(List<RoundResult> results)
    {
        for (int i = 0; i < results.Count; i++)
        {
            slots[i].sprite = GetSprite(results[i]);
        }


        var isEndGame = Server_DueManager.main.IsEndGame;
        currentHighlight.gameObject.SetActive(!isEndGame);

        
        if (!Server_DueManager.main.IsEndGame){
            
            var currentRound = Server_DueManager.main.CurrentRound;
            var currentAt = slots[currentRound - 1].transform;

            currentHighlight.gameObject.SetActive(true);
            currentHighlight.SetParent(currentAt);
            currentHighlight.localPosition = Vector3.zero;
        }
    }


    private Sprite GetSprite(RoundResult result)
    {
        return result switch
        {
            RoundResult.Win => winSprite,
            RoundResult.Lose => loseSprite,
            _ => unsetSprite,
        };
    }
}