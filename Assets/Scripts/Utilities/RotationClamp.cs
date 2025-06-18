using System;
using System.Data;
using UnityEngine;

public class RotationClamp : MonoBehaviour
{
    [Serializable]
    private struct ClampRange
    {
        public float min, max;
    }

    [SerializeField] private bool clampX, clampY, clampZ;
    [SerializeField] private ClampRange xRange, yRange, zRange;

    private void Awake()
    {
        Clamp();
    }

    private void LateUpdate()
    {
        Clamp();
    }

    private void Clamp()
    {
        Vector3 euler = transform.localEulerAngles;

        if (clampX) euler.x = Mathf.Clamp(euler.x.NormalizeAngle(), xRange.min, xRange.max);
        if (clampY) euler.y = Mathf.Clamp(euler.y.NormalizeAngle(), yRange.min, yRange.max);
        if (clampZ) euler.z = Mathf.Clamp(euler.z.NormalizeAngle(), zRange.min, zRange.max);

        transform.localEulerAngles = new(euler.x.WrapAngle(), euler.y.WrapAngle(), euler.z.WrapAngle());
    }

    public Vector3 ClampRotation(Vector3 euler)
    {
        if (clampX) euler.x = Mathf.Clamp(euler.x.NormalizeAngle(), xRange.min, xRange.max);
        if (clampY) euler.y = Mathf.Clamp(euler.y.NormalizeAngle(), yRange.min, yRange.max);
        if (clampZ) euler.z = Mathf.Clamp(euler.z.NormalizeAngle(), zRange.min, zRange.max);

        return euler;
    }
}
