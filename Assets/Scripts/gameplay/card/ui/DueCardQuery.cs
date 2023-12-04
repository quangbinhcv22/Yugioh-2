using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.card.ui;
using Networks;
using Network = Networks.Network;

public static class DueCardQuery
{
    private static readonly List<ServerCard> Cards = new();


    public static ServerCard InitCard(ServerCard card)
    {
        Cards.Add(card);
        return card;
    }
    
    public static ServerCard GetData(string cardGuild)
    {
        return Cards.FirstOrDefault(card => card.Guid == cardGuild);
    }
    

    public static CardLocation Locate(string cardGuid)
    {
        var manager = Server_DueManager.main;

        var locate1 = manager.player1.zone.Locate(cardGuid);
        if (locate1.IsValid)
        {
            locate1.playerIndex = 0;
            return locate1;
        }

        var locate2 = manager.player2.zone.Locate(cardGuid);
        locate2.playerIndex = 1;

        return locate2;
    }

    public static CardSpace_Combat GetBattleInfo(string cardGuild)
    {
        var manager = Server_DueManager.main;
        return manager.player1.zone.mainMonster.Get(cardGuild) ?? manager.player2.zone.mainMonster.Get(cardGuild);
    }


    private static readonly Dictionary<string, UI_Card> UICards = new();

    public static void SetUICard(string cardGuild, UI_Card card)
    {
        if (UICards.ContainsKey(cardGuild)) UICards[cardGuild] = card;
        else UICards.Add(cardGuild, card);
    }

    public static List<UI_Card> GetAll_UICards()
    {
        return UICards.Select(c => c.Value).ToList();
    }

    public static UI_Card GetUICard(string cardGuild)
    {
        if (!UICards.ContainsKey(cardGuild)) return null;
        return UICards[cardGuild];
    }

    private static readonly Dictionary<string, UI_SpaceMainMonster> UICardSpaces = new();


    public static void SetUICard_CombatSpace(string cardGuild, UI_SpaceMainMonster space)
    {
        if (UICardSpaces.ContainsKey(cardGuild)) UICardSpaces[cardGuild] = space;
        else UICardSpaces.Add(cardGuild, space);
    }

    public static UI_SpaceMainMonster GetUICard_CombatSpace(string cardGuild)
    {
        return UICardSpaces[cardGuild];
    }

    public static bool IsAnonymousCard(string cardGuild)
    {
        var result =long .Parse(cardGuild) <= 0;
        return result;
    }


    public static CardConfig GetViewInfo(string cardGuild)
    {
        var code = GetData(cardGuild).code;
        return Network.Query.Config.GetCard(code);
    }

    public static int GetTributeRequire(string guild)
    {
        var define = GetViewInfo(guild);
        var level = define.level;

        return level switch
        {
            >= 7 => 2,
            >= 5 => 1,
            _ => 0
        };
    }
}