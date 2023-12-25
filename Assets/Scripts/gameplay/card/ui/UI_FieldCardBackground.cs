using Cysharp.Threading.Tasks;
using Networks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Gameplay.card.ui
{
    public class UI_FieldCardBackground : MonoBehaviour
    {
        [SerializeField] private Image allZone;
        [SerializeField] private Image selfZone;
        [SerializeField] private Image opponentZone;

        private void OnEnable()
        {
            Networks.Network.Event.Fighting.SetField += SetField;

            Clear();
        }

        private void OnDisable()
        {
            Networks.Network.Event.Fighting.SetField -= SetField;
        }


        private async void SetField(Event_SetField data)
        {
            var selfField = Server_DueManager.main.self.zone.field;
            var opnField = Server_DueManager.main.opponent.zone.field;

            var haveAll = selfField.IsActive() && opnField.IsActive();

            if (haveAll)
            {
                allZone.gameObject.SetActive(false);
                selfZone.gameObject.SetActive(true);
                opponentZone.gameObject.SetActive(true);
                
                selfZone.sprite = await Addressables.LoadAssetAsync<Sprite>(FakeConfig.GetIllusKey(selfField.space.card.code));
                opponentZone.sprite = await Addressables.LoadAssetAsync<Sprite>(FakeConfig.GetIllusKey(opnField.space.card.code));
            }
            else if(selfField.IsActive())
            {
                allZone.gameObject.SetActive(true);
                selfZone.gameObject.SetActive(false);
                opponentZone.gameObject.SetActive(false);
                
                allZone.sprite = await Addressables.LoadAssetAsync<Sprite>(FakeConfig.GetIllusKey(selfField.space.card.code));
            }
            else if(opnField.IsActive())
            {
                allZone.gameObject.SetActive(true);
                selfZone.gameObject.SetActive(false);
                opponentZone.gameObject.SetActive(false);
                
                allZone.sprite = await Addressables.LoadAssetAsync<Sprite>(FakeConfig.GetIllusKey(opnField.space.card.code));
            }
            else
            {
                Clear();
            }
        }

        private void Clear()
        {
            allZone.gameObject.SetActive(false);
            selfZone.gameObject.SetActive(false);
            opponentZone.gameObject.SetActive(false);
        }
    }
}