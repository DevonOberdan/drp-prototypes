using DG.Tweening;
using UnityEngine;

public class LaserSource : MonoBehaviour
{
    [SerializeField] private Transform sourceTransform;
    [SerializeField] private LaserBeam laserBeam;

    [SerializeField] private bool tween;
    [SerializeField] private bool startOn;

    private void Start()
    {
        if(laserBeam == null || !laserBeam.gameObject.activeInHierarchy)
        {
            this.enabled = false;
        }

        laserBeam.SetOn(startOn);
    }

    private void Update() 
    {
        Vector3 startPosition = sourceTransform.position;
        Vector3 direction = sourceTransform.forward;

        if(laserBeam.CurrentWidth > 0f)
        {
            laserBeam.Propagate(startPosition, direction);
        }
    }

    public void SetFire(bool fire)
    {
        if (fire)
            Fire();
        else
            EndFire();
    }

    public void Fire()
    {
        laserBeam.CurrentWidth = 0f;
        laserBeam.SetOn(true);

        if (tween)
        {
            DOTween.To(() => laserBeam.CurrentWidth, x => laserBeam.CurrentWidth = x, laserBeam.defaultWidth, 0.25f);
        }
        else
        {
            laserBeam.CurrentWidth = laserBeam.defaultWidth;
        }
    }

    public void EndFire()
    {
        if (tween)
        {
            DOTween.To(() => laserBeam.CurrentWidth, x => laserBeam.CurrentWidth = x, 0f, 0.25f).OnComplete(() => laserBeam.SetOn(false));
        }
        else
        {
            laserBeam.CurrentWidth = 0f;
            laserBeam.SetOn(false);
        }
    }
}
