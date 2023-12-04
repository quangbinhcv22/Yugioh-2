using battle.query;
using Gameplay.board;

namespace battle.mechanism.interact_card.by_task
{
    public class Mechanism_InteractCard_Task_SummonTribute : IMechanism_InteractCard
    {
        private string _forGuid;
        private MonsterPosition _position;
        private int _tributeAmount;

        public Mechanism_InteractCard_Task_SummonTribute(string forGuid, MonsterPosition position)
        {
            _forGuid = forGuid;
            _position = position;
            // _tributeAmount = DueQuery_Card.GetTributeRequire(forGuid);
        }


        public bool InEnable(string cardGuid)
        {
            throw new System.NotImplementedException();
        }

        public void OnSelect_Any(string cardGuid)
        {
            throw new System.NotImplementedException();
        }

        public void OnSelect_Enable(string cardGuid)
        {
            throw new System.NotImplementedException();
        }
    }
}