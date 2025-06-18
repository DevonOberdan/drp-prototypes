using UnityEngine;
using UnityEngine.Events;

public class EventOnStartup : MonoBehaviour
{
    private enum Startup { AWAKE, START }

    [SerializeField] private Startup startup;
    [SerializeField] private UnityEvent OnStartup;

    private void Awake()
    {
        if(startup == Startup.AWAKE)
        {
            OnStartup.Invoke();
        }
    }

    private void Start()
    {
        if(startup == Startup.START)
        {
            OnStartup.Invoke();
        }
    }
}
