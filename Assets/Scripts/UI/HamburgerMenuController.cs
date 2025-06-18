using DG.Tweening;
using UnityEngine;

public class HamburgerMenuController : MonoBehaviour
{
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private GameObject buttons;
    [SerializeField] private Ease ease;
    [SerializeField] private float openTime = 0.15f;
    private Vector2 closedWidth, expandedWidth;

    private Tween tween;
    private void Awake()
    {
        closedWidth = new Vector2(0,0);
        expandedWidth = menuPanel.rect.size;
        Close();
    }

    public void OpenMenu(bool open)
    {
        if (tween.IsActive())
        {
            tween.Kill();
        }

        if (open)
        {
            tween = menuPanel.DOSizeDelta(expandedWidth, openTime) .SetEase(ease)
                             .OnComplete(() => buttons.SetActive(open));
        }
        else
        {
            Close();
        }
    }

    private void Close()
    {
        menuPanel.sizeDelta = closedWidth;
        buttons.SetActive(false);
    }

    private void OnDisable()
    {
        if (tween.IsActive())
        {
            tween.Kill();
        }
        Close();
    }
}