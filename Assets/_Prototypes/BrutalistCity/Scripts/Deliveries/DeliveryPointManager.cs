using FinishOne.GeneralUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private bool firstGroupOnStart;

    private DestinationPointGroup CurrentGroup => deliveryGroups[groupIndex];

    private int groupIndex;
    private int completedCount;

    private void Awake()
    {
        groupIndex = -1;
        completedCount = 0;
    }
    private void Start()
    {

        if (firstGroupOnStart)
        {
            SetupGroup();
        }
    }

    public void DeliveryPointReached()
    {
        completedCount++;

        if (completedCount == CurrentGroup.DestinationPoints.Count)
        {
            CurrentGroup.OnWaypointsCompleted.Invoke();

            foreach (DestinationPoint point in CurrentGroup.DestinationPoints)
            {
                point.ClearWaypoint();
            }
            StartNextGroup();
        }
    }

    public void SetupGroup()
    {
        completedCount = 0;

        foreach (DestinationPoint root in CurrentGroup.DestinationPoints)
        {
            root.SetupWaypoint(waypointPrefab.gameObject);
        }
    }
    public void StartNextGroup()
    {
        if(groupIndex < 0 || completedCount == CurrentGroup.DestinationPoints.Count)
        {
            groupIndex = Mathf.Clamp(groupIndex + 1, 0, deliveryGroups.Length);
            SetupGroup();
        }
    }
}
