using battle.define;
using event_name;
using Gameplay;
using Gameplay.card.ui;
using TigerForge;
using UnityEngine;

public class Action_ChangeMonsterPosition : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.StartListening(EventName.SelectCard, OnSelect);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.SelectCard, OnSelect);
    }

    private void OnSelect()
    {
        var ownTurn = Networks.Network.Query.Fighting.IsOwnTurn;
        if (!ownTurn) return;

        var currentPhase = Networks.Network.Query.Fighting.CurrentPhase;
        if (currentPhase is not Phase.Main1 or Phase.Main2) return;

        var card = EventManager.GetData(EventName.SelectCard) as UI_Card;

        var location = DueCardQuery.Locate(card.Guid);
        var isMine = location.playerIndex == Client_DueManager.myIndex;
        if (!isMine) return;


        var battleInfo = DueCardQuery.GetBattleInfo(card.Guid);

        // quái thú chưa tấn công
        // quái thú còn lượt
        // thỏa điều kiện
    }
}