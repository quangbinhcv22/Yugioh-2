using System;
using Cysharp.Threading.Tasks;

public class Panel_ResultOfMatches : Singleton<Panel_ResultOfMatches>
{
    protected override async void OnEnable()
    {
        base.OnEnable();
        
        await UniTask.Delay(TimeSpan.FromSeconds(DueConstant.waitViewResult));
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}