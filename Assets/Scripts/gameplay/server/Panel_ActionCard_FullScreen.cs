using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_ActionCard_FullScreen : Singleton<Panel_ActionCard_FullScreen>
{
    [Space] [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private Text_Countdown textCountdown;

    [Space] [SerializeField] private Button btnYes;
    [SerializeField] private TMP_Text txtYes;

    [Space] [SerializeField] private Button btnNo;
    [SerializeField] private TMP_Text txtNo;


    public Action onYes;
    public Action onNo;


    private void Awake()
    {
        btnYes.onClick.AddListener(OnYes);
        btnNo.onClick.AddListener(OnNo);
    }

    private void OnYes()
    {
        Hide();
        onYes?.Invoke();
    }
    
    private void OnNo()
    {
        Hide();
        onNo?.Invoke();
    }


    public Panel_ActionCard_FullScreen Show()
    {
        onYes = null;
        onNo = null;
        
        textCountdown.gameObject.SetActive(false);
        btnYes.gameObject.SetActive(false);
        btnNo.gameObject.SetActive(false);
        
        gameObject.SetActive(true);
        
        return this;
    }
    
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public Panel_ActionCard_FullScreen SetTitle(string title)
    {
        txtTitle.SetText(title);

        return this;
    }

    public Panel_ActionCard_FullScreen UseCountdown(DateTime endTime)
    {
        textCountdown.gameObject.SetActive(true);
        textCountdown.StartCounting(endTime);
        return this;
    }

    public Panel_ActionCard_FullScreen UseButton_Yes(string label)
    {
        btnYes.gameObject.SetActive(true);
        btnYes.interactable = true;

        txtYes.SetText(label);

        return this;
    }
    
    public Panel_ActionCard_FullScreen SetInteractable_Yes(bool interactable)
    {
        btnYes.interactable = interactable;
        return this;
    }
    

    public Panel_ActionCard_FullScreen UseButton_No(string label)
    {
        btnNo.gameObject.SetActive(true);
        btnNo.interactable = true;

        txtNo.SetText(label);

        return this;
    }
    public Panel_ActionCard_FullScreen SetInteractable_No(bool interactable)
    {
        btnNo.interactable = interactable;
        return this;
    }
}