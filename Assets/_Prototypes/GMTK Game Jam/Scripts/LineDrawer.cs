using FinishOne.GeneralUtilities;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] private Transform drawPoint;
    [SerializeField] private float maxDist = 0.1f;

    public UnityEvent OnAddPathPoint;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        ResetLine();
    }

    void Update()
    {
        if (lineRenderer == null || lineRenderer.positionCount == 0)
            return;

        if (Vector3.Distance(drawPoint.position, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) > maxDist)
        {
            AddPoint();
        }
    }

    private void AddPoint()
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, drawPoint.position.NewY(.15f));

        OnAddPathPoint.Invoke();
    }

    public void Clear()
    {
        lineRenderer.positionCount = 0;
    }

    public void ResetLine()
    {
        Clear();
        AddPoint();
    }
}
