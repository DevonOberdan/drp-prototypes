using UnityEngine;
using UnityEngine.Events;

public class FilteredTriggerEvent : MonoBehaviour
{
    public UnityEvent TriggerEnterEvent;
    public UnityEvent TriggerExitEvent;

    [SerializeField] private string colliderTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(colliderTag))
        {
            TriggerEnterEvent.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(colliderTag))
        {
            TriggerExitEvent.Invoke();
        }
    }
}
