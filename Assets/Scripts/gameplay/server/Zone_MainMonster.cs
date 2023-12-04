using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Gameplay.board;
using Gameplay.card.ui;
using Networks;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

[Serializable]
public class Zone_MainMonster
{
    private const int Capacity = 5;
    [ShowInInspector] public readonly List<CardSpace_Combat> spaces;

    public Zone_MainMonster()
    {
        spaces = new(Capacity);
        for (int i = 0; i < Capacity; i++) spaces.Add(new());
    }


    public int Amount => spaces.Count(space => space.card != null);

    public bool HaveMonster()
    {
        return spaces.Any(space => space.card != null);
    }


    public bool HaveAttacker()
    {
        foreach (var space in spaces)
        {
            if (space.card == null) continue;
            if (space.position is MonsterPosition.Attack) return true;
        }

        return false;
    }


    public CardSpace_Combat Get(int index)
    {
        return spaces[index];
    }

    public CardSpace_Combat FindFromID(string cardGuid)
    {
        return spaces.FirstOrDefault(s => s.card != null && s.card.Guid == cardGuid);
    }
    
    public CardSpace_Combat Get(string cardGuild)
    {
        return spaces.FirstOrDefault(s => s.card != null && s.card.Guid == cardGuild);
    }

    public CardSpace_Combat GetFirstAttacker()
    {
        return spaces.FirstOrDefault(space => space.card != null && space.position is MonsterPosition.Attack);
    }

    public CardSpace_Combat GetRandomAttacker()
    {
        return spaces.Where(space => space.card != null && space.position is MonsterPosition.Attack)
            .OrderBy(s => Random.Range(0, 1f)).FirstOrDefault();
    }

    public CardSpace_Combat GetRandomMonster()
    {
        return spaces.Where(space => space.card != null).OrderBy(s => Random.Range(0, 1f)).FirstOrDefault();
    }

    public List<ServerCard> GetAllAttacker()
    {
        return spaces.Where(s => s.card != null && s.position is MonsterPosition.Attack).Select(s => s.card).ToList();
    }

    public int AttackerAmount()
    {
        return spaces.Count(s => s.card != null && s.position is MonsterPosition.Attack);
    }

    public CardSpace_Combat GetFirstMonster()
    {
        return spaces.FirstOrDefault(space => space.card != null);
    }

    public ServerCard Die(string guild)
    {
        var card = Get(guild).card;

        var index = SearchIndex(guild);
        spaces[index] = new CardSpace_Combat();

        return card;
    }


    public int SearchIndex(string cardGuid)
    {
        for (var i = 0; i < spaces.Count; i++)
        {
            if (spaces[i].card != null && spaces[i].card.Guid == cardGuid) return i;
        }

        return CardLocation.NotFound;
    }

    public bool IsFull()
    {
        return spaces.All(space => space.card != null);
    }

    /// <summary>
    /// Trả về ô triệu hồi
    /// </summary>
    /// <returns></returns>
    public int Summon_Attack(ServerCard card) => Summon_Attack(card, FirstEmptyIndex());

    /// <summary>
    /// Trả về ô triệu hồi
    /// </summary>
    /// <returns></returns>
    public int Summon_Attack(ServerCard card, int index)
    {
        var space = spaces[index];

        space.card = card;
        space.position = MonsterPosition.Attack;
        // space.attack = FakeConfig.GetAttackBase(card.id);
        // space.defense = FakeConfig.GetDefenseBase(card.id);

        return index;
    }

    /// <summary>
    /// Trả về ô triệu hồi
    /// </summary>
    /// <returns></returns>
    public int Summon_Defense(ServerCard card) => Summon_Defense(card, FirstEmptyIndex());

    /// <summary>
    /// Trả về ô triệu hồi
    /// </summary>
    /// <returns></returns>
    public int Summon_Defense(ServerCard card, int index)
    {
        var space = spaces[index];

        space.card = card;
        space.position = MonsterPosition.Defense;
        // space.attack = FakeConfig.GetAttackBase(card.id);
        // space.defense = FakeConfig.GetDefenseBase(card.id);

        return index;
    }

    public MonsterPosition ChangePosition(string guid)
    {
        var index = SearchIndex(guid);
        return ChangePosition(index);
    }

    public MonsterPosition ChangePosition(int index)
    {
        var space = spaces[index];
        space.position = space.position is MonsterPosition.Attack ? MonsterPosition.Defense : MonsterPosition.Attack;

        return space.position;
    }

    public int FirstEmptyIndex()
    {
        for (var i = 0; i < spaces.Count; i++)
        {
            var emptySpace = spaces[i].card == null;
            if (emptySpace) return i;
        }

        throw new IndexOutOfRangeException("Something went wrong!");
    }

    public ServerCard Tribute(string guid)
    {
        var card = Get(guid).card;

        var index = SearchIndex(guid);
        spaces[index] = new CardSpace_Combat();

        return card;
    }

    public void Clear()
    {
        for (int i = 0; i < spaces.Count; i++)
        {
            spaces[i].card = null;
        }
    }
}