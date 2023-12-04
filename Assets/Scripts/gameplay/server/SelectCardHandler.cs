using event_name;
using Gameplay;
using Gameplay.card.ui;
using TigerForge;
using UI;
using UnityEngine;

public class SelectCardHandler : MonoBehaviour
{
    // private void OnEnable()
    // {
    //     EventManager.StartListening(EventName.SelectCard, OnSelect);
    // }
    //
    // private void OnDisable()
    // {
    //     EventManager.StopListening(EventName.SelectCard, OnSelect);
    // }
    //
    //
    // public void OnSelect()
    // {
    //     var ownTurn = Client_DueManager.IsOwnTurn;
    //
    //     var card = EventManager.GetData(EventName.SelectCard) as UI_Card;
    //     var currentPhase = Client_DueManager.CurrentPhase;
    //
    //     var location = DueCardQuery.Locate(card.Guid);
    //     var isMine = location.playerIndex == Client_DueManager.myIndex;
    //
    //     // if (isMine)
    //     // {
    //     //     UI_CardDetail.Show(card.Guid);
    //     // }
    //     
    //     if (ownTurn && isMine && location.zoneType is BoardZoneType.InHand && currentPhase is Phase.Main1 or Phase.Main2)
    //     {
    //         Panel_CardOptions.Show(card);
    //     }
    // }
}

public class CardSelection
{
}