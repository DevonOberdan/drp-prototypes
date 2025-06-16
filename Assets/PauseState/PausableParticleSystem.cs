using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PausableParticleSystem : MonoBehaviour, IPausable
{
    private ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
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
            particles.Pause();
        }
        else
        {
            particles.Play();
        }
    }
}
