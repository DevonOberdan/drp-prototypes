using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIVisibilityController : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> OnSetVisible;
    private CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    public void SetUIVisible(bool visible)
    {
        if (TryGetComponent(out CanvasGroup cg))
        {
            cg.alpha = visible ? 1 : 0;
            cg.blocksRaycasts = visible;
        }

        OnSetVisible.Invoke(visible);
    }

    public void SetUIHidden(bool hidden)
    {
        SetUIVisible(!hidden);
    }

    public void ToggleUIVisible()
    {
        SetUIVisible(!cg.blocksRaycasts);
    }
}
