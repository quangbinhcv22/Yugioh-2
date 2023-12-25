using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Network = Networks.Network;

namespace fake
{
    public class ToolPanel_PresetCard : MonoBehaviour
    {
        public const string DEBUG_PRESET_CARD_IN_HAND_KEY = "DEBUG_PRESET_CARD_IN_HAND_KEY";
        public static ToolPanel_PresetCard singleton;


        public List<string> codes;

        [Space] [SerializeField] private ToolPanel_SelectingPresetCard selectingPanel;

        [Space] [SerializeField] private TMP_InputField allCodeInput;
        [SerializeField] TMP_Text errorText;

        [Space] [SerializeField] private TMP_InputField codeInput;
        [SerializeField] private Button btnAddCode;

        [Space] [SerializeField] private Button btnRemove;
        [SerializeField] private Button btnSave;

        private void Awake()
        {
            btnAddCode.onClick.AddListener(AddFromInputCode);

            btnRemove.onClick.AddListener(RemoveAll);
            btnSave.onClick.AddListener(Save);

            allCodeInput.onValueChanged.AddListener(OnDrawCodesChange);
        }

        public static List<string> GetSavedPreset()
        {
            if (PlayerPrefs.HasKey(DEBUG_PRESET_CARD_IN_HAND_KEY))
            {
                var savedJson = PlayerPrefs.GetString(DEBUG_PRESET_CARD_IN_HAND_KEY);
                return JsonConvert.DeserializeObject<List<string>>(savedJson);
            }

            return new();
        }

        private void OnDrawCodesChange(string arg0)
        {
            if (allCodeInput.text == GetDrawCodes_FromCached()) return;

            var inputCodes = GetList_FromDrawInput();

            var invalidCode = new List<string>();
            foreach (var inputCode in inputCodes)
            {
                if (!Network.Query.Config.IsValidCode(inputCode))
                {
                    invalidCode.Add(inputCode);
                }
            }

            if (invalidCode.Any())
            {
                var inValidText = invalidCode.Aggregate((a, x) => $"'{a}'" + ", " + $"'{x}'");
                errorText.SetText($"Code khong hop le: {inValidText}");
            }
            else
            {
                errorText.SetText($"");

                codes = inputCodes;
                ReloadView();
            }
        }

        private void OnEnable()
        {
            singleton = this;

            codes = GetSavedPreset();


            ReloadView();
        }


        private string GetDrawCodes_FromCached()
        {
            return codes.Any() ? codes.Aggregate((a, x) => a + ", " + x) : string.Empty;
        }

        private List<string> GetList_FromDrawInput()
        {
            return allCodeInput.text.Replace(" ", string.Empty).Split(",").ToList();
        }


        private void AddFromInputCode()
        {
            codes.Add(codeInput.text);
            ReloadView();
        }

        public void AddFromPreset(string code)
        {
            codes.Add(code);
            ReloadView();
        }


        private void RemoveAll()
        {
            codes.Clear();
            ReloadView();
        }

        private void Save()
        {
            PlayerPrefs.SetString(DEBUG_PRESET_CARD_IN_HAND_KEY, JsonConvert.SerializeObject(codes));

            gameObject.SetActive(false);
        }


        private void ReloadView()
        {
            Debug.Log("Reload view");
            
            allCodeInput.text = GetDrawCodes_FromCached();
            selectingPanel.Reload(codes);
        }
    }
}