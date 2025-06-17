using TMPro;
using UnityEngine;

public interface ITextDisplay
{
    Awaitable SetText(string text);
    void CompleteText();
}

public class TextTypeEffect : MonoBehaviour, ITextDisplay
{
    [SerializeField] private float charactersPerSecond = 20f;

    private TMP_Text textComponent;
    private string textToType;

    private int counter = 0;

    void Awake()
    {
        if(!TryGetComponent(out textComponent))
        {
            Debug.LogError("TextMeshPro component not found!");
            enabled = false;
            return;
        }
    }

    public async Awaitable SetText(string text)
    {
        textToType = text;


        textComponent.maxVisibleCharacters = 0;
        textComponent.text = textToType;
        counter = 0;

        while (counter < textToType.Length)
        {
            counter++;
            textComponent.maxVisibleCharacters = counter;
            float interval = 1f / charactersPerSecond;
            await AwaitableMethods.WaitUntil(() => !PauseManager.PauseState);
            await Awaitable.WaitForSecondsAsync(interval);
        }
    }

    public void CompleteText()
    {
        counter = textToType.Length;
        textComponent.maxVisibleCharacters = textToType.Length;
    }
}
