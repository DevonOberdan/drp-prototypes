using UnityEngine;

public class DestinationPoint : MonoBehaviour
{
    [SerializeField] private Transform waypointRoot;

    public Transform WayPointRoot 
    {
        get 
        {
            if(waypointRoot == null)
                waypointRoot = transform;

            return waypointRoot;
        }
    }
}
