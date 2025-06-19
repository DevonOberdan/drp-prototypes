using FinishOne.GeneralUtilities;
using UnityEngine;

[RequireComponent(typeof(InteractionBuffer))]
public class PausableBuffer : MonoBehaviour, IPausable
{
    private InteractionBuffer buffer;

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
        if (buffer == null)
            buffer = GetComponent<InteractionBuffer>();

        buffer.enabled = !pause;
    }
}
