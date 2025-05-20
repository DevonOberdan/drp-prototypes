using System.Collections;
using UnityEngine;

public class Juice : MonoBehaviour
{
    public static Juice Instance;

    [SerializeField] float fovTimeScale = 10f;
    [SerializeField] float expandedFOV;

    [Header("References")]
    [SerializeField] Camera playerCam;

    float defaultFOV;

    private void Start()
    {
        defaultFOV = playerCam.fieldOfView;
    }

    public void ExpandFOV(bool expand)
    {
        float newFOV = expand ? expandedFOV : defaultFOV;

        if (newFOV != playerCam.fieldOfView)
        {
            StopAllCoroutines();
            StartCoroutine(ShiftFOV(newFOV));
        }
    }

    private IEnumerator ShiftFOV(float newFOV)
    {
        while(playerCam.fieldOfView != newFOV)
        {
            playerCam.fieldOfView = Mathf.MoveTowards(playerCam.fieldOfView, newFOV, Time.deltaTime * fovTimeScale);
            yield return null;
        }
    }

    private void OnEnable()
    {
        Instance = this;
    }
    private void OnDisable()
    {
        Instance = null;
    }
}