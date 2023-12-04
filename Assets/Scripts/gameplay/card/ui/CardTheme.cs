using System;
using System.Collections.Generic;
using Gameplay.card.@enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.card.ui
{
    public class CardTheme : MonoBehaviour
    {
        [Space] [SerializeField] private Image main;
        [SerializeField] private Image title;
        [SerializeField] private Image detail;
        [SerializeField] private TMP_Text txtName;
        [SerializeField] private TMP_Text txtTypes;
        [SerializeField] private TMP_Text txtLore;

        [Space] [SerializeField] private List<CardThemeConfig> configs;

        public void Set(CardFrame frame)
        {
            return;
            
            var config = configs[(int)frame - 1];

            main.color = config.main;
            title.color = config.title;
            detail.color = config.detail;
            txtName.color = config.txtName;
            txtTypes.color = config.txtTypes;
            txtLore.color = config.txtLore;
        }
    }

    [Serializable]
    public class CardThemeConfig
    {
        public Color main = Color.white;
        public Color title = Color.white;
        public Color detail = Color.white;
        public Color txtName = Color.white;
        public Color txtTypes = Color.white;
        public Color txtLore = Color.white;
    }
}