using battle.define;
using Gameplay.card.ui;
using UnityEngine;

public class UI_SpaceSpellTrap : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private UI_Card uiCard;
    [SerializeField] public int uiIndex;

    private void OnEnable()
    {
        Notifier_DueData.Current.event_SetSpell += OnSet;
    }
    
    private void OnDisable()
    {
        Notifier_DueData.Current.event_SetSpell -= OnSet;
    }

    private void OnSet(Event_SetSpell data)
    {
        if(data.team != team || data.index != uiIndex) return;
        
        // var info = DueCardQuery.GetBattleInfo(data.guid);
        uiCard.gameObject.SetActive(true);
        uiCard.Binding(data.guid);
    }
}