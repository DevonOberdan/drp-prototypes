using Mono.Cecil.Cil;
using UnityEngine;

public class WallRun : MonoBehaviour
{

    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode wallJumpKey = KeyCode.Space;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJuimpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform orientation;
    private FPSMovementRB FPSMovementRB;
    private Rigidbody PlayerRB;

    [Header("Exiting")]
    private bool exitWall;
    public float exitWallTime;
    private float exitwallTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        FPSMovementRB  = GetComponent<FPSMovementRB>();
        PlayerRB = GetComponent<Rigidbody>();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJuimpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitWall)
        {
            if (!FPSMovementRB.wallRunning)
            {
                StartWallRun();
            }
            if(wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }
            if (wallLeft || wallRight) 
            {
                exitwallTimer = exitWallTime;
            }
            if (wallRunTimer <= 0 && FPSMovementRB.wallRunning)
            {
                exitWall = true;
                exitwallTimer = exitWallTime;
                StopWallRun();
            }
            if(Input.GetKeyDown(wallJumpKey))
            {
                WallJump();
            }
        }
        else if (exitWall)
        {
            if (FPSMovementRB.wallRunning)
            {
                StopWallRun();
            }
        }
        else
        {
            if (FPSMovementRB.wallRunning)
            {
                StopWallRun();
            }
            if (exitwallTimer > 0)
            {
                exitwallTimer -= Time.deltaTime;
            }
            if (exitwallTimer <= 0)
            {
                exitWall = false;
            }
        }
    }

    private void StartWallRun()
    {
        FPSMovementRB.wallRunning = true;

        wallRunTimer = maxWallRunTime;
    }

    private void WallRunMovement()
    {
        PlayerRB.useGravity = false;
        PlayerRB.linearVelocity = new Vector3(PlayerRB.linearVelocity.x, 0f, PlayerRB.linearVelocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        PlayerRB.AddForce(wallForward * wallRunForce, ForceMode.Force);
        if(!(wallLeft && horizontalInput > 0) &&  (wallRight && horizontalInput < 0))
            PlayerRB.AddForce(-wallNormal * 100, ForceMode.Force); 
    }

    private void StopWallRun()
    {
            FPSMovementRB.wallRunning = false;
    }

    private void WallJump()
    {
        exitWall = true;
        exitwallTimer = exitWallTime;
        Vector3 wallNormal = wallRight ? rightWallhit.normal: leftWallhit.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        PlayerRB.linearVelocity = new Vector3(PlayerRB.linearVelocity.x, 0f, PlayerRB.linearVelocity.z);
        PlayerRB.AddForce(forceToApply, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (FPSMovementRB.wallRunning)
        {
            WallRunMovement();
        }
    }
}
