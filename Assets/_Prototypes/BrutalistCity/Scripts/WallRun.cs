using FinishOne.GeneralUtilities;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Wallrunning")]
    [SerializeField] private LayerMask whatIsWall;

    [SerializeField] private float wallRunForce;
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;
    [SerializeField] private float maxWallRunTime;

    [Header("Input")]
    [SerializeField] private KeyCode wallJumpKey = KeyCode.Space;

    [Header("Detection")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float minJumpHeight;

    [Header("Exiting")]
    [SerializeField] private float exitWallTime;

    private FPSMovementRB playerMovement;
    private Rigidbody rb;
    private Camera cam;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;
    private float wallRunTimer;

    private bool wallRunning;

    private Transform Player => playerMovement.transform;

    private void Start()
    {
        playerMovement = GetComponentInParent<FPSMovementRB>();
        rb = GetComponentInParent<Rigidbody>();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (wallRunning)
        {
            WallRunMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(Player.position, cam.transform.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(Player.position, -cam.transform.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private void StateMachine()
    {
        //conditions for wall-running
        if((wallLeft || wallRight) && playerMovement.MovementInputDir.z > 0 && !playerMovement.IsGrounded)
        {
            if (!wallRunning)
            {
                StartWallRun();
            }
            else if(wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }

            if (Input.GetKeyDown(wallJumpKey))
            {
                WallJump();
            }

            if (wallRunTimer <= 0 && wallRunning)
            {
                StopWallRun();
            }
        }
        else if (wallRunning)
        {
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        wallRunning = true;
        wallRunTimer = maxWallRunTime;
    }

    private void WallRunMovement()
    {
        rb.useGravity = false;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallRunDir = Vector3.Cross(wallNormal, transform.up);

        //flip if player approaching in opposite direction
        wallRunDir *= Mathf.Sign(Vector3.Dot(cam.transform.forward, wallRunDir));

        rb.AddForce(wallRunDir * wallRunForce, ForceMode.Force);

        bool pushingFromRight = wallRight && playerMovement.MovementInputDir.x < 0;
        bool pushingFromLeft = wallLeft && playerMovement.MovementInputDir.x > 0;

        if ((pushingFromRight && !pushingFromLeft) || (pushingFromLeft && !pushingFromRight))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }
    }

    private void StopWallRun()
    {
        wallRunning = false;
    }

    private void WallJump()
    {
        Vector3 wallNormal = wallRight ? rightWallHit.normal: leftWallHit.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        rb.linearVelocity = rb.linearVelocity.WithNew(y: 0f);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
