using FinishOne.GeneralUtilities;
using UnityEngine;

public class DetectionFillManager : MonoBehaviour
{
    Material mat;

    private enum Axis { X, Y, Z };

    [SerializeField] private int materialIndex;

    [ColorUsageAttribute(false, true), SerializeField] private Color fillColor;

    [SerializeField] private Axis axis;
    [SerializeField] private bool flipDirection;

    [SerializeField] private bool clampRange;

    [DrawIf(nameof(clampRange), true)]
    [SerializeField] private Vector2 Range;

    private void Awake()
    {
        if (!TryGetComponent(out Renderer rend))
        {
            Debug.LogError("Renderer must be attached.", gameObject);
        }

        if (!rend.materials.IsValidIndex(materialIndex))
        {
            Debug.LogError("Provided materialIndex is invalid.", gameObject);
        }

        mat = rend.materials[materialIndex];

        mat.SetColor("_Fill_Color", fillColor);
        mat.SetFloat("_Min", GetAxisValue(rend.localBounds.min));
        mat.SetFloat("_Max", GetAxisValue(rend.localBounds.max));
        mat.SetVector("_FillAxis", GetAxisVector());
        mat.SetInt("_FlipFill", flipDirection ? 1 : 0);

        if (!clampRange)
        {
            Range = new Vector2(0, 1);
        }
    }

    private Vector3 GetAxisVector()
    {
        if (axis == Axis.X)
            return Vector3.right;
        else if (axis == Axis.Y)
            return Vector3.up;

        return Vector3.forward;
    }

    private float GetAxisValue(Vector3 vec)
    {
        if (axis == Axis.X)
            return vec.x;
        else if (axis == Axis.Y)
            return vec.y;

        return vec.z;
    }

    [ContextMenu("SetMinMax")]
    private void SetupMinMax()
    {
        Renderer rend = GetComponent<Renderer>();
        mat = rend.sharedMaterials[materialIndex];

        mat.SetColor("_Fill_Color", fillColor);
        mat.SetFloat("_Min", GetAxisValue(rend.localBounds.min));
        mat.SetFloat("_Max", GetAxisValue(rend.localBounds.max));
        mat.SetVector("_FillAxis", GetAxisVector());
        mat.SetInt("_FlipFill", flipDirection ? 1 : 0);
    }

    public void SetFill(float percent)
    {
        percent = Mathf.Clamp01(percent);
        float input = clampRange ? Mathf.Lerp(Range.x, Range.y, percent) : percent;
        mat.SetFloat("_Percent", input);
    }

    private void OnValidate()
    {
        Range.x = Mathf.Max(Range.x, 0);
        Range.y = Mathf.Min(Range.y, 1);
    }
}
