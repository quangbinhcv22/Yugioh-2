using System.Collections.Generic;
using battle.define;
using Gameplay.card.ui;
using gameplay.present;
using Networks;

public class CardAction_PhaseBattle : Singleton<CardAction_PhaseBattle>, ISourceCardStates
{
    public List<string> attackedGuids = new();


    protected override void OnEnable()
    {
        base.OnEnable();

        Network.Event.Fighting.onPhase += OnPhase;
    }

    private void OnDisable()
    {
        Network.Event.Fighting.onPhase -= OnPhase;
    }

    private void OnPhase(Response_OnPhase data)
    {
        var currentPhase = Network.Query.Fighting.CurrentPhase;

        if (currentPhase is Phase.Battle)
        {
            PresentHandler_SelectCard.Current.Set_StatesSource_Phase(this);
            if (Network.Query.Fighting.IsOwnTurn) StartSelectAttack();
        }
    }

    private void StartSelectAttack()
    {
        attackedGuids = new();

        Input_AttackSelector.Current.StartProcess();

        PresentHandler_Attack.Current.presentCompleted += PresentCompleted;
    }

    private void PresentCompleted()
    {
        var attackerAmount = Client_DueManager.GetPlayer(Team.Self).zone.mainMonster.AttackerAmount();

        // nếu qua round mới thì hủy luôn

        if (attackedGuids.Count != attackerAmount)
        {
            Input_AttackSelector.Current.StartProcess();
        }
        else
        {
            PresentHandler_Attack.Current.presentCompleted -= PresentCompleted;
        }
    }

    public ButtonStates Get_CardStates(string cardGuid)
    {
        return ButtonStates.Default;
    }
}