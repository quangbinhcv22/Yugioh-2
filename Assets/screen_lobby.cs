using Networks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Network = Networks.Network;

public class screen_lobby : Singleton<screen_lobby>
{
    public static RoundResult beforeResult;

    public TMP_Text beforeResultTxt;
    public Button startButton;
    public GameObject screenBattle;
    public Transform canvas;

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        beforeResultTxt.SetText(beforeResult switch
        {
            RoundResult.Win => "You Win",
            RoundResult.Lose => "You Lose",
            _ => "",
        });
    }

    private void StartGame()
    {
        // if (Screen_Battle.Current)
        // {
        //     Screen_Battle.Current.gameObject.SetActive(true);
        // }
        // else
        // {
        //     Instantiate(screenBattle, canvas);
        // }
        
        gameObject.SetActive(false);
    }
}