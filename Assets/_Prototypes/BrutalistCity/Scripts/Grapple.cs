using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform grappleTip;
    [SerializeField] private LayerMask grappleLayers;
    [SerializeField] private LineRenderer grappleLr;

    [Header("Grappling")]
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float grappleDelayTime;
    [SerializeField] private float overshootYAxis;

    [Header("Cooldown")]
    [SerializeField] private float grappleCd;
    [Header("Input")]
    [SerializeField] private KeyCode grappleKey = KeyCode.Mouse1;

    private FPSMovementRB playerController;
    private Camera cam;
    private Vector3 grapplePoint;
    private float grappleCdTimer;
    private bool grappling;

    private void Awake()
    {
        if(Camera.main == null)
        {
            Debug.LogError("Player camera should be set to the `MainCamera` tag.");
            return;
        }

        cam = Camera.main;
        playerController = GetComponentInParent<FPSMovementRB>();
        playerController.onCollision += StopGrapple;
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }

        if (grappleCdTimer > 0)
        {
            grappleCdTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            grappleLr.SetPosition(0, grappleTip.position);
        }
    }

    private void StartGrapple()
    {
        if (grappleCdTimer > 0)
            return;

        grappling = true;

        playerController.Freeze = true;

        if (Physics.Raycast(cam.transform.position, cam.transform.transform.forward, out RaycastHit hit, maxGrappleDistance, grappleLayers))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.transform.position + cam.transform.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        grappleLr.enabled = true;
        grappleLr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        playerController.Freeze = false;

        float grapplePointRelativeYPos = grapplePoint.y - (transform.position.y - 1f);
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        playerController.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        playerController.Freeze = false;
        grappling = false;
        grappleCdTimer = grappleCd;

        grappleLr.enabled = false;
    }
}
