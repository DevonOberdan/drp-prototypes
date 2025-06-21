using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
struct ControlDisplayGroup
{
    public List<GameObject> elements;
    public AbilityCardSO associatedCard;
}

public class AbilityControlDisplay : MonoBehaviour
{
    [SerializeField] List<ControlDisplayGroup> controlList;

    void Start()
    {
        foreach (var control in controlList)
        {
            foreach(var element in control.elements)
            {
                element.SetActive(false);
            }
        }
    }


    public void RevealControlsForAbility(AbilityCardSO card)
    {
        if(controlList.Where(g => card == g.associatedCard).Any() == false)
        {
            return;
        }

        ControlDisplayGroup group = controlList.First(g => card == g.associatedCard);

        foreach (var element in group.elements)
        {
            element.SetActive(true);
        }
    }
}
