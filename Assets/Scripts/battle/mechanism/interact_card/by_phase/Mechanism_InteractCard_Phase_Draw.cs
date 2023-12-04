namespace battle.mechanism.interact_card.by_phase
{
    public class Mechanism_InteractCard_Phase_Draw : IMechanism_InteractCard
    {
        public bool InEnable(string cardGuid)
        {
            return false;
        }

        public void OnSelect_Any(string cardGuid)
        {
        }

        public void OnSelect_Enable(string cardGuid)
        {
        }
    }
}