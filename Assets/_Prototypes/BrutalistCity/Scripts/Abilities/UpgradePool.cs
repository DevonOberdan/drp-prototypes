using FinishOne.GeneralUtilities;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePool : MonoBehaviour
{
    [SerializeField] private List<AbilityCardSO> FullAbilityList;

    private List<AbilityCardSO> AvailableAbilityPool;


    [SerializeField] private bool beginWithAllAbilities;

    [DrawIf(nameof(beginWithAllAbilities), true)]
    [SerializeField] private AbilityCardSOGameEvent SetupNewAbility;

    void Start()
    {
        AvailableAbilityPool = new(FullAbilityList);

        if (beginWithAllAbilities)
        {
            foreach (var ability in FullAbilityList)
            {
                SetupNewAbility.Raise(ability);
            }
        }
    }

    public List<AbilityCardSO> GrabAtRandom(int amount)
    {
        List<AbilityCardSO> tempPool = new(AvailableAbilityPool);
        List<AbilityCardSO> grabbedItems = new();

        for(int i = 0; i<amount; i++)
        {
            if(tempPool.Count == 0)
            {
                return grabbedItems;
            }

            var item = tempPool.RandomItem();
            tempPool.Remove(item);

            grabbedItems.Add(item);
        }

        return grabbedItems;
    }


    public void Remove(AbilityCardSO entry)
    {
        if (AvailableAbilityPool.Contains(entry))
        {
            AvailableAbilityPool.Remove(entry); 
        }
    }
}
