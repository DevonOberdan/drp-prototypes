using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPanelCheck : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> otherPanels;

    [SerializeField] private UIVisibilityController visibilityController;

    public void ConditionalVisibility(bool visible)
    {
        if(otherPanels.Where(p => p.alpha == 1).Any())
        {
            return;
        }

        visibilityController.SetUIVisible(visible);
    }
}
