using System.Collections.Generic;
using battle.define;
using Gameplay.board;
using Gameplay.card.ui;
using gameplay.manager;
using Networks;
using Sirenix.OdinInspector;
using UnityEngine;
using UX;


public class UI_SpaceMainMonster : MonoBehaviour
{
    [SerializeField] private Team team;
    public UI_Card uiCard;
    public View_MonsterStatPosition stats;
    public int uiIndex;

    [ShowInInspector, HideInEditorMode] public MonsterPosition MonsterPosition { get; private set; }


    private static Dictionary<(Team, int), UI_SpaceMainMonster> _spaces = new();

    public static UI_SpaceMainMonster GetSpace(Team team, int dataSummonIndex)
    {
        return _spaces[(team, dataSummonIndex)];
    }

    private void Awake()
    {
        _spaces.Add((team, uiIndex), this);
    }


    private void OnEnable()
    {
        PresentHandler_Zone_MainMonster.onClear += OnClear;
        
        DueManager.onSync_TableCard += OnSyncInHand;
    }


    private void OnDisable()
    {
        PresentHandler_Zone_MainMonster.onClear -= OnClear;
        DueManager.onSync_TableCard -= OnSyncInHand;

        _spaces.Remove((team, uiIndex));

    }


    private void OnClear()
    {
        Die();
    }

    
    
    private void OnSyncInHand()
    {
        var data = DueManager.GetMonsterSpace(team, uiIndex);
        
        // Debug.Log("Sync Table");
        
        Die();
        
        if (data.card == null)
        {
            
        }
        else
        {
            // OnSummon(data.card.Guid);

            SyncCard();
            SyncPosition();
            SyncFace();
        }
    }
    
    

    public void SyncCard()
    {
        var info = DueManager.GetMonsterSpace(team, uiIndex);
        MonsterPosition = info.position;

        uiCard.gameObject.SetActive(true);
        stats.SetGuid(info.card.Guid);
        
        DueCardQuery.SetUICard_CombatSpace(info.card.Guid, this);
        
        uiCard.Binding(info.card.Guid);
    }

    public void SyncFace()
    {
        var info = DueManager.GetMonsterSpace(team, uiIndex);
        
        stats.SetGuid(info.card.Guid);

        if (info.face is "UP")
        {
            stats.Show();
            uiCard.ShowBack();
        }
        else
        {
            stats.Hide();
            uiCard.ShowHide();
        }
    }

    public void SyncPosition()
    {
        var info = DueManager.GetMonsterSpace(team, uiIndex);
        MonsterPosition = info.position;

        var flipper = uiCard.GetComponent<FlipCard>();
        if (info.position is MonsterPosition.Defense)
        {
            flipper.SetHorizontal();
        }
        else
        {
            flipper.SetVertical();
        }
    }
    
    

    public void OnSummon(string cardGuild)
    {
        var info = DueCardQuery.GetBattleInfo(cardGuild);
        _isAttacked = false;

        MonsterPosition = info.position;

        uiCard.gameObject.SetActive(true);
        stats.SetGuid(cardGuild);


        if (team is Team.Self || MonsterPosition is MonsterPosition.Attack)
        {
            stats.Show();
        }


        DueCardQuery.SetUICard_CombatSpace(cardGuild, this);

        uiCard.Binding(cardGuild);
        uiCard.Flip(MonsterPosition is MonsterPosition.Attack);
    }


    private bool _isAttacked;

    public async void ChangePosition(MonsterPosition position)
    {
        MonsterPosition = position;

        bool _wasAttacker = CardAction_PhaseMain.Current.wasAttacker.Contains(uiCard.Guid) || position is MonsterPosition.Attack;

        uiCard.Flip(MonsterPosition is MonsterPosition.Attack, _wasAttacker);
        stats.SetGuid(uiCard.Guid);
        
        
        if (_isAttacked || _wasAttacker)
        {
            stats.Show();
            uiCard.ShowBack();
        }
    }

    public void OnAttacked_OpenIfCan()
    {
        _isAttacked = true;

        if (MonsterPosition is MonsterPosition.Defense)
        {
            stats.Show();
            uiCard.ShowBack();
        }
    }

    public void Die()
    {
        uiCard.UnBinding();
        uiCard.gameObject.SetActive(false);
        stats.Hide();
    }

    
    public void OnRevealedCard(Event_RevealedCard @change)
    {
        uiCard.ShowBack();
        uiCard.OnRevealed(change.cardCode);
        stats.Show();
    }
    
    public void OnTribute()
    {
        uiCard.UnBinding();
        uiCard.gameObject.SetActive(false);
        stats.Hide();
    }
}


public enum ZoneChangeType
{
    Add = 1,
    Remove = 2,
}