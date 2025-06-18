using DG.Tweening;
using FinishOne.GeneralUtilities;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    [SerializeField] private float defaultFadeTime = 0.5f;
    [SerializeField] private UnityEvent OnFadeToBlack, OnFadeFromBlack;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetOpaque(bool opaque)
    {
        image.color = opaque ? Color.black : Color.clear;
        image.enabled = opaque;
    }

    public void FromBlack(float time, Action onComplete = null)
    {
        SetOpaque(true);
        image.DOColor(Color.clear, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            image.enabled = false;
            onComplete?.Invoke();
            OnFadeFromBlack.Invoke();
        });
    }

    public void ToBlack(float time, Action onComplete = null)
    {
        SetOpaque(false);
        image.enabled = true;

        image.DOColor(Color.black, time).SetEase(Ease.Linear).OnComplete(() => {
            onComplete?.Invoke();
            OnFadeToBlack.Invoke();
        });
    }

    public void FromBlack() => FromBlack(defaultFadeTime);
    public void ToBlack() => ToBlack(defaultFadeTime);

    public void FromBlack(float time) => FromBlack(time, null);
    public void ToBlack(float time) => ToBlack(time, null);

    public void ToBlack(GameEvent onComplete) => ToBlack(defaultFadeTime, () => onComplete.Raise());
    public void FromBlack(GameEvent onComplete) => FromBlack(defaultFadeTime, () => onComplete.Raise());
}
