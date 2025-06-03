using TMPro;
using UnityEngine;

public class AbilityCardDisplay : MonoBehaviour
{
    [SerializeField] private AbilityCardSO abilityCardConfig;

    [SerializeField] private TMP_Text type;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    private void Start()
    {
        type.text = abilityCardConfig.Type;
        title.text = abilityCardConfig.Title;
        description.text = abilityCardConfig.Description;
    }

    private void Update()
    {
        
    }

    public void Select()
    {
        // setup gameObject 
    }
}
