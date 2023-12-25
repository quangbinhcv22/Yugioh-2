using System;
using System.Collections.Generic;
using System.Linq;
using AI;
using battle.define;
using Cysharp.Threading.Tasks;
using Gameplay.board;
using Gameplay.card;
using gameplay.manager;
using Gameplay.player;
using gameplay.present;
using gameplay.server;
using Networks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Network = Networks.Network;

[DefaultExecutionOrder(int.MinValue)]
public class Server_DueManager : MonoBehaviour
{
    private const int StartCardsInHand = 5;

    public static Server_DueManager main;

    private async void OnEnable()
    {
        main = this;

        await UniTask.Delay(50);

        
        if (DueManager.isReloadingBattle)
        {
            ReloadingBattle();
        }
        else
        {
            StartGame();
        }
    }

    public async void OnNewRound(int loser)
    {
        // Server_PhaseManager.main.WaitNextRound(loser);
        //
        // await UniTask.Delay(TimeSpan.FromSeconds(DueConstant.waitViewResult));
        // OnNextRound();
    }


    [FormerlySerializedAs("player1")] public Player self;
    [FormerlySerializedAs("player2")] public Player opponent;


    public int RequiredRounds = 3;
    public bool IsEndGame = false;


    [ShowInInspector, HideInEditorMode] public int CurrentRound { get; set; }


    public Player GetPlayer(string id)
    {
        return Networks.Network.Query.IsSelf(id) ? self : opponent;
    }


    public Player GetPlayer(int index)
    {
        return index == 0 ? self : opponent;
    }

    public Player GetPlayer(Team team)
    {
        return team == Team.Self ? self : opponent;
    }

    public int GetPlayerIndex(Player player)
    {
        return player == self ? 0 : 1;
    }


    public Player GetOtherPlayer(int index)
    {
        return index == 1 ? self : opponent;
    }

    public int GetOtherPlayerIndex(int index)
    {
        return index == 0 ? 1 : 0;
    }


    public async void ReloadingBattle()
    {
        while (self == opponent)
        {
            self = FakeData.RandomPlayer();
            opponent = FakeData.RandomPlayer();
        }

        DueManager.StartSync_ByReloading();

        // Server_PhaseManager.main.TurnIndex = (int)Network.Cached.matching_myTurn - 1; //Random.Range(0, 2);
        // State_Stats.StartGame();

        // OnNextRound();

        // DueNotifier.Data_UpdateRound(new Event_UpdateRound
        // {
        //     reason = Event_UpdateRound.Reason.StartGame,
        // });
        //
        await UniTask.Delay(Server_PhaseManager.main.waitToNext * 2);
        
        DueNotifier.Notify_StartGame();
    }


    [Button]
    public async void StartGame()
    {
        CurrentRound = 1;
        IsEndGame = false;

        while (self == opponent)
        {
            self = FakeData.RandomPlayer();
            opponent = FakeData.RandomPlayer();
        }


        Server_PhaseManager.main.TurnIndex = (int)Network.Cached.matching_myTurn - 1; //Random.Range(0, 2);
        State_Stats.StartGame();

        OnNextRound();


        DueNotifier.Data_UpdateRound(new Event_UpdateRound
        {
            reason = Event_UpdateRound.Reason.StartGame,
        });

        await UniTask.Delay(Server_PhaseManager.main.waitToNext * 2);
        DueNotifier.Notify_StartGame();
    }


    private void OnNextRound()
    {
        DueManager.Reset_Hp();

        self.zone.ClearAll();
        opponent.zone.ClearAll();

        self.normalSummonThisTurn = false;
        opponent.normalSummonThisTurn = false;

        main.DrawDefault(Server_PhaseManager.main.TurnIndex);
    }


    public void DiscardInHand(int playerIndex, string[] discardGuids)
    {
        var player = GetPlayer(playerIndex);

        foreach (var discardGuid in discardGuids)
        {
            throw new NotImplementedException();

            // var card = player.zone.inHand.Take(discardGuid);
            // player.zone.graveyard.Add(card);
        }

        foreach (var discardGuid in discardGuids)
        {
            DueNotifier.Notify_InHandRemove(playerIndex, discardGuid);
            DueNotifier.Notify_ToGraveyard(playerIndex, discardGuid);
        }
    }

    public void OnEndRound(int winPlayerIndex, Event_UpdateRound.Reason reason)
    {
        GetPlayer(winPlayerIndex).SetHistory(CurrentRound, RoundResult.Win);
        GetOtherPlayer(winPlayerIndex).SetHistory(CurrentRound, RoundResult.Lose);

        // end game
        if (CurrentRound == RequiredRounds)
        {
            IsEndGame = true;

            DueNotifier.Data_UpdateRound(new Event_UpdateRound
            {
                reason = reason,
            });

            DueNotifier.Data_UpdateRound(new Event_UpdateRound
            {
                reason = Event_UpdateRound.Reason.EndGame,
            });

            Server_PhaseManager.main.EndGame();

            Debug.Log("End Game");
        }
        else
        {
            CurrentRound++;


            DueNotifier.Data_UpdateRound(new Event_UpdateRound
            {
                reason = reason,
            });

            OnNewRound(GetOtherPlayerIndex(winPlayerIndex));


            // EventManager.EmitEvent(EventName.Gameplay.OutTime);
            // Debug.Log($"End Round: {reason}");
        }
    }


    public void DrawDefault(int firstPlayerIndex)
    {
        // Draw(firstPlayerIndex, StartCardsInHand);
        // Draw(GetOtherPlayerIndex(firstPlayerIndex), StartCardsInHand);

        Server_DueManager.main.self.zone.mainDeck.Init();
        Server_DueManager.main.opponent.zone.mainDeck.Init();

        Draw(0, Network.Cached.Fighting.startGame.cards);
        Draw(1, ServerCard.New5Anonymous());


        DueNotifier.Notify_DrawDefault();
    }

    [Button]
    public void Draw(int playerIndex, List<ServerCard> cards)
    {
        var player = GetPlayer(playerIndex);
        var mainDeck = player.zone.mainDeck;
        var inHand = player.zone.inHand;

        mainDeck.Take(cards.Count);

        // var drawnCards = cardCodes.Select(DueCardQuery.CreateMonster).ToList();


        foreach (var card in cards)
        {
            DueCardQuery.InitCard(card);
            inHand.Add(card);

            DueNotifier.Notify_InHandAdd(playerIndex, card.Guid);
        }


        DueNotifier.Notify_DrawCards(playerIndex, cards);
    }


    public void SummonTribute(int playerIndex, string summonGuid, string[] tributeGuids, InHand_CardUse cardUse)
    {
        var player = GetPlayer(playerIndex);

        throw new NotImplementedException();

        // var card = player.zone.inHand.Take(summonGuid);
        var mainMonsterZone = player.zone.mainMonster;

        player.normalSummonThisTurn = true;
        var summonIndex = mainMonsterZone.SearchIndex(tributeGuids.Last());


        foreach (var tributeGuid in tributeGuids)
        {
            var tributeCard = mainMonsterZone.Tribute(tributeGuid);
            player.zone.graveyard.Add(tributeCard);
        }

        throw new NotImplementedException();
        // if (cardUse is InHand_CardUse.Summon) mainMonsterZone.Summon_Attack(card, summonIndex);
        // else if (cardUse is InHand_CardUse.Set) mainMonsterZone.Summon_Defense(card, summonIndex);


        // Notify
        foreach (var tributeGuid in tributeGuids)
        {
            DueNotifier.Notify_MonsterTribute(playerIndex, tributeGuid);
            DueNotifier.Notify_ToGraveyard(playerIndex, tributeGuid);
        }


        DueNotifier.Notify_InHandRemove(playerIndex, summonGuid);

        Notifier_DueData.Current.Event_SummonMonster(new Event_SummonMonster()
        {
            playerIndex = playerIndex,
            summonGuid = summonGuid,
            position = cardUse is InHand_CardUse.Summon ? MonsterPosition.Attack : MonsterPosition.Defense,
            tributeGuids = tributeGuids,
            summonIndex = summonIndex,
        });
    }

    public void Attack(int atkPlayerIndex, string attackerGuild, string defenderGuild)
    {
        var atkPlayer = GetPlayer(atkPlayerIndex);
        var defPlayerIndex = GetOtherPlayerIndex(atkPlayerIndex);
        var defPlayer = GetPlayer(defPlayerIndex);

        var atkMonster = atkPlayer.zone.mainMonster.Get(attackerGuild);
        var directAttack = string.IsNullOrEmpty(defenderGuild);

        if (directAttack)
        {
            Network.Request.Fighting.AttackTableCardDirect(new()
            {
                sourceCardId = long.Parse(atkMonster.card.Guid),
            });

            return;

            // var damage = atkMonster.attack;
            // ModifyHp_ByAttack(defPlayerIndex, -damage);
            //
            // Notifier_DueData.Current.Event_Attack(new Event_Attack()
            // {
            //     playerAttackIndex = atkPlayerIndex,
            //     attackerGuid = attackerGuild,
            //     defenderGuid = defenderGuild,
            //     damagedPlayerIndex = defPlayerIndex,
            //     damage = damage,
            // });
        }
        else
        {
            var defMonster = defPlayer.zone.mainMonster.Get(defenderGuild);

            Network.Request.Fighting.AttackTableCard(new Request_AttackTableCard()
            {
                sourceCardId = long.Parse(atkMonster.card.Guid),
                targetCardId = long.Parse(defMonster.card.Guid),
            });

            return;
        }
    }


    [Obsolete]
    public void MonsterDie(string guild)
    {
        var location = DueCardQuery.Locate(guild);
        var playerIndex = location.playerIndex;

        var player = GetPlayer(playerIndex);
        var dieCard = player.zone.mainMonster.Die(guild);
        player.zone.graveyard.Add(dieCard);

        DueNotifier.Notify_MonsterDie(playerIndex, guild);
        DueNotifier.Notify_ToGraveyard(playerIndex, guild);
    }

    [Obsolete]
    public void FieldCardDie(string guild)
    {
        var location = DueCardQuery.Locate(guild);
        var playerIndex = location.playerIndex;

        var player = GetPlayer(playerIndex);
        var dieCard = player.zone.field.SendOldToGraveyard();
        player.zone.graveyard.Add(dieCard);

        // DueNotifier.Notify_MonsterDie(playerIndex, guild);
        DueNotifier.Notify_ToGraveyard(playerIndex, guild);
    }


    [Obsolete]
    public void ModifyHp_ByAttack(int playerIndex, int amount)
    {
        var player = GetPlayer(playerIndex);
        player.hp += amount;
        player.hp = Mathf.Max(player.hp, 0);

        // Notifier_DueData.Current.Changed_lifePoint(new Event_Changed_LifePoint()
        // {
        //     playerIndex = playerIndex,
        //     reason = Event_Changed_LifePoint.Reason.Attack,
        // });

        CheckHp(player);
    }

    private void CheckHp(Player player)
    {
        if (player.hp <= 0)
        {
            OnEndRound(GetOtherPlayerIndex(GetPlayerIndex(player)), Event_UpdateRound.Reason.OutOfLifePoint);
        }
    }

    public void ChangePosition(int playerIndex, string cardGuid)
    {
        Debug.Log("Changed position");

        var player = GetPlayer(playerIndex);
        var newPosition = player.zone.mainMonster.FindFromID(cardGuid).position.ChangeCase();


        Network.Request.Fighting.ChangeTableCardPosition(new()
        {
            cardId = long.Parse(cardGuid),
            position = newPosition.ServerKey(),
        });
    }
}