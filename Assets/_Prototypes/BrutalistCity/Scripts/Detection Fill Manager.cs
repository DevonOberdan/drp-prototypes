using UnityEngine;

public class DetectionFillManager : MonoBehaviour
{
    Material EyeMat;

    public float fillHeight;
    public Color fillColor;
    public Color defaultColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EyeMat = GetComponent<Renderer>().materials[1];

    }

    // Update is called once per frame
    void Update()
    {
        EyeMat.SetFloat("_Fill_Height", fillHeight);

    }
}
