using UnityEngine;
using UnityEngine.Events;

public class DistanceKiller : MonoBehaviour
{
    [SerializeField] UnityEvent OnDistanceReached;

    readonly float MAX_DISTANCE = 100;
    readonly Vector3 origin = Vector3.zero;

    void Update()
    {
        if (Vector3.Distance(transform.position, origin) > MAX_DISTANCE)
        {
            OnDistanceReached.Invoke();
        }
    }
}
