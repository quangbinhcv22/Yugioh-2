using UnityEngine;

public class Present_LifePoint : MonoBehaviour
{
    public static int selfLp;
    public static int opponentLp;


    private void OnEnable()
    {
        Gameplay_Event.On_NewRound += On_NewRound;
    }

    private void OnDisable()
    {
        Gameplay_Event.On_NewRound -= On_NewRound;
    }


    private void On_NewRound()
    {
    }

    private void UpdateImmediately()
    {
    }
}

public class Event_Change_LifePoint
{
    public enum Reason
    {
    }
}