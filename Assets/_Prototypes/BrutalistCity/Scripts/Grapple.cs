using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Reference")]
    private FPSMovementRB fpsMovement;

    public Transform grappleTip;
    public LayerMask grappleLayers;
    public LineRenderer grappleLr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    [Header("Cooldown")]
    public float grappleCd;
    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

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
        fpsMovement = GetComponent<FPSMovementRB>();
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

        fpsMovement.Freeze = true;

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
        fpsMovement.Freeze = false;

        float grapplePointRelativeYPos = grapplePoint.y - (transform.position.y - 1f);
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        // if grapple downwards, do not add velocity in y-direction
        fpsMovement.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        fpsMovement.Freeze = false;
        grappling = false;
        grappleCdTimer = grappleCd;

        grappleLr.enabled = false;
    }
}
