using UnityEngine;
using UnityEngine.UI;

namespace fake
{
    public class OpenToolCardPresetInHand : MonoBehaviour
    {
        public Button button;
        public GameObject panel;

        private void Awake()
        {
            button.onClick.AddListener(OpenPanel);
        }

        private void OpenPanel()
        {
            panel.SetActive(true);
        }
    }
}