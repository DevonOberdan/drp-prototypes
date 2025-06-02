using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Wallrunning")]
    [SerializeField] private LayerMask whatIsWall;

    [SerializeField] private float wallRunForce;
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;
    [SerializeField] private float maxWallRunTime;
    [Range(0f, 1f)]
    [SerializeField] private float verticalVelocityDampening = 0.25f;

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

    private readonly float CooldownMax = 1.0f;
    private float cooldownTimer;

    private Transform Player => playerMovement.transform;

    private int flagIndex;

    private void Start()
    {
        playerMovement = GetComponentInParent<FPSMovementRB>();
        rb = GetComponentInParent<Rigidbody>();
        cam = Camera.main;

        playerMovement.onJumped += WallJump;

        flagIndex = playerMovement.JumpOverrideHandler.AddFlag();
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
        else if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Clamp01(cooldownTimer);
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(Player.position, cam.transform.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(Player.position, -cam.transform.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private void StateMachine()
    {
        playerMovement.JumpOverrideHandler.SetFlag(flagIndex, wallLeft || wallRight);

        if (!playerMovement.IsGrounded && (wallLeft || wallRight))
        {
            playerMovement.ResetJumps();
        }

        //conditions for wall-running
        if ((wallLeft || wallRight) && playerMovement.MovementInputDir.z > 0 && !playerMovement.IsGrounded)
        {
            if (!wallRunning)
            {
                if(cooldownTimer <= 0f)
                {
                    StartWallRun();
                }
                else
                {
                    return;
                }
            }
            
            if (wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
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
        playerMovement.InFreeMovement = false;
        wallRunning = true;
        wallRunTimer = maxWallRunTime;
    }

    private void WallRunMovement()
    {
        rb.useGravity = false;

        rb.linearVelocity = Vector3.Scale(rb.linearVelocity, new(1, verticalVelocityDampening, 1));

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
        playerMovement.InFreeMovement = true;
        wallRunning = false;
        cooldownTimer = CooldownMax;
    }

    private void WallJump()
    {
        if (!wallRunning)
        {
            return;
        }

        wallRunning = false;

        Vector3 wallNormal = wallRight ? rightWallHit.normal: leftWallHit.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
