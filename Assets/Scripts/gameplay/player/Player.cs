using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay.player
{
    [Serializable]
    public class Player
    {
        public static readonly List<RoundResult> HistoryDefault = new() { RoundResult.Unset, RoundResult.Unset, RoundResult.Unset };

        // public int avatar;
        // public string name;
        public List<RoundResult> history = HistoryDefault.ToList();
        public int hp = DueConstant.hp;
        public bool normalSummonThisTurn;

        public BoardZone zone = new();




        // public string AvatarKey => $"avatar_{avatar}";
        

        public void SetHistory(int round, RoundResult result)
        {
            history[round - 1] = result;
        }
    }
}