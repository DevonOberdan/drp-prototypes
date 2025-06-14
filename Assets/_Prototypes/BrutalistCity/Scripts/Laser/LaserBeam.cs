using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBeam : MonoBehaviour
{
    public float defaultWidth = 0.1f;

    [SerializeField] private float maxDistance = 100f;

    [field: SerializeField] public Vector3 StartPosition { get; private set; }
    [field: SerializeField] public Vector3 EndPosition { get; private set; }
    [field: SerializeField] public LaserBeam Prefab { get; private set; }

    private OpticalElement _opticalElementThatTheBeamHit;
    private LineRenderer _lineRenderer;

    public Vector3 HitNormal { get; private set; }
    public Vector3 Direction => (EndPosition - StartPosition).normalized;

    public bool IsOn => _lineRenderer.enabled;

    public float CurrentWidth 
    {
        get => _lineRenderer.startWidth;
        set 
        {
            _lineRenderer.startWidth = value;
            _lineRenderer.endWidth = value;
        }
    }

    public OpticalElement OpticalElementThatTheBeamHit 
    { 
        get => _opticalElementThatTheBeamHit; 
        set 
        {
            if (_opticalElementThatTheBeamHit == value) 
            {
                return;
            }
            else 
            {
                if (_opticalElementThatTheBeamHit != null) 
                {
                    _opticalElementThatTheBeamHit.UnregisterLaserBeam(this);
                }

                _opticalElementThatTheBeamHit = value;

                if (_opticalElementThatTheBeamHit != null) 
                {
                    _opticalElementThatTheBeamHit.RegisterLaserBeam(this);
                }
            }
        }
    }


    private void Awake() 
    {                                                                                                               
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = defaultWidth;
        _lineRenderer.endWidth = defaultWidth;
    }

    public void Propagate(Vector3 startPosition, Vector3 direction) 
    {
        Vector3 endPosition = startPosition + direction * maxDistance;
        Vector3 hitNormal = Vector3.zero;

        if (Physics.Raycast(startPosition, direction, out RaycastHit hit, maxDistance)) 
        {
            endPosition = hit.point;
            hitNormal = hit.normal;

            OpticalElementThatTheBeamHit = hit.collider.TryGetComponent(out OpticalElement opticalElement) ? opticalElement : null;

            if(hit.collider.TryGetComponent(out ILaserImpact laserImpact) && IsOn && CurrentWidth == defaultWidth)
            {
                laserImpact.Hit();
            }
        }
        else 
        {
            OpticalElementThatTheBeamHit = null;
        }

        StartPosition = startPosition;
        EndPosition = endPosition;
        HitNormal = hitNormal;
        UpdateVisuals();

        if (OpticalElementThatTheBeamHit) {
            OpticalElementThatTheBeamHit.Propagate(this);
        }
    }

    public void SetOn(bool isOn)
    {
        _lineRenderer.enabled = isOn;
    }

    private void UpdateVisuals()
    {
        _lineRenderer.SetPosition(0, StartPosition);
        _lineRenderer.SetPosition(1, EndPosition);
    }
}