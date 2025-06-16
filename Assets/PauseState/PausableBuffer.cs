using FinishOne.GeneralUtilities;
using UnityEngine;

[RequireComponent(typeof(InteractionBuffer))]
public class PausableBuffer : MonoBehaviour, IPausable
{
    private InteractionBuffer buffer;

    private void Awake()
    {
        buffer = GetComponent<InteractionBuffer>();
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
        buffer.enabled = !pause;
    }
}
