using battle.define;
using event_name;
using Gameplay;
using Gameplay.card.ui;
using Networks;
using TigerForge;
using UnityEngine;

public class CardAction_PhaseDraw : Singleton<CardAction_PhaseDraw>, ISourceCardStates
{
    protected override void OnEnable()
    {
        base.OnEnable();

        // PresentHandler_SelectCard.Current.onSelected += OnSelected;

        Networks.Network.Event.Fighting.onPhase += OnPhase_SV;
        
        PresentHandler_SummonMonster.Current.OnSummon += OnPhase;
        // PresentHandler_DrawCard.Current.onCheckOver += CheckOverCards;
    }

    protected void OnDisable()
    {
        // PresentHandler_SelectCard.Current.onSelected -= OnSelected;

        Networks.Network.Event.Fighting.onPhase -= OnPhase_SV;
        PresentHandler_SummonMonster.Current.OnSummon -= OnPhase;
        // PresentHandler_DrawCard.Current.onCheckOver -= CheckOverCards;
    }

    protected void OnPhase_SV(Response_OnPhase data)
    {
        OnPhase();
    }

    private void OnPhase()
    {
        // var isOwnTurn = Client_DueManager.IsOwnTurn;
        var currentPhase = Networks.Network.Query.Fighting.CurrentPhase;
        
        if (currentPhase is Phase.End)
        {
            PresentHandler_SelectCard.Current.Set_StatesSource_Phase(this);
            CheckOverCards();
        }
    }

    public ButtonStates Get_CardStates(string cardGuid)
    {
        return ButtonStates.Default;
    }


    private void CheckOverCards()
    {
        if (!Networks.Network.Query.Fighting.IsOwnTurn) return;

        var handZone = Client_DueManager.GetPlayer(Team.Self).zone.inHand;
        if (handZone.IsOver)
        {
            CardAction_DiscardCard.Current.StartProcess(handZone.OverAmount);
        }
    }
}