using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PausableParticleSystem : MonoBehaviour, IPausable
{
    private ParticleSystem particles;

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
        if (particles == null)
            particles = GetComponent<ParticleSystem>();

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
