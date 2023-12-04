using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QBPlugins.ScreenFlow
{
    public class Screen : MonoBehaviour
    {
        [SerializeField] internal bool useOnce;

        public string Key => name;


        public UniTask PresentOpen()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public UniTask PresentClose()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}