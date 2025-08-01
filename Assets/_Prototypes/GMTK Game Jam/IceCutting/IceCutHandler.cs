using FinishOne.GeneralUtilities;
using LibTessDotNet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineLoopDetector))]
public class IceCutHandler : MonoBehaviour
{
    public UnityEvent OnIceCut;
    
    [SerializeField] private Material waterMat;
    [SerializeField] private string holeTag = "Hole";

    [SerializeField] private float sizeMinimum = 2f;
    [SerializeField] private bool debug = false;

    private LineRenderer lineRenderer;
    private LineLoopDetector lineLoopDetector;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineLoopDetector = GetComponent<LineLoopDetector>();
    }

    private void Start()
    {
        lineLoopDetector.OnLoopCreated.AddListener(HandleIceCut);
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

        if (debug)
        {
            Debug.Log($"X: {loopMesh.bounds.size.x} Y: {loopMesh.bounds.size.y}");
        }

        if (loopMesh.bounds.size.x > sizeMinimum && loopMesh.bounds.size.y > sizeMinimum)
        {
            SpawnMesh(loopMesh);
            OnIceCut.Invoke();
        }
    }

    private void SpawnMesh(Mesh mesh)
    {
        GameObject filledShape = new("Hole");
        MeshFilter meshFilter = filledShape.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        filledShape.transform.Rotate(Vector3.right, 90);
        filledShape.transform.position = filledShape.transform.position.WithNew(y: 0.15f);

        MeshRenderer meshRenderer = filledShape.AddComponent<MeshRenderer>();
        meshRenderer.material = waterMat;

        _ = filledShape.AddComponent<MeshCollider>();
        filledShape.tag = holeTag;
    }


    public Mesh Triangulate(List<Vector2> loop)
    {
        Tess tess = new();

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

        return new Mesh()
        {
            vertices = vertices,
            triangles = indices
        };
    }

}
