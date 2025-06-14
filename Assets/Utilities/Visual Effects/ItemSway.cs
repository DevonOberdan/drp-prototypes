using FinishOne.GeneralUtilities;
using UnityEngine;

public class ItemSway : MonoBehaviour
{
    [SerializeField] Rigidbody playerRB;
    [SerializeField] private InputReader inputReader;

    [SerializeField] float mouseSwayForce = 1.5f;
    [SerializeField] float movementSwayForce = 1.5f;
    [SerializeField] float smoothVal = 10f;
    [SerializeField] float walkFactor = 10f;
    [SerializeField] float jumpRange = 10f;

    [SerializeField] bool tiltOnJump;

    [DrawIf(nameof(tiltOnJump), true)]
    [Range(0,10f)]
    [SerializeField] float jumpTiltVal = 10;

    Transform childTransform;
    Quaternion centerRotation, childCenterRotation;
    Quaternion newGoalRotation, newSwayRotation, newMovementRotation, newJumpRotation;

    void Start()
    {
        centerRotation = transform.localRotation;

        inputReader.EnablePlayerActions();

        GetChildReference();
    }

    /* May want to try independent smoothing values for mouse sway vs. jumping sway, etc.
     * 
     * Dont think i can because logically can only Slerp to one new rotation at a time
     */

    private void FixedUpdate()
    {
        HandleMouseSway();
        HandleMovementSway();

        newGoalRotation = centerRotation * newSwayRotation * newMovementRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, newGoalRotation, smoothVal * Time.deltaTime);

        if(childTransform != null)
        {
            childTransform.localRotation = Quaternion.Slerp(childTransform.localRotation, childCenterRotation * newJumpRotation, jumpTiltVal * Time.deltaTime);
        }
    }

    void HandleMouseSway()
    {
        Quaternion yRot = Quaternion.AngleAxis(-inputReader.LookVector.x * mouseSwayForce, Vector3.up);
        Quaternion xRot = Quaternion.AngleAxis(inputReader.LookVector.y * mouseSwayForce, Vector3.right);
        newSwayRotation = yRot * xRot;
    }

    void HandleMovementSway()
    {
        Vector3 localVelocity = playerRB.transform.InverseTransformDirection(playerRB.linearVelocity);
        newJumpRotation = Quaternion.AngleAxis(Mathf.Clamp(localVelocity.y,-jumpRange, jumpRange), Vector3.right);

        Quaternion yRot = Quaternion.AngleAxis(-inputReader.MoveDirection.x * movementSwayForce, Vector3.up);
        Quaternion xRot = Quaternion.AngleAxis(inputReader.MoveDirection.y * Mathf.Sin(walkFactor * Time.realtimeSinceStartup) * .5f, Vector3.right);

        newMovementRotation = yRot * xRot * newJumpRotation;
    }

    private void OnTransformChildrenChanged()
    {
        GetChildReference();
    }

    private void GetChildReference()
    {
        if(transform.childCount > 0)
        {
            childTransform = transform.GetChild(0);
            childCenterRotation = childTransform.localRotation;
        }
    }
}
