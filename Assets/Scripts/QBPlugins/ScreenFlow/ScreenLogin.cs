using Networks;
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
    }

    private void RequestLogin()
    {
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