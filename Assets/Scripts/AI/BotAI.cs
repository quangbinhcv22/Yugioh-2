using System.Linq;
using battle.define;
using Cysharp.Threading.Tasks;
using event_name;
using Gameplay;
using TigerForge;
using UnityEngine;

public class BotAI : MonoBehaviour
{
    private void OnEnable()
    {
        // EventManager.StartListening(EventName.Gameplay.ToPhase, OnPhase);
    }

    private void OnDisable()
    {
        // EventManager.StopListening(EventName.Gameplay.ToPhase, OnPhase);
    }

    // private void OnPhase()
    // {
    //     if (Networks.Network.Query.Fighting.IsOwnTurn) return;
    //
    //     var currentPhase = Networks.Network.Query.Fighting.CurrentPhase;
    //     
    //     if (currentPhase is Phase.Draw) ActionOn_PhaseDraw();
    //     if (currentPhase is Phase.Main1) ActionOn_PhaseMain1();
    //     if (currentPhase is Phase.Battle) ActionOn_PhaseBattle();
    // }
    //
    // private async void ActionOn_PhaseDraw()
    // {
    //     await UniTask.Delay(500);
    //
    //     var player = Client_DueManager.GetPlayer(Team.Opponent);
    //     if (player.zone.inHand.IsOver)
    //     {
    //         var overAmount = player.zone.inHand.OverAmount;
    //         var discardGuid = player.zone.inHand.spaces.Take(overAmount).Select(s => s.card.Guid).ToArray();
    //         
    //         Server_DueManager.main.DiscardInHand(1, discardGuid);
    //     }
    // }
    //
    //
    // private async void ActionOn_PhaseMain1()
    // {
    //     await UniTask.Delay(500);
    //
    //     var player = Client_DueManager.GetPlayer(Team.Opponent);
    //
    //     if (!player.zone.mainMonster.IsFull())
    //     {
    //         var firstCardInHand = player.zone.inHand.spaces.First().card;
    //
    //         var monsterPosition = Random.Range(0, 2) == 0 ? InHand_CardUse.Summon : InHand_CardUse.Set;
    //         Server_DueManager.main.InHand_Use(1, firstCardInHand.Guid, monsterPosition);
    //     }
    //
    //
    //     await UniTask.Delay(1000);
    //
    //     if (!Server_PhaseManager.main.IsFirstTurn && player.zone.mainMonster.HaveAttacker())
    //     {
    //         Server_PhaseManager.main.Switch_Battle();
    //
    //     }
    //     else
    //     {
    //         Server_PhaseManager.main.Switch_End();
    //     }
    // }
    //
    // private async void ActionOn_PhaseBattle()
    // {
    //     await UniTask.Delay(500);
    //
    //     var people = Client_DueManager.GetPlayer(Team.Self);
    //     var bot = Client_DueManager.GetPlayer(Team.Opponent);
    //
    //     if (bot.zone.mainMonster.HaveAttacker())
    //     {
    //         var attacker = bot.zone.mainMonster.GetRandomAttacker();
    //     
    //         if (people.zone.mainMonster.HaveMonster())
    //         {
    //             var defender = people.zone.mainMonster.GetRandomMonster();
    //             Server_DueManager.main.Attack(1, attacker.card.Guid, defender.card.Guid);
    //         }
    //         else
    //         {
    //             Server_DueManager.main.Attack(1, attacker.card.Guid, string.Empty);
    //         }
    //     }
    //
    //     await UniTask.Delay(1500);
    //     
    //     if(Networks.Network.Query.Fighting.CurrentPhase is Phase.Unset) return;
    //     Server_PhaseManager.main.Switch_End();
    // }
}