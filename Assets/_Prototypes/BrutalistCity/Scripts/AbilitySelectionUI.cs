using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilitySelectionUI : MonoBehaviour
{
    [SerializeField] private Transform cardRoot;
    [SerializeField] private UpgradePool upgradePool;

    private List<AbilityCardDisplay> cards;

    void Start()
    {
        cards = cardRoot.GetComponentsInChildren<AbilityCardDisplay>().ToList();
    }

    public void DisplayCards(bool display)
    {
        if (display)
        {
            PullNewCards();
        }
    }

    private void PullNewCards()
    {
        List<AbilityCardSO> newAbilities = upgradePool.Grab(cards.Count);

        cards.ForEach(c => c.gameObject.SetActive(false));

        for (int i = 0; i < newAbilities.Count; i++)
        {
            cards[i].SetConfig(newAbilities[i]);
            cards[i].gameObject.SetActive(true);
        }
    }
}
