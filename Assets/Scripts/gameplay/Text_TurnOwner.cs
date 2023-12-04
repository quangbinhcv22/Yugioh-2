using event_name;
using TigerForge;
using TMPro;
using UnityEngine;

public class Text_TurnOwner : MonoBehaviour
{
    private TMP_Text _text;
    public Color selfColor;
    public Color opponentColor;

    
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    
    private void OnEnable()
    {
        EventManager.StartListening(EventName.Gameplay.ToTurn, OnTurn);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.Gameplay.ToTurn, OnTurn);
    }

    
    private void OnTurn()
    {
        var turnOwner = Networks.Network.Query.Fighting.IsOwnTurn;
        
        _text.SetText(turnOwner ? "Your Turn" : "Opponent's Turn");
        _text.color = turnOwner ? selfColor : opponentColor;
    }
}