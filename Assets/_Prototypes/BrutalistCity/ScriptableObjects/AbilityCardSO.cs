using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(AbilityCardSO), menuName = nameof(AbilityCardSO), order = 0)]
public class AbilityCardSO : ScriptableObject
{
    public enum CardTypes { Ability, Item, Modification }

    public CardTypes Type;
    public string TypeLabel => Enum.GetName(typeof(CardTypes), Type);

    public string Title;
    public string Description;
    public GameObject AbilityPrefab;
}