using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FlipCardAnimation : MonoBehaviour
{
    public Button button;
    public float flipDuration = 0.5f;
    public Ease ease;
    private bool isFlipped;

    public GameObject upFace;
    public GameObject downFace;

    private void Start()
    {
        // Attach a click listener to the image
        button.onClick.AddListener(OnCardClick);
    }

    private void OnCardClick()
    {
        if (!DOTween.IsTweening(transform))
        {
            if (isFlipped)
            {
                // Card is already flipped, so flip it back
                FlipCardBack();
            }
            else
            {
                // Card is not flipped, so flip it
                FlipCard();
            }
        }
    }

    private bool sideChanged;


    private void FlipCard()
    {
        sideChanged = false;

        transform.DOScaleX(-1f, flipDuration).SetEase(ease).OnUpdate(() =>
        {
            if (!sideChanged && IsConverting())
            {
                sideChanged = true;
                SetFront();
            }
        }).OnComplete(() => { isFlipped = true; });
    }


    private bool IsConverting()
    {
        return Mathf.Abs(transform.localScale.x) <= 0.05f;
    }


    private void SetFront()
    {
        upFace.SetActive(true);
        downFace.SetActive(false);
    }

    private void SetBack()
    {
        upFace.SetActive(false);
        downFace.SetActive(true);
    }


    private void FlipCardBack()
    {
        sideChanged = false;

        transform.DOScaleX(1f, flipDuration).SetEase(ease).OnUpdate(() =>
        {
            if (!sideChanged && IsConverting())
            {
                sideChanged = true;
                SetBack();
            }
        }).OnComplete(() => { isFlipped = false; });
    }
}