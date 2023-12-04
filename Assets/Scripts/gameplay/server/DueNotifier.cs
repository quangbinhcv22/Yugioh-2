using System.Collections.Generic;
using battle.define;
using event_name;
using Gameplay;
using Gameplay.board;
using gameplay.present;
using Networks;
using TigerForge;
using UnityEngine;

public static class DueNotifier
{
    public static void Notify_StartGame()
    {
        Debug.Log($"BẮT ĐẦU TRẬN ĐẤU");
        EventManager.EmitEvent(EventName.Gameplay.StartGame);
    }

    public static void Notify_DrawDefault()
    {
        EventManager.EmitEvent(EventName.Gameplay.DrawDefault);
    }


    public static void Data_UpdateRound(Event_UpdateRound data)
    {
        EventManager.EmitEventData(EventName.Gameplay.Data.UpdateRound, data);
    }
    
    public static void Notify_ToTurn(int playerIndex)
    {
        // Debug.Log($"Sự kiện: Đến lượt {player.name}");

        var @event = new Event_ToTurn() { playerIndex = playerIndex };
        EventManager.EmitEventData(EventName.Gameplay.ToTurn, @event);
    }

    // public static void Notify_SwitchPhase(int playerIndex, Phase phase)
    // {
    //     var player = Server_DueManager.main.GetPlayer(playerIndex);
    //     // Debug.Log($"Sự kiện: {player.name} chuyển sang giai đoạn {phase}");
    //
    //     EventManager.EmitEvent(EventName.Gameplay.ToPhase);
    // }


    public static void Notify_InHandAdd(int playerIndex, string cardGuild)
    {
        var changed = new Changed_ZoneInHand()
        {
            playerIndex = playerIndex,
            cardGuild = cardGuild,
            type = ZoneChangeType.Add,
        };

        EventManager.EmitEventData(EventName.Gameplay.ZoneChanged_InHand, changed);
    }

    public static void Notify_InHandRemove(int playerIndex, string cardGuild)
    {
        var changed = new Changed_ZoneInHand()
        {
            playerIndex = playerIndex,
            cardGuild = cardGuild,
            type = ZoneChangeType.Remove,
        };

        EventManager.EmitEventData(EventName.Gameplay.ZoneChanged_InHand, changed);
    }


    // public static void Notify_Summon(int playerIndex, string cardGuild, MonsterPosition position)
    // {
    //     var data = new Event_SummonMonster
    //     {
    //         playerIndex = playerIndex,
    //         cardGuild = cardGuild,
    //         position = position,
    //     };
    //
    //     EventManager.EmitEventData(EventName.Gameplay.Summon, data);
    // }


    public static void Notify_DrawCards(int playerIndex, List<ServerCard> cards)
    {
        var player = Server_DueManager.main.GetPlayer(playerIndex);
        // Debug.Log($"Sự kiện: {player.name} đã rút {cards.Count} thẻ:");
        //
        // foreach (var card in cards)
        // {
        //     Debug.Log($"-Thẻ {card.id}");
        // }

        EventManager.EmitEvent(EventName.Gameplay.Draw);
    }
    
    
    public static void Notify_MonsterTribute(int playerIndex, string cardGuild)
    {
        // var data = new Event_MonsterDie()
        // {
        //     playerIndex = playerIndex,
        //     cardGuild = cardGuild,
        //     reason = Event_MonsterDie.Reason.Attack
        // };
        //
        // EventManager.EmitEventData(EventName.Gameplay.MonsterDie, data);
    }

    public static void Notify_MonsterDie(int playerIndex, string cardGuild)
    {
        var data = new Event_MonsterDie()
        {
            playerIndex = playerIndex,
            cardGuild = cardGuild,
            reason = Event_MonsterDie.Reason.Attack
        };

        EventManager.EmitEventData(EventName.Gameplay.MonsterDie, data);
    }

    public static void Notify_ToGraveyard(int playerIndex, string cardGuild)
    {
        EventManager.EmitEvent(EventName.Gameplay.ToGraveyard);
    }
}

public class Event_MonsterDie
{
    public int playerIndex;
    public string cardGuild;
    public Reason reason;

    public enum Reason
    {
        Attack = 1,
    }
}