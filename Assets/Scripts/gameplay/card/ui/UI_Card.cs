using System;
using battle.define;
using Cysharp.Threading.Tasks;
using event_name;
using Gameplay.card.core;
using Networks;
using Sirenix.OdinInspector;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UX;

namespace Gameplay.card.ui
{
    [Obsolete]
    public class UI_Card : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtName;
        [SerializeField] private TMP_Text txtLType;
        [SerializeField] private TMP_Text txtLevel;
        [SerializeField] private Image imgAttribute;
        [SerializeField] private StarsView levelView;
        [SerializeField] private Image illustration;
        [SerializeField] private Image illustrationTemp;
        [SerializeField] private TMP_Text txtTypes;
        [SerializeField] private TMP_Text txtLore;
        [SerializeField] private TMP_Text txtAtk;
        [SerializeField] private TMP_Text txtDef;
        [SerializeField] private CardTheme theme;
        [SerializeField] private Button button;

        [ShowInInspector, HideInEditorMode] public string Guid { get; set; }

        public UI_CardSelectable selectable { get; set; }

        private void Awake()
        {
            selectable = GetComponent<UI_CardSelectable>();
        }


        public void ViewOnly(string guild)
        {
            Guid = guild;

            if (DueCardQuery.IsAnonymousCard(guild))
            {
            }
            else
            {
                SetData(DueCardQuery.GetViewInfo(Guid));
            }
        }

        public void Binding(string guild)
        {
            Guid = guild;

            if (DueCardQuery.IsAnonymousCard(guild))
            {
            }
            else
            {
                SetData(DueCardQuery.GetViewInfo(Guid));
            }

            DueCardQuery.SetUICard(guild, this);
        }


        public bool IsBack => GetComponent<FlipCard>().IsBack;

        public void Flip(bool isOpen)
        {
            var flipper = GetComponent<FlipCard>();
            if (isOpen) flipper.Open();
            else flipper.Close();
        }

        public void Flip(bool isOpen, bool frontFace)
        {
            var flipper = GetComponent<FlipCard>();
            if (isOpen) flipper.Open(frontFace);
            else flipper.Close(frontFace);
        }

        public void ShowBack()
        {
            var flipper = GetComponent<FlipCard>();
            flipper.ShowBack();
        }

        public void ShowHide()
        {
            var flipper = GetComponent<FlipCard>();
            flipper.ShowFront();
        }


        public void UnBinding()
        {
            Guid = string.Empty;
        }


        private void Start()
        {
            if (button) button.onClick.AddListener(Select);
        }


        private async void SetData(CardConfig card)
        {
            // var typeCard = FakeConfig.GetType_ById(card.id);
            if (card.type == "MONSTER")
            {
                if (illustrationTemp) illustrationTemp.gameObject.SetActive(false);


                if (imgAttribute)
                {
                    var attKey = $"attribute_{card.monsterAttribute.ToLower()}";
                    imgAttribute.sprite = await Addressables.LoadAssetAsync<Sprite>(attKey);
                }

                if (illustration)
                    illustration.sprite = await Addressables.LoadAssetAsync<Sprite>(FakeConfig.GetIllusKey(card.code));
                if (txtName) txtName.SetText(card.name);
                if (txtLore) txtLore.SetText(card.desc);

                if (txtTypes) txtTypes.SetText(card.monsterType);

                if (txtAtk) txtAtk.SetText($"{card.atk}");
                if (txtDef) txtDef.SetText($"{card.def}");

                if (levelView) levelView.Set(card.level);
                if (txtLevel) txtLevel.SetText($"{card.level}");
            }
            // else
            // {
            //     if (illustrationTemp)
            //     {
            //         illustrationTemp.gameObject.SetActive(true);
            //         illustrationTemp.sprite = await Addressables.LoadAssetAsync<Sprite>(FakeConfig.GetIllusKey(card.id));
            //     }
            // }
        }

        private void Select()
        {
            EventManager.EmitEventData(EventName.SelectCard, this);
            PresentHandler_SelectCard.Current.Select(Guid);
        }
    }

    [Serializable]
    public class CardLocation
    {
        public const int NotFound = -1;

        public int playerIndex;
        public BoardZoneType zoneType;
        public int index;

        public bool IsValid => !(playerIndex is NotFound || zoneType is BoardZoneType.Unknown || index is NotFound);

        public Team OfTeam => Client_DueManager.GetTeam(playerIndex);
    }
}