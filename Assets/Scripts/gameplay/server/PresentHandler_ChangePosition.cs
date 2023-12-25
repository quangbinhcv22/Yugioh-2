using battle.define;
using event_name;
using Gameplay.board;
using TigerForge;

public class PresentHandler_ChangePosition : Singleton<PresentHandler_ChangePosition>
{
    protected override void OnEnable()
    {
        base.OnEnable();

        Notifier_DueData.Current.event_changePosition += On_ChangePosition;
    }

    private void OnDisable()
    {
        Notifier_DueData.Current.event_changePosition -= On_ChangePosition;
    }

    private void On_ChangePosition(Event_ChangePosition data)
    {
        var uiSlot = DueCardQuery.GetUICard_CombatSpace(data.guid);
        uiSlot.ChangePosition(data.position);

        EventManager.EmitEvent(EventName.Gameplay.UI_RefreshPhase);
    }
}

public class Event_ChangePosition
{
    public Team team;
    public string guid;
    public MonsterPosition position;
}