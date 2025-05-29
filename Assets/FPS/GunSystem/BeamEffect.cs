using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public interface IBeamable
{
    void HitByBeam(RaycastHit hit);
}

public class BeamEffect : MonoBehaviour
{
    [Range(0.05f, 2f)]
    [SerializeField] float fireTime = 0.1f;

    public UnityEvent OnBeamStarted;

    MeshRenderer beamVisualMaterial;
    Tween fireBeamTween;

    const string SCROLL_SPEED = "_Scroll_Speed";

    public float MaxDistance { get; private set; }

    public bool FullBeam => transform.localScale.z == 1;

    private void Awake()
    {
        beamVisualMaterial = transform.GetChild(0).GetComponent<MeshRenderer>();
        MaxDistance = beamVisualMaterial.transform.localScale.y;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0);
        StartBeam();
    }

    public void StartBeam()
    {
        float activePercentage = 1;
        if (fireBeamTween.IsActive())
        {
            activePercentage = fireBeamTween.ElapsedPercentage();
            fireBeamTween.Kill();
        }

        fireBeamTween = transform.DOScaleZ(1, fireTime * activePercentage).SetEase(Ease.Linear);

        OnBeamStarted.Invoke();
    }


    public void EndBeam()
    {
        float activePercentage = 1;
        if (fireBeamTween.IsActive())
        {
            activePercentage = fireBeamTween.ElapsedPercentage();
            fireBeamTween.Kill();
        }

        fireBeamTween = transform.DOScaleX(0f, fireTime* activePercentage).SetEase(Ease.Linear).OnComplete(DestroyBeam);
        transform.DOScaleY(0, fireTime* activePercentage).SetEase(Ease.Linear);
    }

    private void DestroyBeam()
    {
        Destroy(gameObject);
    }

    public void FlipBeam()
    {
        beamVisualMaterial.material.SetFloat(SCROLL_SPEED, -beamVisualMaterial.material.GetFloat(SCROLL_SPEED));
    }
}
