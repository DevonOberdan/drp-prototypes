using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] private VolumeProfile profile;

    private DepthOfField depthOfField;

    private Vignette vignette;
    public Vignette Vignette => vignette;

    public Vector2 VignetteRange { get; private set; } = new Vector2(0, 0.4f);

    private void Awake()
    {
        profile.TryGet(out depthOfField);
        profile.TryGet(out vignette);

        EnableVignette(false);
        SetVignetteIntensity(VignetteRange.x);
    }


    public void EnableDepthOfField(bool enable)
    {
        depthOfField.active = enable;
    }

    public void EnableVignette(bool enable)
    {
        Vignette.active = enable;
    }

    public void SetVignetteIntensity(float intensity)
    {
        Vignette.intensity.value = intensity;
    }

    private void OnDestroy()
    {
        EnableVignette(false);
        SetVignetteIntensity(VignetteRange.x);
        EnableDepthOfField(false);
    }
}