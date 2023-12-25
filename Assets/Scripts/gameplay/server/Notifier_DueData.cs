using System;
using battle.define;
using gameplay.present;
using Networks;
using UnityEngine;

[DefaultExecutionOrder(int.MinValue)]
public class Notifier_DueData : Singleton<Notifier_DueData>
{
    public Action<Event_Changed_LifePoint> changed_lifePoint;

    public void Changed_lifePoint(Event_Changed_LifePoint @event)
    {
        changed_lifePoint?.Invoke(@event);
    }


    public Action<Event_Attack> event_attack;

    public void Event_Attack(Event_Attack @event)
    {
        event_attack?.Invoke(@event);
    }


    public Action<Event_SummonMonster> event_summonMonster;

    public void Event_SummonMonster(Event_SummonMonster @event)
    {
        event_summonMonster?.Invoke(@event);
    }


    public Action<Event_ChangePosition> event_changePosition;

    public void Event_ChangePosition(Event_ChangePosition @event)
    {
        event_changePosition?.Invoke(@event);
    }


    public Action<Event_SetSpell> event_SetSpell;

    public void Event_SetSpell(Event_SetSpell @event)
    {
        event_SetSpell?.Invoke(@event);
    }
}

public class Event_SetSpell
{
    public Team team;
    public string guid;
    public int index;
}