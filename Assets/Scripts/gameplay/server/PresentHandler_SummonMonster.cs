using System;
using System.Linq;
using battle.define;
using Cysharp.Threading.Tasks;
using event_name;
using Gameplay.board;
// using Sirenix.Utilities;
using TigerForge;
using UnityEngine;

[DefaultExecutionOrder(int.MinValue + 1)]
public class PresentHandler_SummonMonster : Singleton<PresentHandler_SummonMonster>
{
    [SerializeField] private float delayTribute = 0.35f;

    public Action OnSummon;

    protected override void OnEnable()
    {
        base.OnEnable();
        Notifier_DueData.Current.event_summonMonster += HandleData;
    }

    private void OnDisable()
    {
        Notifier_DueData.Current.event_summonMonster -= HandleData;
    }

    private async void HandleData(Event_SummonMonster data)
    {
        if ((Team)data.playerIndex is Team.Self)
        {
            CardAction_PhaseMain.Current.summonThisTurn_Cards.Add(data.summonGuid);
        }

        if (data.position is MonsterPosition.Attack)
        {
            CardAction_PhaseMain.Current.wasAttacker.Add(data.summonGuid);
        }

        var isTribute = data.tributeGuids != null && data.tributeGuids.Any();
        if (isTribute)
        {
            data.tributeGuids.ToList().ForEach(PresentTribute);
            await UniTask.Delay(TimeSpan.FromSeconds(delayTribute));
        }

        var team = Client_DueManager.GetTeam(data.playerIndex);
        PresentSummon(team, data.summonGuid, data.summonIndex);
    }

    private void PresentTribute(string guid)
    {
        var space = DueCardQuery.GetUICard_CombatSpace(guid);
        space.OnTribute();
    }

    private void PresentSummon(Team team, string guid, int spaceIndex)
    {
        var summonSpace = UI_SpaceMainMonster.GetSpace(team, spaceIndex);
        summonSpace.OnSummon(guid);

        EventManager.EmitEvent(EventName.Gameplay.UI_RefreshPhase);
        OnSummon?.Invoke();
    }
}

public sealed class Event_SummonMonster
{
    public int playerIndex;
    public string summonGuid;
    public int summonIndex;
    public string[] tributeGuids;
    public MonsterPosition position;
}