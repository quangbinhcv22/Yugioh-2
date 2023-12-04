using System;
using UnityEngine;

public class NewCard : MonoBehaviour
{
}

public interface IActivateCondition
{
    bool CanActivate();
}

public interface IActivateCost
{
    Action onCostDone { get; }
}

public interface IActionTargeting
{
    Action OnTargetingDone { get; set; }
    TargetingFilters TargetingFilter { get; }
    void StartTargeting();
    void CancelTargeting();
}

public interface IActionMain
{
    void ExecuteAction();
}


public class SpellCardUser : MonoBehaviour
{
    public object card;

    public void Use()
    {
        if (card is IActivateCondition activateCondition)
        {
            if (!activateCondition.CanActivate()) return;
        }

        if (card is IActivateCost activateCost)
        {
        }
    }

    public void Do_ActionTargeting()
    {
        if (card is IActionTargeting actionTargeting)
        {
            actionTargeting.StartTargeting();
            actionTargeting.OnTargetingDone += ExecuteAction;
        }
        else
        {
            ExecuteAction();
        }
    }

    public void ExecuteAction()
    {
        if (card is IActionTargeting actionTargeting)
        {
            actionTargeting.OnTargetingDone -= ExecuteAction;
        }
        
        
    }
}


public class SpellCard_05318639 : IActivateCondition, IActionTargeting, IActionMain
{
    public bool CanActivate()
    {
        return true;
    }


    public Action OnTargetingDone { get; set; }

    public TargetingFilters TargetingFilter { get; } = new()
    {
        owners = Filter_Owners.Opponent,
        cardTypes = Filter_CardTypes.SpellOrTrap,
        zones = Filter_Zones.SpellAndTrap,
    };

    private string _selectedCard;


    public void StartTargeting()
    {
        throw new System.NotImplementedException();
    }

    public void CancelTargeting()
    {
        throw new System.NotImplementedException();
    }


    public void ExecuteAction()
    {
        GameNetwork.RequestDestroy(_selectedCard, this);
        GameNetwork.RequestUseAndToGraveyard(this);
    }
}

public class GameNetwork
{
    public static void RequestDestroy(string cardGuid, object source)
    {
        Debug.Log($"Destroy {cardGuid} by {source}");
    }

    public static void RequestUseAndToGraveyard(object card)
    {
        Debug.Log($"{card} used, to graveyard");
    }
}


public struct TargetingFilters
{
    public Filter_Owners owners;
    public Filter_CardTypes cardTypes;
    public Filter_Zones zones;
}


[Flags]
public enum Filter_Owners
{
    Self = 1 << 0,
    Opponent = 1 << 1,

    Anyone = Self | Opponent,
}

[Flags]
public enum Filter_CardTypes
{
    NormalMonster = 1 << 0,
    EffectMonster = 1 << 1,
    Spell = 1 << 2,
    Trap = 1 << 3,

    SpellOrTrap = Spell | Trap,
    All = NormalMonster | EffectMonster | Spell | Trap,
}

[Flags]
public enum Filter_Zones
{
    MainDeck = 1 << 0,
    InHand = 1 << 1,
    MainMonster = 1 << 2,
    SpellAndTrap = 1 << 3,
    Field = 1 << 4,
    Graveyard = 1 << 5,
    ExtraDeck = 1 << 6,
    ExtraMonster = 1 << 7,

    All = MainDeck | InHand | MainMonster | SpellAndTrap | Field | Graveyard | ExtraDeck | ExtraMonster,
}