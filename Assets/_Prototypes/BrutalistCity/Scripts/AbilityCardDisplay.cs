using TMPro;
using UnityEngine;

public class AbilityCardDisplay : MonoBehaviour
{
    [SerializeField] private AbilityCardSO abilityCardConfig;
    [SerializeField] private AbilityCardSOGameEvent broadcastCard;
    [SerializeField] private TMP_Text type;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    private void Start()
    {
        type.text = abilityCardConfig.TypeLabel;
        title.text = abilityCardConfig.Title;
        description.text = abilityCardConfig.Description;
    }

    public void Select()
    {
        broadcastCard.Raise(abilityCardConfig);
    }
}
