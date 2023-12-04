using System;
using TMPro;
using UnityEngine;

public class Text_Countdown : MonoBehaviour
{
    [SerializeField] private string format = @"mm\:ss";
    [SerializeField] private string textDefault = @"";
    [SerializeField] private bool useTextDefault = false;
    [SerializeField] private bool stopWhenTimeOut = false;

    private DateTime _targetTime;
    private TMP_Text _text;


    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnDisable()
    {
        StopCounting();
    }


    public void StartCounting(DateTime targetTime)
    {
        _targetTime = targetTime;
        InvokeRepeating(nameof(Counting), 0f, 1f);
    }

    public void StopCounting()
    {
        CancelInvoke(nameof(Counting));

        if (useTextDefault)
        {
            _text.SetText(textDefault);
        }
    }

    private void Counting()
    {
        var remaining = _targetTime - DateTime.Now;
        if (remaining < TimeSpan.Zero)
        {
            remaining = TimeSpan.Zero;
            if (stopWhenTimeOut) StopCounting();
        }

        _text.SetText(remaining.ToString(format));
    }
}