using System;
using System.Linq;
using battle.define;
using event_name;
using Gameplay.card.ui;
using Networks;
using TigerForge;

public class CardAction_DiscardCard : Singleton<CardAction_DiscardCard>, ISourceCardStates
{
    public int time = 15;

    public bool InProcess { get; set; }
    public string[] discardGuids;

    private string GetTitle()
    {
        return $"Discard {Selected}/{Required} cards from your hand";
    }

    private Panel_ActionCard_FullScreen Panel => Panel_ActionCard_FullScreen.Current;
    private int Selected => discardGuids.Count(g => !string.IsNullOrEmpty(g));
    private int Required => discardGuids.Length;
    private bool SelectedEnough => Selected == Required;


    public ButtonStates Get_CardStates(string cardGuid)
    {
        if (discardGuids.Any(selectedGuid => selectedGuid == cardGuid))
        {
            return ButtonStates.LightUI | ButtonStates.Default | ButtonStates.Highlight;
        }

        var locate = DueCardQuery.Locate(cardGuid);
        var isMine = locate.playerIndex == Client_DueManager.myIndex;

        if (!SelectedEnough && isMine && locate.zoneType is BoardZoneType.InHand)
        {
            return ButtonStates.LightUI | ButtonStates.Default;
        }

        return ButtonStates.Default;
    }


    public void StartProcess(int discardAmount)
    {
        InProcess = true;

        discardGuids = new string[discardAmount];

        Panel.Show();
        Panel.UseButton_Yes("Confirm");
        Panel.onYes += OnYes;
        
        Networks.Network.Event.Fighting.onPhase += OnPhase;

        PresentHandler_SelectCard.Current.onSelected += On_SelectCard;
        PresentHandler_SelectCard.Current.Set_StatesSource_Task(this);
        
        Panel.UseCountdown(DateTime.Now + TimeSpan.FromSeconds(DueConstant.waitDiscard));

        RefreshPanel();
    }

    private void OnPhase(Response_OnPhase obj)
    {
        StopProcess();
    }

    public void StopProcess()
    {
        InProcess = false;
        
        Panel.Hide();

        Networks.Network.Event.Fighting.onPhase -= OnPhase;
        
        PresentHandler_SelectCard.Current.onSelected -= On_SelectCard;
        PresentHandler_SelectCard.Current.Set_StatesSource_Task(null);
    }


    private void On_SelectCard(string cardGuid)
    {
        const int breakIndex = int.MaxValue - 1;

        var location = DueCardQuery.Locate(cardGuid);
        if(location.OfTeam != Team.Self || location.zoneType != BoardZoneType.InHand) return;

        if (discardGuids.Any(g => g == cardGuid))
        {
            // un select
            for (int i = 0; i < discardGuids.Length; i++)
            {
                if (discardGuids[i] == cardGuid)
                {
                    discardGuids[i] = string.Empty;
                    i = breakIndex;
                }
            }
        }
        else
        {
            for (int i = 0; i < discardGuids.Length; i++)
            {
                // select
                if (string.IsNullOrEmpty(discardGuids[i]))
                {
                    discardGuids[i] = cardGuid;
                    i = breakIndex;
                }
            }
        }
        
        PresentHandler_SelectCard.Current.ReCalculateState();
        RefreshPanel();
    }

    private void RefreshPanel()
    {
        Panel.SetTitle(GetTitle());
        Panel.UseButton_Yes("Confirm").SetInteractable_Yes(SelectedEnough);
    }


    private void OnYes()
    {
        Server_DueManager.main.DiscardInHand(Client_DueManager.myIndex, discardGuids);
        StopProcess();
    }
}