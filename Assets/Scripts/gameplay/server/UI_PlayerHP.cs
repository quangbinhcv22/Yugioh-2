using battle.define;
using gameplay.present;
using TMPro;
using UnityEngine;

public class UI_PlayerHP : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private TMP_Text text;

    private void OnEnable()
    {
        PresentHandler_LifePoint.Current.onChanged += UpdateImmediately;

        // EventManager.StartListening(EventName.Gameplay.StartGame, UpdateView);
        // EventManager.StartListening(EventName.Gameplay.ChangeHP, UpdateView);

        FetchDefault();
    }

    private void OnDisable()
    {
        PresentHandler_LifePoint.Current.onChanged -= UpdateImmediately;

        // EventManager.StopListening(EventName.Gameplay.StartGame, UpdateView);
        // EventManager.StopListening(EventName.Gameplay.ChangeHP, UpdateView);
    }


    private void FetchDefault()
    {
        var value = PresentHandler_LifePoint.Current.GetValue(team);
        SetText(value);
    }

    private void UpdateImmediately(Team changedTeam)
    {
        if (team != changedTeam) return;

        var value = PresentHandler_LifePoint.Current.GetValue(team);
        SetText(value);
    }


    private void SetText(int value)
    {
        text.SetText($"{value:N0}");
    }


    // private void UpdateView()
    //
    // {
    //     var data = EventManager.GetData(EventName.Gameplay.ChangeHP) as Event_ChangedHP;
    //     if (data == null || data.reason is Event_ChangedHP.Reason.Init)
    //     {
    //         ReFetch();
    //         return;
    //     }
    //
    //     if (data.reason is Event_ChangedHP.Reason.Attack)
    //     {
    //         EventManager.StartListening(EventName.Gameplay.AfterAttackUX, OnAttackUxDone);
    //     }
    // }
    //
    // private void OnAttackUxDone()
    // {
    //     EventManager.StopListening(EventName.Gameplay.AfterAttackUX, OnAttackUxDone);
    //     ReFetch();
    // }
    //
    // private void ReFetch()
    // {
    //     var currentAmount = Client_DueManager.GetPlayer(team).hp;
    //     amount.SetText($"{currentAmount}");
    // }
}