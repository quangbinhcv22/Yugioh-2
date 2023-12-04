using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_DamageText : Singleton<UI_DamageText>
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Vector3 offset = Vector3.up * 15f;
    [SerializeField] private Ease ease = Ease.OutQuad;

    public void Show(Vector3 position, int damage)
    {
        gameObject.SetActive(true);
        
        text.SetText($"{damage}");
        
        transform.position = position;
        transform.DOMove(position + offset, duration).SetEase(ease).onComplete += () =>
        {
            gameObject.SetActive(false);
        };
    }
}