using System;
using System.Collections;
using System.Linq;
using battle.define;
using Cysharp.Threading.Tasks;
using Gameplay;
using gameplay.present;
using Sirenix.OdinInspector;
using UnityEngine;


[DefaultExecutionOrder(int.MinValue)]
public class Server_PhaseManager : MonoBehaviour
{
    public static Server_PhaseManager main;

    private void OnEnable()
    {
        main = this;
    }


    [Header("Config")] public int waitToNext = 1250; //milliseconds
    public int waitDrawPhase = 2500; //milliseconds


    public int TurnIndex { get; set; }
    public DateTime PhaseStart { get; set; }
    public DateTime PhaseEnd { get; set; }

    [ShowInInspector] private string st => PhaseStart.ToString("G");
    [ShowInInspector] private string et => PhaseEnd.ToString("G");



    public int TurnCount { get; set; }
    public Phase CurrentPhase { get; set; }

    public TurnTimer timer = new();


    public bool InTurn;

    public async void StartGame()
    {
        // TurnCount = 1;
        //
        // PhaseStart = DateTime.Now;
        // PhaseEnd = PhaseStart + TimeSpan.FromSeconds(DueConstant.secondsPerTurn);
        // InTurn = true;
        //
        // DueNotifier.Notify_ToTurn(TurnIndex);
        //
        // await UniTask.Delay(waitToNext);
        // Switch_Main1();
    }

    private void Update()
    {
        if (InTurn)
        {
            var outTime = PhaseEnd - DateTime.Now <= TimeSpan.FromSeconds(0);
            if (outTime)
            {
                Debug.Log("Out Time");
                OnOutTime();
            }
        }
    }

    private void OnOutTime()
    {
        InTurn = false;

        var winner = Server_DueManager.main.GetOtherPlayerIndex(TurnIndex);
        Server_DueManager.main.OnEndRound(winner, Event_UpdateRound.Reason.TimeUp);
    }


    public async void NextTurn()
    {
        // TurnCount++;
        //
        // TurnIndex = Server_DueManager.main.GetOtherPlayerIndex(TurnIndex);
        // Server_DueManager.main.GetPlayer(TurnIndex).normalSummonThisTurn = false;
        //
        // PhaseStart = DateTime.Now;
        // PhaseEnd = PhaseStart + TimeSpan.FromSeconds(DueConstant.secondsPerTurn);
        // InTurn = true;
        //
        // DueNotifier.Notify_ToTurn(TurnIndex);
        //
        // if(IsEndGame) return;
        // await UniTask.Delay(waitToNext);
        // if(IsEndGame) return;
        //
        // Switch_Draw();
    }

    public void SwitchPhase(Phase phase)
    {
        // switch (phase)
        // {
        //     case Phase.Draw:
        //         Switch_Draw();
        //         break;
        //     case Phase.Standby:
        //         Switch_Standby();
        //         break;
        //     case Phase.Main1:
        //         Switch_Main1();
        //         break;
        //     case Phase.Battle:
        //         Switch_Battle();
        //         break;
        //     case Phase.Main2:
        //         Switch_Main2();
        //         break;
        //     case Phase.End:
        //         Switch_End();
        //         break;
        // }
    }


    // [Button]
    // public async void Switch_Draw()
    // {
    //     CurrentPhase = Phase.Draw;
    //
    //     Server_DueManager.main.Draw(TurnIndex);
    //
    //
    //     DueNotifier.Notify_SwitchPhase(TurnIndex, Phase.Draw);
    //
    //
    //
    //     if(IsEndGame) return;
    //     await UniTask.Delay(waitDrawPhase);
    //     if(IsEndGame) return;
    //
    //     Switch_Standby();
    // }

    private IEnumerator AutoDiscardOverCard()
    {
        yield return new WaitForSeconds(DueConstant.waitDiscard);

        var player = Server_DueManager.main.GetPlayer(TurnIndex);
        var overAmount = player.zone.inHand.OverAmount;
        var discardGuid = player.zone.inHand.spaces.Take(overAmount).Select(s => s.card.Guid).ToArray();

        Server_DueManager.main.DiscardInHand(TurnIndex, discardGuid);
    }


    // [Button]
    // public async void Switch_Standby()
    // {
    //     CurrentPhase = Phase.Standby;
    //
    //     // Debug.Log("Kích hoạt các bài đặc biệt");
    //
    //     DueNotifier.Notify_SwitchPhase(TurnIndex, Phase.Standby);
    //
    //     if(IsEndGame) return;
    //     await UniTask.Delay(waitToNext);
    //     if(IsEndGame) return;
    //
    //     Switch_Main1();
    // }


    // [Button]
    // public void Switch_Main1()
    // {
    //     CurrentPhase = Phase.Main1;
    //
    //     // Debug.Log("Có thể triệu hồi thường quái vật");
    //     // Debug.Log("Có thể chuyển tư thế phòng thủ -> tấn công");
    //     // Debug.Log("Có thể kích hoạt bài phép, bẫy");
    //     // Debug.Log("Có thể chuyển sang BP hoặc EP");
    //
    //     DueNotifier.Notify_SwitchPhase(TurnIndex, Phase.Main1);
    // }

    // [Button]
    // public void Switch_Battle()
    // {
    //     CurrentPhase = Phase.Battle;
    //
    //     // Debug.Log("Có thể tấn công đối thủ");
    //     // Debug.Log("Có thể chuyển sang M2 hoặc EP");
    //
    //     DueNotifier.Notify_SwitchPhase(TurnIndex, Phase.Battle);
    // }

    // [Button]
    // public void Switch_Main2()
    // {
    //     CurrentPhase = Phase.Main2;
    //
    //     // Debug.Log("MAIN PHASE 2");
    //     // Debug.Log("Có thể triệu hồi thường quái vật nếu M1 chưa có");
    //     // Debug.Log("Có thể chuyển sang EP");
    //
    //     DueNotifier.Notify_SwitchPhase(TurnIndex, Phase.Main2);
    // }

    // [Button]
    // public async void Switch_End()
    // {
    //     CurrentPhase = Phase.End;
    //     InTurn = false;
    //
    //     // Debug.Log("END PHASE");
    //
    //     timer.Stop();
    //
    //     DueNotifier.Notify_SwitchPhase(TurnIndex, Phase.End);
    //
    //     var player = Server_DueManager.main.GetPlayer(TurnIndex);
    //
    //     if (player.zone.inHand.IsOver)
    //     {
    //         StartCoroutine(AutoDiscardOverCard());
    //         await UniTask.WaitUntil(() => !player.zone.inHand.IsOver);
    //         StopCoroutine(AutoDiscardOverCard());
    //     }
    //     
    //     if(IsEndGame) return;
    //     await UniTask.Delay(waitToNext);
    //     if(IsEndGame) return;
    //
    //     // NextTurn();
    // }

    // public async void WaitNextRound(int loser)
    // {
    //     CurrentPhase = Phase.Unset;
    //     InTurn = false;
    //
    //     DueNotifier.Notify_SwitchPhase(TurnIndex, Phase.Unset);
    //
    //
    //     if(IsEndGame) return;
    //     await UniTask.Delay(TimeSpan.FromSeconds(DueConstant.waitViewResult));
    //     if(IsEndGame) return;
    //     
    //     
    //     TurnIndex = loser;
    //     StartGame();
    // }


    public bool IsEndGame => Server_DueManager.main.IsEndGame;
    
    public void EndGame()
    {
        CurrentPhase = Phase.Unset;
        InTurn = false;
    }

}