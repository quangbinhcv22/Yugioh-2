using System.Collections.Generic;
using System.Linq;
using battle.define;
using event_name;
using Gameplay.card.ui;
using Networks;
using TigerForge;
using UX;

public class Input_AttackSelector : Singleton<Input_AttackSelector>, ISourceCardStates
{
    public static string attackerGuid;
    public static string defenderGuid;

    public List<string> attackerGuids = new();
    public List<string> attackedGuids => CardAction_PhaseBattle.Current.attackedGuids;


    public bool HaveAttacker => !string.IsNullOrEmpty(attackerGuid);
    public bool IsAttacked(string guid) => attackedGuids.Contains(guid);


    public Sword swordPrefab;
    private Dictionary<string, Sword> _swords = new();


    public void StartProcess()
    {
        attackerGuid = string.Empty;
        defenderGuid = string.Empty;
        attackerGuids = Client_DueManager.GetPlayer(Team.Self).zone.mainMonster.GetAllAttacker().Select(a => a.Guid).ToList();

        Show_SwordIcons();

        PresentHandler_SelectCard.Current.onSelected += OnSelect_Card;
        
        
        Network.Event.Fighting.onPhase += OnPhase;
        

        PresentHandler_SelectCard.Current.Set_StatesSource_Task(this);
    }

    private void OnPhase(Response_OnPhase _)
    {
        StopProcess();
    }

    public void StopProcess()
    {
        Hide_SwordIcons();

        PresentHandler_SelectCard.Current.onSelected -= OnSelect_Card;
        Network.Event.Fighting.onPhase -= OnPhase;

        PresentHandler_SelectCard.Current.Set_StatesSource_Task(null);
    }


    public ButtonStates Get_CardStates(string cardGuid)
    {
        if (!Network.Query.Fighting.IsOwnTurn) return ButtonStates.Default;

        var locate = DueCardQuery.Locate(cardGuid);

        if (locate.zoneType is not BoardZoneType.MainMonster)
        {
            return ButtonStates.Default;
        }
        else
        {
            if (locate.OfTeam is Team.Self)
            {
                if (attackerGuid == cardGuid) return ButtonStates.LightUI | ButtonStates.Highlight;

                var isAttacker = attackerGuids.Contains(cardGuid);

                if (!isAttacker) return ButtonStates.Default;
                if (IsAttacked(cardGuid)) return ButtonStates.Default;

                return ButtonStates.LightUI | ButtonStates.Default;
            }
            else
            {
                if (HaveAttacker) return ButtonStates.LightUI;
                return ButtonStates.Default;
            }
        }
    }

    private void OnSelect_Card(string cardGuid)
    {
        var location = DueCardQuery.Locate(cardGuid);

        if (location.zoneType is not BoardZoneType.MainMonster) return;

        var isAttacker = attackerGuids.Contains(cardGuid);

        if (location.OfTeam is Team.Self && isAttacker && !IsAttacked(cardGuid))
        {
            SetAttacker(cardGuid);
        }
        else if (location.OfTeam is Team.Opponent && HaveAttacker)
        {
            SetDefender(cardGuid);
        }
    }

    private void SetAttacker(string guild)
    {
        attackerGuid = guild;

        foreach (var sword in _swords) sword.Value.Highlight(sword.Key == guild);


        PresentHandler_SelectCard.Current.ReCalculateState();


        var haveDefender = Client_DueManager.GetPlayer(Team.Opponent).zone.mainMonster.HaveMonster();
        if (!haveDefender)
        {
            Server_DueManager.main.Attack(0, attackerGuid, string.Empty);
            StopProcess();
        }
    }

    private void SetDefender(string guild)
    {
        defenderGuid = guild;

        PresentHandler_SelectCard.Current.ReCalculateState();


        Server_DueManager.main.Attack(0, attackerGuid, defenderGuid);
        StopProcess();
    }


    private void Show_SwordIcons()
    {
        foreach (var guid in attackerGuids)
        {
            if (IsAttacked(guid)) continue;
            
            var uiSlot = DueCardQuery.GetUICard(guid);

            var sword = Instantiate(swordPrefab, transform);
            sword.transform.position = uiSlot.transform.position;

            sword.Highlight(false);
            _swords.Add(guid, sword);
        }
    }

    private void Hide_SwordIcons()
    {
        foreach (var sword in _swords)
        {
            Destroy(sword.Value.gameObject);
        }

        _swords.Clear();
    }


    // private void FirstCheck()
    // {
    //     // var player = Client_DueManager.GetPlayer(Team.Self);
    //     // UnAttackers = player.zone.mainMonster.GetAllAttacker();
    //
    //     StartSelect();
    // }
    //
    // private void NextCheck()
    // {
    //     // if (UnAttackers.Any()) StartSelect();
    // }

    // private void StartSelect()
    // {
    //     EventManager.StartListening(EventName.SelectCard, OnSelect);
    //     EventManager.StartListening(EventName.Gameplay.ToPhase, OnSwitchPhase);
    //     // EventManager.StartListening(EventName.Gameplay.AfterAttackUX, NextCheck);
    //
    //
    //     var player = Client_DueManager.GetPlayer(Team.Self);
    //
    //     foreach (var attacker in UnAttackers)
    //     {
    //         var uiSlot = DueCardQuery.GetUICard(attacker.Guid);
    //
    //         var sword = Instantiate(swordPrefab, transform);
    //         sword.transform.position = uiSlot.transform.position;
    //
    //         sword.Highlight(false);
    //
    //         _swords.Add(attacker.Guid, sword);
    //     }
    // }


    // private void StopSelect()
    // {
    //     EventManager.StopListening(EventName.SelectCard, OnSelect);
    //     EventManager.StopListening(EventName.Gameplay.ToPhase, OnSwitchPhase);
    //     // EventManager.StopListening(EventName.Gameplay.AfterAttackUX, NextCheck);
    //
    //
    //     foreach (var sword in _swords)
    //     {
    //         Destroy(sword.Value.gameObject);
    //     }
    //
    //     AttackerGuid = string.Empty;
    //     DefenderGuid = string.Empty;
    //
    //     _swords.Clear();
    // }
}