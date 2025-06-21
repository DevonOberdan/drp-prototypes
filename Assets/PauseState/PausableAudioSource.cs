using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PausableAudioSource : MonoBehaviour, IPausable
{
    private AudioSource source;

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
        if (source == null)
            source = GetComponent<AudioSource>();

        if (pause)
        {
            source.Pause();
        }
        else
        {
            source.UnPause();
        }
    }
}
