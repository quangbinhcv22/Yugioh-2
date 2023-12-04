using System.Collections.Generic;
using System.Linq;
using Gameplay.card.ui;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Old_Panel_CardOptions : Singleton<Old_Panel_CardOptions>
{
    public Vector3 offset = new Vector3(0, 200f, 0);
    public Button buttonActive;
    public Button buttonSummon;
    public Button buttonSet;
    public Button buttonChangePosition;
    public TMP_Text notifyText;
    
    public Button buttonSpellSet;


    private string _cardGuid;


    private void Awake()
    {
        buttonSummon.onClick.AddListener(OnClick_Summon);
        buttonSet.onClick.AddListener(OnClick_Set);
        buttonChangePosition.onClick.AddListener(OnClick_ChangePosition);
        buttonSpellSet.onClick.AddListener(OnClick_SetSpell);
    }


    public Old_Panel_CardOptions Show(string cardGuid)
    {
        gameObject.SetActive(true);

        notifyText.gameObject.SetActive(false);
        buttonSummon.gameObject.SetActive(false);
        buttonSet.gameObject.SetActive(false);
        buttonChangePosition.gameObject.SetActive(false);

        var uiSlot = DueCardQuery.GetUICard(cardGuid);
        transform.position = uiSlot.transform.position + offset;

        _cardGuid = cardGuid;

        return this;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public Old_Panel_CardOptions ShowText(string content)
    {
        notifyText.gameObject.SetActive(true);
        notifyText.SetText(content);

        return this;
    }

    public Old_Panel_CardOptions UseAction_MonsterInHand()
    {
        buttonSummon.gameObject.SetActive(true);
        buttonSet.gameObject.SetActive(true);

        return this;
    }
    
    public Old_Panel_CardOptions UseAction_SpellInHand()
    {
        buttonSpellSet.gameObject.SetActive(true);

        return this;
    }
    

    public Old_Panel_CardOptions UseAction_MonsterInBoard()
    {
        buttonChangePosition.gameObject.SetActive(true);

        return this;
    }


    [ShowInInspector, HideInEditorMode] private List<Transform> allObjects;

    protected override void OnEnable()
    {
        base.OnEnable();

        allObjects = transform.GetAllChild();
        allObjects.Add(transform);

        CheckClick.Subscribe(gameObject);
        CheckClick.OnClick += OnClick;
    }


    private void OnDisable()
    {
        CheckClick.UnSubscribe(gameObject);
        CheckClick.OnClick -= OnClick;
    }

    private void OnClick(List<RaycastResult> raycastResults)
    {
        var clickMe = allObjects.Any(o => raycastResults.Any(r => o.gameObject == r.gameObject));
        if (!clickMe) Hide();
    }


    private void RequestActive()
    {
    }


    private void OnClick_SetSpell()
    {
        RequestSetSpell();
    }

    private void RequestSetSpell()
    {
        var myIndex = Client_DueManager.myIndex;
        var cardGuild = _cardGuid;

        Server_DueManager.main.InHand_Use(myIndex, cardGuild, InHand_CardUse.Set);

        Hide();
    }
    

    private void OnClick_Summon()
    {
        var requiredAmount = DueCardQuery.GetTributeRequire(_cardGuid);

        if (requiredAmount == 0)
        {
            RequestSummon();
        }
        else
        {
            CardAction_SummonTribute.Current.StartProcess(_cardGuid, InHand_CardUse.Summon);
            Hide();
        }
    }

    private void RequestSummon()
    {
        var myIndex = Client_DueManager.myIndex;
        var cardGuild = _cardGuid;
        
        Networks.Network.Request.Fighting.ReleaseHandCard(new ()
        {
            cardId = long.Parse(cardGuild),
            releaseType = "SUMMON",
        });
        

        Hide();
    }


    private void OnClick_Set()
    {
        var requiredAmount = DueCardQuery.GetTributeRequire(_cardGuid);

        if (requiredAmount == 0) RequestSet();
        else
        {
            CardAction_SummonTribute.Current.StartProcess(_cardGuid, InHand_CardUse.Set);
            Hide();
        }
    }

    private void RequestSet()
    {
        var myIndex = Client_DueManager.myIndex;
        var cardGuild = _cardGuid;

        // Server_DueManager.main.InHand_Use(myIndex, cardGuild, InHand_CardUse.Set);

        Networks.Network.Request.Fighting.ReleaseHandCard(new ()
        {
            cardId = long.Parse(cardGuild),
            releaseType = "SET",
        });
        
        Hide();
    }

    private void OnClick_ChangePosition()
    {
        CardAction_PhaseMain.Current.changePositionCards.Add(_cardGuid);
        Server_DueManager.main.ChangePosition(Client_DueManager.myIndex, _cardGuid);
        Hide();
    }
}