using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Network = Networks.Network;
using Screen = QBPlugins.ScreenFlow.Screen;

public class Screen_Matching : Screen
{
    public MatchingState state;

    [Space] [SerializeField] private GameObject unsetState;
    [SerializeField] private GameObject matchingState;
    [SerializeField] private GameObject confirmState;
    [SerializeField] private GameObject selectTurnState;

    [Space] [SerializeField] private Button btnStart;

    [Space] [SerializeField] private Button btnConfirm;

    [Space] [SerializeField] private Button btnFirst;
    [SerializeField] private Button btnSecond;

    [Space] [SerializeField] private TMP_Text statusText;
    [SerializeField] private Text_Countdown countdownText;

    [Space] [SerializeField] private TMP_Text txt_selfName;
    [SerializeField] private TMP_Text txt_opnName;
    [SerializeField] private HttpImage img_selfAvt;
    [SerializeField] private HttpImage img_opnAvt;
    [SerializeField] private Sprite defaultAvt;
    [SerializeField] private GameObject ui_selfReady;
    [SerializeField] private GameObject ui_opponentReady;


    private void Awake()
    {
        btnStart.onClick.AddListener(OnClick_Start);
        btnConfirm.onClick.AddListener(OnClick_Confirm);
        btnFirst.onClick.AddListener(OnClick_First);
        btnSecond.onClick.AddListener(OnClick_Second);
    }

    private void OnClick_Start()
    {
        Network.Request.FindMatchDefault();
    }

    private void OnClick_Confirm()
    {
        Network.Request.ConfirmMatching();

        if (!Network.Cached.matching_selfConfirm)
        {
            statusText.SetText("Wait opponent to be ready...");

            btnConfirm.gameObject.SetActive(false);
        }
    }



    private void OnClick_First()
    {
        Network.Request.Matching_SelectOrderToGo(new() { order = 1 });

        btnFirst.gameObject.SetActive(false);
        btnSecond.gameObject.SetActive(false);
    }

    private void OnClick_Second()
    {
        Network.Request.Matching_SelectOrderToGo(new() { order = 2 });

        btnFirst.gameObject.SetActive(false);
        btnSecond.gameObject.SetActive(false);
    }


    private async void OnEnable()
    {
        Network.Event.onMatchingSuccess += OnMatchingSuccess;
        Network.Event.onMatchingCancel += OnMatchingCancel;

        Network.Event.onRoomInit += OnRoomInit;
        Network.Event.onMatchingConfirm += OnConfirm;

        Network.Event.matching_StartOrderToGo += OnStartOrderToGo;
        Network.Event.matching_SelectOrderToGo += OnSelectOrderToGo;

        SwitchState(MatchingState.Unset);
    }
    
    private void OnDisable()
    {
        Network.Event.onMatchingSuccess -= OnMatchingSuccess;
        Network.Event.onMatchingCancel -= OnMatchingCancel;

        Network.Event.onRoomInit -= OnRoomInit;
        Network.Event.onMatchingConfirm -= OnConfirm;

        Network.Event.matching_StartOrderToGo -= OnStartOrderToGo;
        Network.Event.matching_SelectOrderToGo -= OnSelectOrderToGo;
    }



    private void OnConfirm()
    {
        ui_selfReady.SetActive(Network.Cached.matching_selfConfirm);
        ui_opponentReady.SetActive(Network.Cached.matching_opponentConfirm);

        if (Network.Cached.matching_selfConfirm && Network.Cached.matching_opponentConfirm)
        {
            countdownText.StopCounting();
        }
    }


    private void OnUnset()
    {
        var self = Network.Cached.playerInfo;
        txt_selfName.SetText(self.displayName);
        img_selfAvt.SetData(self.avatarImage);

        txt_opnName.SetText("");
        img_opnAvt.image.sprite = defaultAvt;

        ui_selfReady.SetActive(false);
        ui_opponentReady.SetActive(false);
    }


    private async void OnStartOrderToGo()
    {
        SwitchState(MatchingState.SelectTurn);

        var myDecision = Network.Cached.matching_playerSelectTurn == Network.Cached.playerInfo.id;

        if (myDecision)
        {
            btnFirst.gameObject.SetActive(true);
            btnSecond.gameObject.SetActive(true);

            statusText.SetText("Choose your turn");
        }
        else
        {
            btnFirst.gameObject.SetActive(false);
            btnSecond.gameObject.SetActive(false);

            statusText.SetText("Opponent choosing turn...");
        }


        await UniTask.DelayFrame(3);
        countdownText.StartCounting(DateTime.Now + TimeSpan.FromMilliseconds(Network.Cached.matching_startOrderToGo.timeout));
    }

    private void OnSelectOrderToGo()
    {
        var myTurn = Network.Cached.matching_myTurn;
        statusText.SetText(myTurn == 1 ? "Your Turn: First" : "Your Turn: Second");
        
        countdownText.StopCounting();
    }


    private void OnMatchingSuccess()
    {
        SwitchState(MatchingState.Matching);

        statusText.SetText("Matching...");
    }

    private void OnMatchingCancel()
    {
        SwitchState(MatchingState.Unset);
        countdownText.StopCounting();
    }


    private void OnRoomInit()
    {
        SwitchState(MatchingState.Confirming);

        statusText.SetText("");

        var opponent = Network.Cached.matching_opponent;
        txt_opnName.SetText(opponent.displayName);
        img_opnAvt.SetData(opponent.avatarImage);

        countdownText.StartCounting(DateTime.Now +
                                    TimeSpan.FromMilliseconds(Network.Cached.matchRoomInit.stateTimeout));
    }


    private void SwitchState(MatchingState newState)
    {
        statusText.SetText("");


        state = newState;

        unsetState.SetActive(false);
        matchingState.SetActive(false);
        confirmState.SetActive(false);
        selectTurnState.SetActive(false);

        switch (state)
        {
            case MatchingState.Unset:
                unsetState.SetActive(true);
                OnUnset();
                break;
            case MatchingState.Matching:
                matchingState.SetActive(true);
                break;
            case MatchingState.Confirming:
                confirmState.SetActive(true);
                btnConfirm.gameObject.SetActive(true);
                break;
            case MatchingState.SelectTurn:
                selectTurnState.SetActive(true);
                break;
        }
    }

    public enum MatchingState
    {
        Unset,
        Matching,
        Confirming,
        SelectTurn,
    }
}