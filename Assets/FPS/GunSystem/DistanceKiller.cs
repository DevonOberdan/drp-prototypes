using UnityEngine;
using UnityEngine.Events;

public class DistanceKiller : MonoBehaviour
{
    [SerializeField] UnityEvent OnDistanceReached;
    [SerializeField] private float MAX_DISTANCE = 1000f;

    void Update()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, Camera.main.transform.position)) > MAX_DISTANCE)
        {
            OnDistanceReached.Invoke();
        }
    }
}
