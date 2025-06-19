using System;
using TMPro;
using UnityEngine;

public interface ITextDisplay
{
    Awaitable SetText(NotificationSO textData, AudioSource audioSource);
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

    [Range(1, 5)]
    [SerializeField] private int pace = 1;

    public async Awaitable SetText(NotificationSO textData, AudioSource audioSource)
    {
        textToType = textData.NotificationMessage;

        textComponent.maxVisibleCharacters = 0;
        textComponent.text = textToType;
        counter = 0;

        int paceCount = 0;

        while (counter < textToType.Length)
        {
            textComponent.maxVisibleCharacters = counter+1;
            char c = textComponent.text[counter];

            if(IsAudibleChar(c) && textData != null && textData.VoiceClip != null)
            {
                if(paceCount >= pace)
                {
                    audioSource.pitch = UnityEngine.Random.Range(textData.PitchRange.x, textData.PitchRange.y);
                    audioSource.PlayOneShot(textData.VoiceClip);
                    paceCount = 0;
                }
                paceCount++;
            }

            float interval = 1f / charactersPerSecond;
            await AwaitableMethods.WaitUntil(() => !PauseManager.PauseState);
            await Awaitable.WaitForSecondsAsync(interval);

            counter++;
        }
    }

    private bool IsAudibleChar(char c)
    {
        return Char.IsLetterOrDigit(c);
    }

    public void CompleteText()
    {
        counter = textToType.Length;
        textComponent.maxVisibleCharacters = textToType.Length;
    }
}
