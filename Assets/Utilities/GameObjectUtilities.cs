using UnityEngine;
using UnityEngine.UI;

public class GameObjectUtilities : MonoBehaviour
{
    public void ToggleGO()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SetDisabled(bool enabled) => gameObject.SetActive(!enabled);

    public void SetButtonDisabled(bool enabled)
    {
        if(TryGetComponent(out Button button))
        {
            button.interactable = !enabled;
        }
    }

    public void SetCanvasGroupVisibility(bool visible)
    {
        if(TryGetComponent(out CanvasGroup cg))
        {
            cg.alpha = visible ? 1 : 0;
            cg.interactable = visible;
            cg.blocksRaycasts = visible;
        }
    }
}