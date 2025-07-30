using FinishOne.GeneralUtilities;
using UnityEngine;

public class TracePlayerPath : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private float maxDist = 0.1f;

    private float step = 0.1f;
    private float time = 0;

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
    }
}
