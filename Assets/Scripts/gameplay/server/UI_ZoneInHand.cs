using System;
using System.Collections.Generic;
using System.Linq;
using battle.define;
using event_name;
using Gameplay.card.ui;
using Sirenix.OdinInspector;
using TigerForge;
using UnityEngine;

public class UI_ZoneInHand : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private UI_Card prefab;
    
    [ShowInInspector, HideInEditorMode] private List<UI_Card> _cards = new();


    private void OnEnable()
    {
        // EventManager.StartListening(EventName.Gameplay.Draw, OnDraw);
        EventManager.StartListening(EventName.Gameplay.ZoneChanged_InHand, OnChanged);

        PresentHandler_Zone_InHand.onClear += Clear;
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.Gameplay.ZoneChanged_InHand, OnChanged);
        
        PresentHandler_Zone_InHand.onClear -= Clear;
    }

    private void OnChanged()
    {
        var changed = EventManager.GetData(EventName.Gameplay.ZoneChanged_InHand) as Changed_ZoneInHand;
        if (!Client_DueManager.IsMine(team, changed.playerIndex)) return;

        switch (changed.type)
        {
            case ZoneChangeType.Add:
                OnAdd(changed);
                break;
            case ZoneChangeType.Remove:
                OnRemove(changed);
                break;
        }
    }

    private void OnAdd(Changed_ZoneInHand changed)
    {
        var usedCard = Client_DueManager.GetPlayer(team).zone.inHand.Get(changed.cardGuild);
        
        var cardGo = Instantiate(prefab, transform);
        _cards.Add(cardGo);

        if(team is Team.Opponent) cardGo.ShowHide();
        cardGo.Binding(usedCard.Guid);
    }

    private void OnRemove(Changed_ZoneInHand changed)
    {
        var toRemove = changed.playerIndex == (int) Team.Self ? _cards.First(c => c.Guid == changed.cardGuild) : _cards.First();
        _cards.Remove(toRemove);
        
        Destroy(toRemove.gameObject);
    }
    

    public void Clear()
    {
        foreach (var uiCard in _cards.ToList())
        {
            Destroy(uiCard.gameObject);
        }
        
        _cards.Clear();
    }
}

[Serializable]
public class Changed_ZoneInHand
{
    public int playerIndex;
    public string cardGuild;
    public ZoneChangeType type;
}