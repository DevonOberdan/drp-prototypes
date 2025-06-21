using FinishOne.GeneralUtilities;
using System;
using System.Collections.Generic;
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
    [SerializeField] private float punctuationDelayFactor = 5f;
    
    private TMP_Text textComponent;
    private string textToType;
    private bool typeWhenPaused;
    private int counter = 0;

    private readonly List<char> PauseChars = new() { '.', '!' };

    void Awake()
    {
        if(!TryGetComponent(out textComponent))
        {
            Debug.LogError("TextMeshPro component not found!");
            enabled = false;
            return;
        }
    }

    public async Awaitable SetText(NotificationSO textData, AudioSource audioSource)
    {
        textToType = textData.NotificationMessage;
        CharacterSO character = textData.Character;

        textComponent.maxVisibleCharacters = 0;
        textComponent.text = textToType;
        counter = 0;

        int paceCount = 0;
        float interval = 1f / charactersPerSecond;

        if(character != null && character.SinglePlay)
        {
            audioSource.clip = character.VoiceClip;
            audioSource.pitch = UnityEngine.Random.Range(character.PitchRange.x, character.PitchRange.y);
            audioSource.Play();
        }
        else
        {
            audioSource.clip = null;
        }

        while (counter < textToType.Length)
            {
                textComponent.maxVisibleCharacters = counter + 1;
                char c = textComponent.text[counter];

                if (IsAudibleChar(c) && textData != null && character != null && !character.SinglePlay && character.VoiceClip != null)
                {
                    if (paceCount >= character.VoicePace)
                    {
                        audioSource.pitch = UnityEngine.Random.Range(character.PitchRange.x, character.PitchRange.y);
                        audioSource.PlayOneShot(character.VoiceClip);
                        paceCount = 0;
                    }
                    paceCount++;
                }

                if (JustPassedPunctuation() && Char.IsWhiteSpace(c))
                {
                    await Awaitable.WaitForSecondsAsync(interval * punctuationDelayFactor);
                }

                await AwaitableMethods.WaitUntil(() => !PauseManager.PauseState || textData.PlayWhenPaused);
                await Awaitable.WaitForSecondsAsync(interval);

                counter++;
            }
    }

    private bool JustPassedPunctuation()
    {
        return textComponent.text.IsValidIndex(counter - 1) && PauseChars.Contains(textComponent.text[counter - 1]);
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
