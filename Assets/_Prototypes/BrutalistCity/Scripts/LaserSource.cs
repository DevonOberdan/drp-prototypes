using DG.Tweening;
using UnityEngine;


public class LaserSource : MonoBehaviour
{
    [SerializeField] private Transform sourceTransform;
    [SerializeField] private LaserBeam laserBeam;

    [SerializeField] private float thinWidth = 0.2f;
    
    private void Start()
    {
        if(laserBeam == null || !laserBeam.gameObject.activeInHierarchy)
        {
            this.enabled = false;
        }
    }

    private void Update() 
    {
        Vector3 startPosition = sourceTransform.position;
        Vector3 direction = sourceTransform.forward;

        laserBeam.Propagate(startPosition, direction);
    }


    public void SetupThin()
    {
        laserBeam.defaultWidth = thinWidth;
    }

    public void SetToFullWidth(bool full)
    {
        DOTween.To(() => laserBeam.CurrentWidth, x => laserBeam.CurrentWidth = x, full ? laserBeam.defaultWidth : thinWidth, 0.25f);
    }

    public void SetLaser(bool isOn)
    {
        laserBeam.CurrentWidth = thinWidth;
        laserBeam.SetOn(isOn);
        this.enabled = isOn;
    }
}
