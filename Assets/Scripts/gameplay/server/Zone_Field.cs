using System;
using battle.define;
using Gameplay.board;
using Gameplay.card.ui;
using Networks;
using Sirenix.OdinInspector;

[Serializable]
public class Zone_Field
{
    [ShowInInspector] public CardSpace oldSpace = new();
    [ShowInInspector] public CardSpace space = new();

    public void Set(ServerCard card)
    {
        oldSpace.card = space.card;
        space.card = card;
    }

    public int SearchIndex(string cardGuid)
    {
        if (space.card != null && space.card.Guid == cardGuid)
        {
            return 0;
        }

        if (oldSpace.card != null && oldSpace.card.Guid == cardGuid)
        {
            return 0;
        }

        return CardLocation.NotFound;
    }

    public bool IsActive() => space.card != null;

    public ServerCard SendOldToGraveyard()
    {
        var sendCard = oldSpace.card;
        oldSpace.card = null;

        return sendCard;
    }

    public void Sync(int playerIndex, SyncTemp_TableCard tempCard)
    {
        // không còn nữa
        if (tempCard == null)
        {
            SendOldToGraveyard();
        }
        else
        {
            // card hiện tại
            if (space.card != null && space.card.id == tempCard.id)
            {
                return;
            }
            else
            {
                var fieldCard = tempCard.To_ServerCard();
                DueCardQuery.InitCard(fieldCard);

                Set(fieldCard);

                var @event = new Event_SetField()
                {
                    team = (Team)playerIndex,
                    serverCard = fieldCard,
                };

                Network.Event.Fighting.SetField?.Invoke(@event);
            }
        }
    }
}

[Serializable]
public class CardSpace
{
    public ServerCard card;
}

public class CardSpace_Combat
{
    public ServerCard card;
    public MonsterPosition position;
    public string face;
}