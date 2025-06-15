using FinishOne.GeneralUtilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct DestinationPointGroup
{
    [field: SerializeField] public List<DestinationPoint> DestinationPoints { get; private set; }

    public UnityEvent OnWaypointsCompleted;
}

public class DeliveryPointManager : MonoBehaviour
{
    [SerializeField] private DestinationPointGroup[] deliveryGroups;

    [SerializeField] private Transform waypointPrefab;

    private DestinationPointGroup CurrentGroup => deliveryGroups[groupIndex];

    private int groupIndex;
    private int completedCount;

    void Start()
    {
        completedCount = 0;
        SetupGroup();
    }

    public void DeliveryPointReached()
    {
        completedCount++;

        if (completedCount == CurrentGroup.DestinationPoints.Count)
        {
            Debug.Log("Group done!");
            CurrentGroup.OnWaypointsCompleted.Invoke();

            foreach (DestinationPoint point in CurrentGroup.DestinationPoints)
            {
                for(int i = point.WayPointRoot.childCount - 1; i >= 0; i--)
                {
                    Destroy(point.WayPointRoot.GetChild(i).gameObject);
                }
            }

            groupIndex = Mathf.Clamp(groupIndex + 1, 0, deliveryGroups.Length);

            SetupGroup();
        }
    }

    private void SetupGroup()
    {
        completedCount = 0;

        foreach (var root in CurrentGroup.DestinationPoints)
        {
            Instantiate(waypointPrefab, root.WayPointRoot);
        }
    }
}
