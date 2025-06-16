using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PausableAudioSource : MonoBehaviour, IPausable
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Pause()
    {
        SetPause(true);
    }

    public void Unpause()
    {
        SetPause(false);
    }

    public void SetPause(bool pause)
    {
        if (pause)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }
}
