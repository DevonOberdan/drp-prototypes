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
            PullNewCardsAtRandom();
        }
    }

    private void PullNewCardsAtRandom()
    {
        List<AbilityCardSO> newAbilities = upgradePool.GrabAtRandom(cards.Count);

        SetCardDisplays(newAbilities.ToArray());
    }

    public void PresentCard(AbilityCardSO card)
    {
        SetCardDisplays(card);
    }

    public void SetCardDisplays(params AbilityCardSO[] newAbilities)
    {
        cards.ForEach(c => c.gameObject.SetActive(false));

        for (int i = 0; i < newAbilities.Length; i++)
        {
            cards[i].SetConfig(newAbilities[i]);
            cards[i].gameObject.SetActive(true);
        }
    }
}
