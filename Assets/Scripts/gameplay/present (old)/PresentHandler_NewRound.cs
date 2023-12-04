using System;
using Cysharp.Threading.Tasks;
using event_name;
using QBPlugins.ScreenFlow;
using TigerForge;
using UnityEngine;
using UX;

namespace gameplay.present
{
    [DefaultExecutionOrder(int.MinValue)]
    public class PresentHandler_NewRound : Singleton<PresentHandler_NewRound>
    {
        public static Action onResetRound;


        protected override void OnEnable()
        {
            EventManager.StartListening(EventName.Gameplay.Data.UpdateRound, On_UpdateRound);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventName.Gameplay.Data.UpdateRound, On_UpdateRound);
        }

        private void On_UpdateRound()
        {
            var data = (Event_UpdateRound)EventManager.GetData(EventName.Gameplay.Data.UpdateRound);

            switch (data.reason)
            {
                case Event_UpdateRound.Reason.StartGame:
                {
                    UpdateImmediately();

                    NotifyText.SetColor(Color.white);
                    NotifyText.Notify("Round 1");
                }
                    break;
                case Event_UpdateRound.Reason.EndGame:
                    OnEndGame();
                    break;
                case Event_UpdateRound.Reason.TimeUp:
                    Update_EndLifePoint();
                    break;
                case Event_UpdateRound.Reason.OutOfLifePoint:
                    WaitUpdate_EndLifePoint();
                    break;
                case Event_UpdateRound.Reason.OutOfCard:
                    break;
            }
        }

        private async void OnEndGame()
        {
            screen_lobby.beforeResult = Client_DueManager.MyResult();

            await UniTask.Delay(TimeSpan.FromSeconds(DueConstant.waitViewResult));


            ScreenManager.ReleaseAll_ToMain();
            
            // screen_lobby.Current.gameObject.SetActive(true);
            // Screen_Battle.Current.gameObject.SetActive(false);
        }

        private void EmitPresent()
        {
            EventManager.EmitEvent(EventName.Gameplay.Present.UpdateRound);
        }


        private void UpdateImmediately()
        {
            EmitPresent();
        }


        private void WaitUpdate_EndLifePoint()
        {
            PresentHandler_Attack.Current.presentCompleted += Update_EndLifePoint;
        }

        private void Update_EndLifePoint()
        {
            PresentHandler_Attack.Current.presentCompleted -= Update_EndLifePoint;
            EmitPresent();
            
            onResetRound?.Invoke();
            ShowPanel();
        }
        
        private void ShowPanel()
        {
            Panel_ResultOfMatches.Current.Show();
        }
        
    }

    public class Event_UpdateRound
    {
        public Reason reason;


        public enum Reason
        {
            StartGame, //1
            EndGame, //2

            Surrender,
            TimeUp, //3

            OutOfLifePoint, //4
            OutOfCard,
        }
    }
}