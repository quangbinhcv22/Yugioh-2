using Sirenix.OdinInspector;
using UnityEngine;

namespace battle.view.card
{
    public class View_CardFace : MonoBehaviour
    {
        [SerializeField] private CanvasGroup up;
        [SerializeField] private CanvasGroup down;

        [Button]
        public void SetFace(CardFace face)
        {
            switch (face)
            {
                case CardFace.Up:
                    up.alpha = 1;
                    down.alpha = 0;
                    break;
                case CardFace.Down:
                    up.alpha = 0;
                    down.alpha = 1;
                    break;
            }
        }
    }
}

    