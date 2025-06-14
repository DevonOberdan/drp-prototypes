using UnityEngine;
using UnityEngine.Events;

public class PausableUnityEvents : MonoBehaviour, IPausable
{
    [SerializeField] private UnityEvent OnPaused;
    [SerializeField] private UnityEvent OnUnpaused;
    [SerializeField] private UnityEvent<bool> OnSetPaused;
    [SerializeField] private UnityEvent<bool> OnSetUnpaused;

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
            OnPaused.Invoke();
        }
        else
        {
            OnUnpaused.Invoke();
        }

        OnSetPaused.Invoke(pause);
        OnSetUnpaused.Invoke(!pause);
    }
}
