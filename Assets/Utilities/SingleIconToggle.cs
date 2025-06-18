using UnityEngine;
using UnityEngine.UI;

public class SingleIconToggle : MonoBehaviour
{
    [SerializeField] Sprite toggleSprite;

    private Image image;

    private readonly Color INACTIVE_COLOR = new(0, 0, 0, 0);

    public void SetActive(bool value)
    {
        image.sprite = value ? toggleSprite : null;
        image.color = value ? Color.black : INACTIVE_COLOR;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }
}
