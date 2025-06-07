using UnityEngine;

public static class ExtensionMethods
{
    public static float NormalizeAngle(this float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    public static float WrapAngle(this float angle)
    {
        angle %= 360f;
        if (angle < 0f) angle += 360f;
        return angle;
    }
}
