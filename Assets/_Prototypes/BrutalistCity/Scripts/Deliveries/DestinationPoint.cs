using FinishOne.GeneralUtilities;
using UnityEngine;
using UnityEngine.Events;

public class DestinationPoint : MonoBehaviour
{
    [SerializeField] private Transform waypointRoot;
    [SerializeField] private UnityEvent OnReached;
    private TriggerEvent trigger;
    public Transform WayPointRoot 
    {
        get 
        {
            if(waypointRoot == null)
                waypointRoot = transform;

            return waypointRoot;
        }
    }
    public bool WaypointActive => waypointRoot.childCount > 0;
    public void SetupWaypoint(GameObject prefab)
    {
        GameObject waypoint = Instantiate(prefab, WayPointRoot);

        if (waypoint.TryGetComponent(out trigger))
        {
            trigger.TriggerEnterEvent.AddListener(() => OnReached.Invoke());
        }
    }

    internal void ClearWaypoint()
    {
        if (trigger != null)
        {
            trigger.TriggerEnterEvent.RemoveListener(() => OnReached.Invoke());
        }

        for (int i = WayPointRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(WayPointRoot.GetChild(i).gameObject);
        }
    }
}
