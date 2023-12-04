using event_name;
using Gameplay.card.ui;
using TigerForge;

public class ActionCard_Wait : Singleton<ActionCard_Wait>, ISourceCardStates
{
    protected override void OnEnable()
    {
        base.OnEnable();

        EventManager.StartListening(EventName.Gameplay.ToTurn, OnTurn);
        OnTurn();
    }

    protected void OnDisable()
    {
        EventManager.StopListening(EventName.Gameplay.ToTurn, OnTurn);
    }

    private void OnTurn()
    {
        PresentHandler_SelectCard.Current.Set_StatesSource_Phase(this);
    }

    public ButtonStates Get_CardStates(string cardGuid)
    {
        return ButtonStates.Default;
    }
}