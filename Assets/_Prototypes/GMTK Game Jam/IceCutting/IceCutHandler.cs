using FinishOne.GeneralUtilities;
using LibTessDotNet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IceCutHandler : MonoBehaviour
{
    [SerializeField] private int minimumPointGap = 15;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }


    private void DetectLoop()
    {
        Vector3 newPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        for (int i = 0; i < lineRenderer.positionCount - minimumPointGap; i++)
        {
            if (Vector3.Distance(newPoint, lineRenderer.GetPosition(i)) < 0.15f)
            {
                HandleIceCut(i);
            }
        }
    }

    private void HandleIceCut(int pointIndex)
    {
        List<Vector2> loopPoints = new();

        for (int i = pointIndex; i < lineRenderer.positionCount - 1; i++)
        {
            Vector3 p = lineRenderer.GetPosition(i);
            loopPoints.Add(new(p.x, p.z));
        }

        Mesh loopMesh = Triangulate(loopPoints);
        SpawnMesh(loopMesh);
    }

    private void SpawnMesh(Mesh mesh)
    {
        GameObject filledShape = new GameObject("Hole");
        MeshFilter meshFilter = filledShape.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        filledShape.transform.Rotate(Vector3.right, 90);
        filledShape.transform.position = filledShape.transform.position.WithNew(y: 0.15f);

        MeshRenderer meshRenderer = filledShape.AddComponent<MeshRenderer>();
        meshRenderer.material = waterMat;
    }


    public Mesh Triangulate(List<Vector2> loop)
    {
        Tess tess = new Tess();

        ContourVertex[] contour = loop.Select(p => new ContourVertex
        {
            Position = new Vec3(p.x, p.y, 0),
            Data = null
        }).ToArray();

        tess.AddContour(contour, ContourOrientation.Original);
        tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3);

        Vector3[] vertices = new Vector3[tess.Vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = new Vector3(tess.Vertices[i].Position.X, tess.Vertices[i].Position.Y, 0);

        int[] indices = tess.Elements;

        Mesh mesh = new()
        {
            vertices = vertices,
            triangles = indices
        };

        return mesh;
    }

}
