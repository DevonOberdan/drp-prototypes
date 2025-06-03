using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = nameof(AbilityCardSO), menuName = nameof(AbilityCardSO), order = 0)]
public class AbilityCardSO : ScriptableObject
{
    public string Type = "New Ability";
    public string Title;
    public string Description;
    public GameObject AbilityPrefab;
}