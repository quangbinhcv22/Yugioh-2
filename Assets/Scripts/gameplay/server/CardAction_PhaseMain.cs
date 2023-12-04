using System.Collections.Generic;
using battle.define;
using event_name;
using Gameplay;
using Gameplay.card.ui;
using Networks;
using TigerForge;
using UnityEngine;

[DefaultExecutionOrder(int.MinValue + 2)]
public class CardAction_PhaseMain : Singleton<CardAction_PhaseMain>, ISourceCardStates
{
    public string selecting { get; set; }
    public List<string> changePositionCards = new();

    public List<string> summonThisTurn_Cards = new();
    public List<string> wasAttacker = new();


    protected override void OnEnable()
    {
        base.OnEnable();

        PresentHandler_SelectCard.Current.onSelected += OnSelected;

        Networks.Network.Event.Fighting.onPhase += OnPhase_SV;
        PresentHandler_SummonMonster.Current.OnSummon += OnPhase;

        EventManager.StartListening(EventName.Gameplay.ToTurn, OnTurn);
    }

    private void OnTurn()
    {
        summonThisTurn_Cards.Clear();
    }


    protected void OnDisable()
    {
        PresentHandler_SelectCard.Current.onSelected -= OnSelected;

        Networks.Network.Event.Fighting.onPhase -= OnPhase_SV;
        PresentHandler_SummonMonster.Current.OnSummon -= OnPhase;

        EventManager.StopListening(EventName.Gameplay.ToTurn, OnTurn);
    }


    private void OnPhase_SV(Response_OnPhase data)
    {
        OnPhase();
    }
    
    private void OnPhase()
    {
        var isOwnTurn = Networks.Network.Query.Fighting.IsOwnTurn;
        var currentPhase = Networks.Network.Query.Fighting.CurrentPhase;

        if (isOwnTurn && currentPhase is Phase.Main1 or Phase.Main2)
        {
            if (currentPhase is Phase.Main1)
            {
                CardAction_PhaseBattle.Current.attackedGuids.Clear();
                changePositionCards.Clear();
            }

            PresentHandler_SelectCard.Current.Set_StatesSource_Phase(this);
        }
    }

    private void OnSelected(string cardGuid)
    {
        if (CardAction_SummonTribute.Current.InProcess) return;


        var isOwnTurn = Networks.Network.Query.Fighting.IsOwnTurn;
        var currentPhase = Networks.Network.Query.Fighting.CurrentPhase;

        var location = DueCardQuery.Locate(cardGuid);
        var isMine = location.OfTeam is Team.Self;

        if (isOwnTurn && isMine && currentPhase is Phase.Main1 or Phase.Main2)
        {
            // Debug.Log("Select");
            selecting = cardGuid;
            var typeCard = FakeConfig.GetType_ByGuid(cardGuid);

            if (location.zoneType is BoardZoneType.InHand)
            {
                if (typeCard is CardType.Monster)
                {
                    if (CanNormalSummon(cardGuid, out var errorReason))
                    {
                        Old_Panel_CardOptions.Current.Show(cardGuid).UseAction_MonsterInHand();
                    }
                    else
                    {
                        Old_Panel_CardOptions.Current.Show(cardGuid).ShowText(errorReason);
                    }
                }
                else if (typeCard is CardType.Spell)
                {
                    Old_Panel_CardOptions.Current.Show(cardGuid).UseAction_SpellInHand();
                }
            }
            else if (location.zoneType is BoardZoneType.MainMonster)
            {
                if (typeCard is CardType.Monster)
                {
                    if (CanChangePosition(cardGuid, out var errorReason))
                    {
                        Old_Panel_CardOptions.Current.Show(cardGuid).UseAction_MonsterInBoard();
                    }
                    else
                    {
                        Old_Panel_CardOptions.Current.Show(cardGuid).ShowText(errorReason);
                    }
                }
                else if (typeCard is CardType.Spell)
                {
                }
            }
        }

        PresentHandler_SelectCard.Current.ReCalculateState();
    }


    public ButtonStates Get_CardStates(string cardGuid)
    {
        var canExecute = CanExecuteOption(cardGuid);
        var isSelecting = cardGuid == selecting;
        var locate = DueCardQuery.Locate(cardGuid);
        var isMine = locate.playerIndex == Client_DueManager.myIndex;

        var returnStates = ButtonStates.Default;
        if (isSelecting) returnStates |= ButtonStates.Highlight;

        if (canExecute) returnStates |= ButtonStates.LightUI;

        return returnStates;
    }


    public bool CanExecuteOption(string cardGuid)
    {
        var locate = DueCardQuery.Locate(cardGuid);
        var isMine = locate.playerIndex == Client_DueManager.myIndex;

        if (!isMine) return false;


        var type = FakeConfig.GetType_ByGuid(cardGuid);

        if (type is CardType.Monster)
        {
            if (locate.zoneType is BoardZoneType.InHand)
            {
                return CanNormalSummon(cardGuid, out _);
            }
            else if (locate.zoneType is BoardZoneType.MainMonster)
            {
                return CanChangePosition(cardGuid, out _);
            }
        }
        else
        {
            return true;
        }


        return false;
    }


    private bool CanChangePosition(string cardGuid, out string reason)
    {
        if (summonThisTurn_Cards.Contains(cardGuid))
        {
            reason = "The monster appearing on the field in this turn can’t be changed position";
            return false;
        }

        var isAttacked = CardAction_PhaseBattle.Current.attackedGuids.Contains(cardGuid);

        if (isAttacked)
        {
            reason = "The monster has attacked, can’t change its position";
            return false;
        }

        var isChanged = changePositionCards.Contains(cardGuid);
        if (isChanged)
        {
            reason = "The monster has been changed position";
            return false;
        }

        reason = string.Empty;

        return true;
    }

    private bool CanNormalSummon(string cardGuid, out string reason)
    {
        var self = Client_DueManager.GetPlayer(Team.Self);


        if (Client_DueManager.NormalSummonThisTurn)
        {
            reason = "Can’t execute normal summon more in this turn";
            return false;
        }


        if (self.zone.mainMonster.IsFull())
        {
            reason = "Your monster zone is full of cards";
            return false;
        }


        var requiredTributeAmount = DueCardQuery.GetTributeRequire(cardGuid);
        var isSummonTribute = requiredTributeAmount > 0;

        if (isSummonTribute && self.zone.mainMonster.Amount < requiredTributeAmount)
        {
            // var missing = requiredTributeAmount - self.zone.mainMonster.Amount;
            reason = $"Need {requiredTributeAmount} tribute monster(s) on the field to summon this monster";

            return false;
        }


        reason = string.Empty;
        return true;
    }
}