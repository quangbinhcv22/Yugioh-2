using System;
using Gameplay.card.ui;
using UnityEngine;

[DefaultExecutionOrder(int.MinValue)]
public class PresentHandler_SelectCard : Singleton<PresentHandler_SelectCard>
{
    public Action<string> onSelected;
    public string lastSelect;

    public void Select(string guid)
    {
        lastSelect = guid;
        onSelected?.Invoke(guid);
    }


    private ISourceCardStates _phase_stateSource;
    private ISourceCardStates _task_stateSource;
    private ISourceCardStates _stateSource;

    
    public void Set_StatesSource_Phase(ISourceCardStates source)
    {
        _phase_stateSource = source;
        _stateSource = _task_stateSource ?? _phase_stateSource;
        
        ReCalculateState();
    }
    
    public void Set_StatesSource_Task(ISourceCardStates source)
    {
        _task_stateSource = source;
        _stateSource = _task_stateSource ?? _phase_stateSource;

        ReCalculateState();
    }

    public void ReCalculateState()
    {
        var uiCards = DueCardQuery.GetAll_UICards();

        foreach (var uiCard in uiCards)
        {
            if(uiCard == null) continue;
            
            var guid = uiCard.Guid;
            var states =  _stateSource?.Get_CardStates(guid) ?? ButtonStates.LightUI;

            uiCard.selectable.SetStatus(states);
        }
    }
}

public interface ISourceCardStates
{
    public ButtonStates Get_CardStates(string cardGuid);
}