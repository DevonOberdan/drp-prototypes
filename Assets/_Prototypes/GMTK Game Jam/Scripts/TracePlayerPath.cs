using FinishOne.GeneralUtilities;
using UnityEngine;

public class TracePlayerPath : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private float maxDist = 0.1f;
    [SerializeField] private int minimumPointGap = 15;

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

            DetectLoop();
        }
    }

    private void AddPoint()
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position.NewY(.15f));
    }

    private void DetectLoop()
    {
        Vector3 newPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        for (int i = 0; i < lineRenderer.positionCount - minimumPointGap; i++)
        {
            if(Vector3.Distance(newPoint, lineRenderer.GetPosition(i)) < 0.15f)
            {
                Debug.Log("Closed loop!");
            }
        }
    }
}
