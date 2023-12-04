using System.Linq;
using event_name;
using Gameplay.card.ui;
using TigerForge;

public class CardAction_SummonTribute : Singleton<CardAction_SummonTribute>, ISourceCardStates
{
    public bool InProcess;

    public string summonGuid;
    public InHand_CardUse cardUse;
    public string[] tributeGuids;


    private Panel_ActionCard_FullScreen Panel => Panel_ActionCard_FullScreen.Current;


    public void StartProcess(string _summonGuid, InHand_CardUse _cardUse)
    {
        InProcess = true;

        summonGuid = _summonGuid;
        cardUse = _cardUse;

        var requiredAmount = DueCardQuery.GetTributeRequire(summonGuid);
        if (requiredAmount is 0) return;

        tributeGuids = new string[requiredAmount];

        Panel.Show();
        Panel.onYes += OnYes;
        Panel.onNo += OnNo;

        PresentHandler_SelectCard.Current.onSelected += OnSelected;
        PresentHandler_SelectCard.Current.Set_StatesSource_Task(this);

        RefreshPanel();
    }


    private void OnSelected(string cardGuid)
    {
        var locate = DueCardQuery.Locate(cardGuid);
        if (locate.zoneType is not BoardZoneType.MainMonster) return;

        const int breakIndex = int.MaxValue - 1;

        if (tributeGuids.Any(g => g == cardGuid))
        {
            // un select
            for (int i = 0; i < tributeGuids.Length; i++)
            {
                if (tributeGuids[i] == cardGuid)
                {
                    tributeGuids[i] = string.Empty;
                    i = breakIndex;
                }
            }
        }
        else
        {
            for (int i = 0; i < tributeGuids.Length; i++)
            {
                // select
                if (string.IsNullOrEmpty(tributeGuids[i]))
                {
                    tributeGuids[i] = cardGuid;
                    i = breakIndex;
                }
            }
        }


        PresentHandler_SelectCard.Current.ReCalculateState();

        RefreshPanel();
    }


    public ButtonStates Get_CardStates(string cardGuid)
    {
        if (tributeGuids.Any(selectedGuid => selectedGuid == cardGuid))
        {
            return ButtonStates.LightUI | ButtonStates.Default | ButtonStates.Highlight;
        }

        var locate = DueCardQuery.Locate(cardGuid);
        var isMine = locate.playerIndex == Client_DueManager.myIndex;

        if (!SelectedEnough && isMine && locate.zoneType is BoardZoneType.MainMonster)
        {
            return ButtonStates.LightUI | ButtonStates.Default;
        }

        return ButtonStates.Default;
    }

    public void StopProcess()
    {
        InProcess = false;

        PresentHandler_SelectCard.Current.onSelected -= OnSelected;
        PresentHandler_SelectCard.Current.Set_StatesSource_Task(null);
    }


    private void OnYes()
    {
        Server_DueManager.main.SummonTribute(Client_DueManager.myIndex, summonGuid, tributeGuids, cardUse);
        StopProcess();
    }

    private void OnNo()
    {
        StopProcess();
    }


    private void OnSelect()
    {
        RefreshPanel();
    }

    private void RefreshPanel()
    {
        var panel = Panel_ActionCard_FullScreen.Current;

        panel.SetTitle(GetTitle());
        panel.UseButton_Yes("Confirm").SetInteractable_Yes(SelectedEnough);
        panel.UseButton_No("Cancel").SetInteractable_No(true);
    }


    private string GetTitle()
    {
        return $"Choose {Selected}/{Required} tribute monster(s) on the field";
    }

    private int Selected => tributeGuids.Count(g => !string.IsNullOrEmpty(g));
    private int Required => tributeGuids.Length;
    private bool SelectedEnough => Selected == Required;
}