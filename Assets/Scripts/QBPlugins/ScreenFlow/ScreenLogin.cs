using System;
using System.Collections.Generic;
using System.Linq;
using Networks;
using Newtonsoft.Json;
using QBPlugins.ScreenFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Network = Networks.Network;
using Screen = QBPlugins.ScreenFlow.Screen;

public class ScreenLogin : Screen
{
    [Space] [SerializeField] private TMP_InputField ip_userName;
    [SerializeField] private TMP_InputField ip_password;
    [SerializeField] private Button btnLogin;

    [Space] [SerializeField] private Button fastLogin;
    [SerializeField] private Button btnKillBattles;

    [Space] [SerializeField] private Button btnStart;
    [SerializeField] private Button btnLg1;
    [SerializeField] private Button btnLg2;

    private void Awake()
    {
        btnStart.onClick.AddListener(OnClicked_Start);

        btnLg1.onClick.AddListener(Login_WithUser1);
        btnLg2.onClick.AddListener(Login_WithUser2);

        btnLogin.onClick.AddListener(RequestLogin);

        fastLogin.onClick.AddListener(() =>
        {
            btnLg1.gameObject.SetActive(true);
            btnLg2.gameObject.SetActive(true);
        });
        
        btnKillBattles.onClick.AddListener(Network.Request.Fighting.Testing_EndGame);
    }


    private const string CACHED_USER_NAME_KEY = "CACHED_USER_NAME_KEY";
    private const string CACHED_USER_PASSWORD_KEY = "CACHED_USER_PASSWORD_KEY";
    
    private void OnEnable()
    {
        var cached_userName = PlayerPrefs.GetString(CACHED_USER_NAME_KEY);
        var cached_userPassword = PlayerPrefs.GetString(CACHED_USER_PASSWORD_KEY);

        ip_userName.text = cached_userName;
        ip_password.text = cached_userPassword;
    }

    private void RequestLogin()
    {
        PlayerPrefs.SetString(CACHED_USER_NAME_KEY, ip_userName.text);
        PlayerPrefs.SetString(CACHED_USER_PASSWORD_KEY, ip_password.text);
        
        Network.Request.LoginByPassword(new Request_LoginByPassword()
        {
            username = ip_userName.text,
            password = ip_password.text,
        });
    }


    private void Login_WithUser1()
    {
        Networks.Network.Request.LoginByPassword(new Request_LoginByPassword()
        {
            username = "user01",
            password = "9mHGdW54-5gaKfayjWRNvg",
        });
    }

    private void Login_WithUser2()
    {
        Network.Request.LoginByPassword(new Request_LoginByPassword()
        {
            username = "user02",
            password = "9mHGdW54-5gaKfayjWRNvg",
        });
    }

    private void OnClicked_Start()
    {
        ScreenManager.Open(ScreenKey.Loading);
    }
}