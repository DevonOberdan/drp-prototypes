using FinishOne.GeneralUtilities;
using UnityEngine;
using UnityEngine.Events;

public class Grapple : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform grappleTip;
    [SerializeField] private LayerMask grappleLayers;
    [SerializeField] private LineRenderer grappleLr;
    [SerializeField] private GameObject grappleHook;

    [Header("Grappling")]
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float grappleDelayTime;
    [SerializeField] private float overshootYAxis;

    [SerializeField] private bool clampMagnitude;

    [DrawIf(nameof(clampMagnitude), true)]
    [SerializeField] private float maxMagnitude = 50;


    [Header("Cooldown")]
    [SerializeField] private float grappleCd;
    [Header("Input")]
    [SerializeField] private KeyCode grappleKey = KeyCode.Mouse1;

    [SerializeField] private UnityEvent OnFired;
    [SerializeField] private UnityEvent OnConnected;
    [SerializeField] private UnityEvent OnMissed;

    private FPSMovementRB playerController;
    private Rigidbody rb;
    private Transform cam;
    private Vector3 grapplePoint;
    private float grappleCdTimer;
    private bool grappling;
    private Vector3 defaultHookPosition;

    private void Awake()
    {
        defaultHookPosition = grappleHook.transform.localPosition;
        if(Camera.main == null)
        {
            Debug.LogError("Player camera should be set to the `MainCamera` tag.");
            return;
        }

        cam = Camera.main.transform;
        playerController = GetComponentInParent<FPSMovementRB>();
        playerController.onCollision += StopGrapple;

        rb = GetComponentInParent<Rigidbody>();

        if (!clampMagnitude)
        {
            maxMagnitude = -1f;
        }

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
            grappleHook.transform.position = grappleLr.GetPosition(grappleLr.positionCount - 1);
        }
    }

    private void StartGrapple()
    {
        if (grappleCdTimer > 0)
            return;

        OnFired.Invoke();
        grappling = true;

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxGrappleDistance, grappleLayers))
        {
            playerController.Freeze = true;
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            OnMissed.Invoke();
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        grappleLr.enabled = true;
        grappleLr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        playerController.Freeze = false;
        OnConnected.Invoke();
        float grapplePointRelativeYPos = grapplePoint.y - transform.position.y - 1f;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        Vector3 velocity = CalculateJumpVelocity(transform.position, grapplePoint, highestPointOnArc);
        playerController.DelaySetVelocity(velocity);

        Invoke(nameof(StopGrapple), 1f);
    }

    private Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravityForce = Mathf.Abs(Physics.gravity.y);
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = (endPoint - startPoint).NewY(0);

        // will pull us down if we grapple below us
        float dir = Mathf.Sign(trajectoryHeight);
        Vector3 velocityY = dir * Mathf.Sqrt(Mathf.Abs(2 * gravityForce * trajectoryHeight)) * Vector3.up;

        float heightGravCalc = Mathf.Sqrt(Mathf.Abs(2 * trajectoryHeight / gravityForce));
        float displacementHeightCalc = Mathf.Sqrt(Mathf.Abs(2 * displacementY - trajectoryHeight)) / gravityForce;

        Vector3 velocityXZ = displacementXZ / (heightGravCalc + displacementHeightCalc);

        Vector3 finalVelocity = velocityXZ + velocityY;

        if (maxMagnitude > 0f && finalVelocity.magnitude > maxMagnitude)
        {
            finalVelocity = finalVelocity.normalized * maxMagnitude;
        }

        return finalVelocity;
    }



    public void StopGrapple()
    {
        playerController.Freeze = false;
        grappling = false;
        grappleCdTimer = grappleCd;

        grappleLr.enabled = false;
        grappleHook.transform.localPosition = defaultHookPosition;

    }
}
