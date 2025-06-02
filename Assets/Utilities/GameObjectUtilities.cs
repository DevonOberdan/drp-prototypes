using System.Collections;
using System.Collections.Generic;
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
}