using System;
using System.Collections.Generic;
using System.Linq;
using AI;
using battle.define;
using Cysharp.Threading.Tasks;
using Gameplay;
using Gameplay.board;
using Gameplay.player;
using gameplay.present;
using Networks;
using Sirenix.OdinInspector;
using UnityEngine;
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
        StartGame();
    }

    public async void OnNewRound(int loser)
    {
        // Server_PhaseManager.main.WaitNextRound(loser);
        //
        // await UniTask.Delay(TimeSpan.FromSeconds(DueConstant.waitViewResult));
        // OnNextRound();
    }


    public Player player1;
    public Player player2;


    public int RequiredRounds = 3;
    public bool IsEndGame = false;


    [ShowInInspector, HideInEditorMode] public int CurrentRound { get; set; }


    public Player GetPlayer(int index)
    {
        return index == 0 ? player1 : player2;
    }

    public Player GetPlayer(Team team)
    {
        return team == Team.Self ? player1 : player2;
    }

    public int GetPlayerIndex(Player player)
    {
        return player == player1 ? 0 : 1;
    }


    public Player GetOtherPlayer(int index)
    {
        return index == 1 ? player1 : player2;
    }

    public int GetOtherPlayerIndex(int index)
    {
        return index == 0 ? 1 : 0;
    }


    [Button]
    public async void StartGame()
    {
        CurrentRound = 1;
        IsEndGame = false;

        while (player1 == player2)
        {
            player1 = FakeData.RandomPlayer();
            player2 = FakeData.RandomPlayer();
        }


        // FakeConfig.NewMonster();

        Server_PhaseManager.main.TurnIndex = (int)Network.Cached.matching_myTurn - 1; //Random.Range(0, 2);
        OnNextRound();


        DueNotifier.Data_UpdateRound(new Event_UpdateRound
        {
            reason = Event_UpdateRound.Reason.StartGame,
        });

        await UniTask.Delay(Server_PhaseManager.main.waitToNext * 2);
        Server_PhaseManager.main.StartGame();

        DueNotifier.Notify_StartGame();
    }


    private void OnNextRound()
    {
        player1.hp = DueConstant.hp;
        player2.hp = DueConstant.hp;

        player1.zone.ClearAll();
        player2.zone.ClearAll();

        player1.normalSummonThisTurn = false;
        player2.normalSummonThisTurn = false;


        // player1.zone.mainDeck.Import(Network.Cached.Fighting.startGame.cardCodes);
        // player2.zone.mainDeck.Import(Network.Cached.Fighting.startGame.cardCodes);
        //

        main.DrawDefault(Server_PhaseManager.main.TurnIndex);


        Notifier_DueData.Current.Changed_lifePoint(new Event_Changed_LifePoint()
        {
            playerIndex = 0,
            reason = Event_Changed_LifePoint.Reason.StartGame,
        });

        Notifier_DueData.Current.Changed_lifePoint(new Event_Changed_LifePoint()
        {
            playerIndex = 1,
            reason = Event_Changed_LifePoint.Reason.StartGame,
        });
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

        Server_DueManager.main.player1.zone.mainDeck.Init();
        Server_DueManager.main.player2.zone.mainDeck.Init();
        
        Draw(0, Network.Cached.Fighting.startGame.cards);
        Draw(1,  ServerCard.New5Anonymous());


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


    public void InHand_Use(int playerIndex, string cardGuid, InHand_CardUse cardUse)
    {
        var player = GetPlayer(playerIndex);

        throw new NotImplementedException();
        // var card = player.zone.inHand.Take(cardGuid);
        var cardType = FakeConfig.GetType_ByGuid(cardGuid);

        // kiểm tra đã triệu hồi thường lượt này chưa
        // kiểm tra tính hợp lý triệu hồi

        if (cardType is CardType.Monster)
        {
            // hiến tế nếu có

            if (cardUse is InHand_CardUse.Summon)
            {
                return;

                throw new NotImplementedException();
                // player.normalSummonThisTurn = true; var index = player.zone.mainMonster.Summon_Attack(card);

                // DueNotifier.Notify_InHandRemove(playerIndex, cardGuid);
                //
                // Notifier_DueData.Current.Event_SummonMonster(new()
                // {
                //     playerIndex = playerIndex,
                //     summonGuid = cardGuid,
                //     position = MonsterPosition.Attack,
                //     summonIndex = index,
                // });
            }
            else if (cardUse is InHand_CardUse.Set)
            {
                player.normalSummonThisTurn = true;

                throw new NotImplementedException();
                // var index = player.zone.mainMonster.Summon_Defense(card);

                // DueNotifier.Notify_InHandRemove(playerIndex, cardGuid);
                // Notifier_DueData.Current.Event_SummonMonster(new()
                // {
                //     playerIndex = playerIndex,
                //     summonGuid = cardGuid,
                //     position = MonsterPosition.Defense,
                //     summonIndex = index,
                // });
            }
        }
        else if (cardType is CardType.Spell) //or CardType.Trap)
        {
            throw new NotImplementedException();

            // var index = player.zone.spellTrap.Set(card);
            // var team = playerIndex == 0 ? Team.Self : Team.Opponent;
            //
            // DueNotifier.Notify_InHandRemove(playerIndex, cardGuid);
            //
            // Notifier_DueData.Current.Event_SetSpell(new()
            // {
            //     team = playerIndex == 0 ? Team.Self : Team.Opponent,
            //     guid = cardGuid,
            //     index = index,
            // });
            //
            // var spell = SpellCard_Query.InstantiateSpell(team, cardGuid);
            // spell.OnActive();
        }
        else if (cardType is CardType.Field)
        {
            // xử lý sau
        }

        // Thông báo
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
            Network.Request.Fighting.AttackTableCardDirect(new ()
            {
                sourceCardId = long.Parse(atkMonster.card.Guid),
            });

            return;

            var damage = atkMonster.attack;
            ModifyHp_ByAttack(defPlayerIndex, -damage);

            Notifier_DueData.Current.Event_Attack(new Event_Attack()
            {
                playerAttackIndex = atkPlayerIndex,
                attackerGuid = attackerGuild,
                defenderGuid = defenderGuild,
                damagedPlayerIndex = defPlayerIndex,
                damage = damage,
            });
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

            // quái bị tấn công trong trạng thái công
            if (defMonster.position is MonsterPosition.Attack)
            {
                var difference = atkMonster.attack - defMonster.attack;

                // cả 2 quái đều chết, không ai bị thiệt hại
                if (difference == 0)
                {
                    MonsterDie(atkPlayerIndex, attackerGuild);
                    MonsterDie(defPlayerIndex, defenderGuild);

                    Notifier_DueData.Current.Event_Attack(new Event_Attack()
                    {
                        playerAttackIndex = atkPlayerIndex,
                        attackerGuid = attackerGuild,
                        defenderGuid = defenderGuild,
                        damagedPlayerIndex = defPlayerIndex,
                        damage = difference,
                        dieCards = new() { attackerGuild, defenderGuild }
                    });
                }
                // quái tấn công sát thương lớn hơn, quái thủ chết, người thủ bị thiệt hại
                else if (difference > 0)
                {
                    MonsterDie(defPlayerIndex, defenderGuild);
                    ModifyHp_ByAttack(defPlayerIndex, -Mathf.Abs(difference));


                    Notifier_DueData.Current.Event_Attack(new Event_Attack()
                    {
                        playerAttackIndex = atkPlayerIndex,
                        attackerGuid = attackerGuild,
                        defenderGuid = defenderGuild,
                        damagedPlayerIndex = defPlayerIndex,
                        damage = difference,
                        dieCards = new() { defenderGuild }
                    });
                }
                // quái tấn công sát thương nhỏ hơn, quái công chết, người công bị thiệt hại
                else
                {
                    MonsterDie(atkPlayerIndex, attackerGuild);
                    ModifyHp_ByAttack(atkPlayerIndex, -Mathf.Abs(difference));


                    Notifier_DueData.Current.Event_Attack(new Event_Attack()
                    {
                        playerAttackIndex = atkPlayerIndex,
                        attackerGuid = attackerGuild,
                        defenderGuid = defenderGuild,
                        damagedPlayerIndex = atkPlayerIndex,
                        damage = difference,
                        dieCards = new() { attackerGuild }
                    });
                }
            }
            // quái bị tấn công trong trạng thái thủ
            else
            {
                //lật bài nếu có
                var difference = atkMonster.attack - defMonster.defense;

                // đánh làm màu
                if (difference == 0)
                {
                    Notifier_DueData.Current.Event_Attack(new Event_Attack()
                    {
                        playerAttackIndex = atkPlayerIndex,
                        attackerGuid = attackerGuild,
                        defenderGuid = defenderGuild,
                        damagedPlayerIndex = defPlayerIndex,
                        damage = 0,
                    });
                }
                // quái thủ chết
                else if (difference > 0)
                {
                    MonsterDie(defPlayerIndex, defenderGuild);

                    Notifier_DueData.Current.Event_Attack(new Event_Attack()
                    {
                        playerAttackIndex = atkPlayerIndex,
                        attackerGuid = attackerGuild,
                        defenderGuid = defenderGuild,
                        damagedPlayerIndex = defPlayerIndex,
                        damage = 0,
                        dieCards = new() { defenderGuild }
                    });
                }
                // người công chịu thiệt hại
                else
                {
                    ModifyHp_ByAttack(atkPlayerIndex, -Mathf.Abs(difference));

                    Notifier_DueData.Current.Event_Attack(new Event_Attack()
                    {
                        playerAttackIndex = atkPlayerIndex,
                        attackerGuid = attackerGuild,
                        defenderGuid = defenderGuild,
                        damagedPlayerIndex = atkPlayerIndex,
                        damage = difference,
                    });
                }
            }
        }
    }


    public void DealDamage(Team team, int damage, string sourceGuid)
    {
        var player = GetPlayer(team);
        player.hp -= damage;
        player.hp = Mathf.Max(player.hp, 0);

        CheckHp(player);
    }


    [Obsolete]
    public void MonsterDie(int playerIndex, string guild)
    {
        var player = GetPlayer(playerIndex);
        var dieCard = player.zone.mainMonster.Die(guild);
        player.zone.graveyard.Add(dieCard);

        DueNotifier.Notify_MonsterDie(playerIndex, guild);
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

    public void SendToGraveyard(string guid)
    {
        // var location = DueCardQuery.Locate(guid);
        //
        // var player = GetPlayer(location.OfTeam);
        //
        // var dieCard = player.zone.spellTrap.Take(guild);
        // player.zone.graveyard.Add(dieCard);
        //
        // DueNotifier.Notify_MonsterDie(playerIndex, guild);
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
        var newPosition = player.zone.mainMonster.ChangePosition(cardGuid);

        Notifier_DueData.Current.Event_ChangePosition(new()
        {
            playerIndex = playerIndex,
            guid = cardGuid,
            position = newPosition,
        });
    }
}