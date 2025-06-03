using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private Transform itemContainer;
    [SerializeField] private Transform abilityContainer;

    public void AddNewAbility(AbilityCardSO abilityCard)
    {
        if (abilityCard.Type == AbilityCardSO.CardTypes.Ability)
        {
            GameObject.Instantiate(abilityCard.AbilityPrefab, abilityContainer);
        }
        if (abilityCard.Type == AbilityCardSO.CardTypes.Item)
        {
            for (int i = itemContainer.childCount; i > 0; i--)
            {
                GameObject child = itemContainer.GetChild(i).gameObject;
                Destroy(child);
            }

            GameObject.Instantiate(abilityCard.AbilityPrefab, itemContainer);
        }
    }
}
