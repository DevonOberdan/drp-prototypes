using System.Linq;
using UnityEngine;

public class FadeLinePoints : MonoBehaviour
{
    [SerializeField] private int maxPointCount;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(lineRenderer.positionCount > maxPointCount)
        {
            Vector3[] positions = new Vector3[lineRenderer.positionCount];

            int count = lineRenderer.GetPositions(positions);
            Vector3[] newPositions = positions.Skip(count - maxPointCount).ToArray();

            lineRenderer.positionCount = newPositions.Length;
            lineRenderer.SetPositions(newPositions);
        }
    }
}
