using System.Linq;
using battle.define;
using Gameplay;
using Gameplay.player;
using UnityEngine;

public class Client_DueManager : MonoBehaviour
{
    public static int myIndex;

    public static Player GetPlayer(Team team)
    {
        var serverManager = Server_DueManager.main;
        return team is Team.Self ? serverManager.GetPlayer(myIndex) : serverManager.GetOtherPlayer(myIndex);
    }
    
    public static RoundResult MyResult()
    {
        return GetPlayer(Team.Self).history.Count(h => h is RoundResult.Win) >= 2 ? RoundResult.Win : RoundResult.Lose;
    }

    public static bool IsMine(Team team, int index)
    {
        return (team is Team.Self && index == myIndex) || (team is Team.Opponent && index != myIndex);
    }

    public static bool IsSelf(int index)
    {
        return index == myIndex;
    }


    public static Team GetTeam(int playerIndex)
    {
        return IsSelf(playerIndex) ? Team.Self : Team.Opponent;
    }

    public static Team GetRemainingTeam(int playerIndex)
    {
        return IsSelf(playerIndex) ? Team.Opponent : Team.Self;
    }

    public static bool NormalSummonThisTurn => GetPlayer(Team.Self).normalSummonThisTurn;



    public static PhaseStatus StatusOf(Phase phase)
    {
        var currentPhase = Networks.Network.Query.Fighting.CurrentPhase;
        if (phase == currentPhase) return PhaseStatus.Highlight;
        if (!Networks.Network.Query.Fighting.IsOwnTurn) return PhaseStatus.Disable;

        if (phase is Phase.Draw)
        {
            return PhaseStatus.Disable;
        }

        if (phase is Phase.Standby)
        {
            return PhaseStatus.Disable;
        }

        if (phase is Phase.Main1)
        {
            return PhaseStatus.Disable;
        }

        if (phase is Phase.Battle)
        {
            // first turn
            if (Networks.Network.Query.Fighting.IsFirstTun) return PhaseStatus.Disable;

            var haveAttacker = GetPlayer(Team.Self).zone.mainMonster.HaveAttacker();
            if (haveAttacker && currentPhase is Phase.Main1) return PhaseStatus.Enable;

            return PhaseStatus.Disable;
        }

        if (phase is Phase.Main2)
        {
            if (currentPhase is Phase.Battle) return PhaseStatus.Enable;
            return PhaseStatus.Disable;
        }

        if (phase is Phase.End)
        {
            if (currentPhase is Phase.Main1 or Phase.Battle or Phase.Main2) return PhaseStatus.Enable;
            return PhaseStatus.Disable;
        }

        return PhaseStatus.Disable;
    }


    private void OnEnable()
    {
        myIndex = 0;
    }
}