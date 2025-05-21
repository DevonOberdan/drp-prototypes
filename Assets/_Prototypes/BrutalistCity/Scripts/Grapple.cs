using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Reference")]
    private FPSMovementRB fpsMovement;
    public Transform cam;
    public Transform grappleTip;
    public LayerMask grappleLayers;
    public LineRenderer grappleLr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grappleCd;
    private float grappleCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fpsMovement = GetComponent<FPSMovementRB>();
    }

    private void StartGrapple()
    {
        if (grappleCdTimer > 0) return;

        grappling = true;

        fpsMovement.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, grappleLayers))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        grappleLr.enabled = true;
        grappleLr.SetPosition(1, grapplePoint);

    }

    private void ExecuteGrapple()
    {
        fpsMovement.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        fpsMovement.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(StopGrapple), 1f);
    }

    private void StopGrapple()
    {
        fpsMovement.freeze = false;
        grappling = false;
        grappleCdTimer = grappleCd;

        grappleLr.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(grappleKey)) StartGrapple();

        if(grappleCdTimer > 0)
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
}
