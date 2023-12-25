using System;
using System.Collections.Generic;
using battle.define;
using Cysharp.Threading.Tasks;
using event_name;
using Networks;
using Sirenix.OdinInspector;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UX;
using Network = Networks.Network;

namespace Gameplay.card.ui
{
    public class UI_Card : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtName;
        [SerializeField] private TMP_Text txtLType;
        [SerializeField] private TMP_Text txtCardType;
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
        [SerializeField] private Image background;

        [SerializeField] private List<GameObject> monsterObjs;

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

        public void ViewOnly_ByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
            }
            else
            {
                SetData(Network.Query.Config.GetCard(code));
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

        public void OnRevealed(string code)
        {
            Debug.Log(Guid, gameObject);
            ViewOnly_ByCode(code);
            // SetData(DueCardQuery.GetViewInfo(Guid));
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

        public void HandleChangePosition_OnBack()
        {
            ViewOnly(Guid);
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
            if (DueCardQuery.IsAnonymousCode(card.code)) return;

            var cardType = Networks.Network.Query.Config.GetType_ByCode(card.code);


            var isMonster = cardType is CardType.Monster;

            if (imgAttribute) imgAttribute.gameObject.SetActive(isMonster);
            if (txtAtk) txtAtk.gameObject.SetActive(isMonster);
            if (txtDef) txtDef.gameObject.SetActive(isMonster);
            if (levelView) levelView.gameObject.SetActive(isMonster);
            if (txtLevel) txtLevel.gameObject.SetActive(isMonster);

            if (isMonster)
            {
                if (imgAttribute)
                {
                    var attKey = $"attribute_{card.monsterAttribute.ToLower()}";
                    imgAttribute.sprite = await Addressables.LoadAssetAsync<Sprite>(attKey);

                    if (txtAtk) txtAtk.SetText($"{card.atk}");
                    if (txtDef) txtDef.SetText($"{card.def}");
                    if (levelView) levelView.Set(card.level);
                    if (txtLevel) txtLevel.SetText($"{card.level}");
                }
            }

            monsterObjs.ForEach(o => o.SetActive(isMonster));

            LoadIllus(card);

            if (txtCardType)
            {
                txtCardType.SetText((cardType) switch
                {
                    CardType.Monster => "NORMAL",
                    CardType.Spell => "SPELL",
                    _ => "",
                });
            }



            if (txtName) txtName.SetText(card.name);
            if (txtLore) txtLore.SetText(card.desc);

            if (txtTypes) txtTypes.SetText(card.monsterType);
        }

        private void Select()
        {
            EventManager.EmitEventData(EventName.SelectCard, this);
            PresentHandler_SelectCard.Current.Select(Guid);
        }


        private bool _isLoadingIllus;

        private async void LoadIllus(CardConfig card)
        {
            // if(_isLoadingIllus) return;
            // _isLoadingIllus = true;

            Addressables.LoadAssetAsync<Sprite>(FakeConfig.GetIllusKey(card.code)).Completed += OnLoadIllusCompleted;


            if (background)
            {
                var cardType = Network.Query.Config.GetType_ByCode(card.code);
                var bgKey = (cardType) switch
                {
                    CardType.Monster => "card_background_normal_monster",
                    CardType.Spell => "card_background_spell",
                    _ => "card_background_normal_monster",
                };

                background.sprite = await Addressables.LoadAssetAsync<Sprite>(bgKey);
            }


            // if (illustration) illustration.sprite = await Addressables.LoadAssetAsync<Sprite>(FakeConfig.GetIllusKey(card.code));
        }

        private void OnLoadIllusCompleted(AsyncOperationHandle<Sprite> obj)
        {
            if (illustration) illustration.sprite = obj.Result;
            // _isLoadingIllus = false;
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