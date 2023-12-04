using System.Collections.Generic;
using battle.view.card;

namespace battle.query
{
    public static class DueQuery_UI
    {
        private static readonly List<View_Card> ActivatingCards = new();


        public static void OnActive(View_Card card)
        {
            if (ActivatingCards.Contains(card)) return;
            ActivatingCards.Add(card);
        }

        public static void OnInactive(View_Card card)
        {
            ActivatingCards.Remove(card);
        }

        
        public static View_Card Get(string cardGuid)
        {
            foreach (var card in ActivatingCards)
            {
                if (card.Guid == cardGuid) return card;
            }

            return null;
        }
    }
}