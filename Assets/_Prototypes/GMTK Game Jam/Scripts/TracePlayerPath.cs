using FinishOne.GeneralUtilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TracePlayerPath : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private float maxDist = 0.1f;

    [SerializeField] private UnityEvent OnAddPathPoint;

    [SerializeField] private RawImage image;
    [SerializeField] private Material waterMat;

    void Start()
    {
        lineRenderer.positionCount = 0;
        AddPoint();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) > maxDist)
        {
            AddPoint();
        }
    }

    private void AddPoint()
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position.NewY(.15f));

        OnAddPathPoint.Invoke();
    }
}
