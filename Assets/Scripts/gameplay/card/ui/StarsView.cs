using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.card.ui
{
    public class StarsView : MonoBehaviour
    {
        public GameObject prefab;
        private readonly List<GameObject> _elements = new();


        private void Awake()
        {
            _elements.Add(prefab);
        }

        [Button]
        public void Set(int number)
        {
            for (int i = 1; i <= Mathf.Max(number, _elements.Count); i++)
            {
                var isActive = number >= i;
                var isExist = _elements.Count >= i;
                var index = i - 1;

                if (isExist)
                {
                    _elements[index].SetActive(isActive);
                }
                else if (isActive)
                {
                    _elements.Add(Instantiate(prefab, transform));
                }
            }
        }
    }
}