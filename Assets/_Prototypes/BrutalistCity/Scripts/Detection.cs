using DG.Tweening;
using FinishOne.GeneralUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Detection : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask targetLayer;

    [Header("Configuration")]
    [SerializeField] private float detectRange = 50.0f;
    [SerializeField] private float detectAngle = 45f;

    [SerializeField] private float lockOnSpeed = 30f;
    [SerializeField] private float timeToLockOn = 0.5f;
    [SerializeField] private float timeToReturn = 1f;

    RaycastHit[] playerHit;
    private float angle;

    private InteractionBuffer visibilityBuffer;
    private RotateObject rotateObj;

    private bool wasVisible;
    bool currentlyVisible;
    private Vector3 rotationAtDetection;

    private Vector3 VecToPlayer => target.position - transform.position;

    private void Start()
    {
        playerHit = new RaycastHit[1];
        visibilityBuffer = GetComponent<InteractionBuffer>();
        rotateObj = GetComponent<RotateObject>();
    }

    void Update()
    {
        currentlyVisible = InRange() && InViewingAngle() && HasLineOfSight();
        visibilityBuffer.Interacting = currentlyVisible;

        //currently patrolling && just saw player
        if(rotateObj.enabled && !wasVisible && currentlyVisible)
        {
            rotationAtDetection = transform.eulerAngles;
            rotateObj.enabled = false;
            Debug.Log("Spotted!: " + rotationAtDetection);
        }

        if (currentlyVisible)
        {
            Quaternion newRot = Quaternion.LookRotation(VecToPlayer+Vector3.up*90, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, lockOnSpeed*Time.deltaTime);
        }
        else if(!rotateObj.enabled && visibilityBuffer.CurrentTime == 0)
        {
            transform.DORotate(rotationAtDetection, timeToReturn, RotateMode.Fast).OnComplete(ReturnToPatrol);
        }

        wasVisible = currentlyVisible;
    }


    private void ReturnToPatrol()
    {
        rotateObj.enabled = true;
    }
    
    private bool InRange()
    {
        return Vector3.Distance(transform.position, target.position) < detectRange;
    }

    private bool InViewingAngle()
    {
        Vector3 side1 = target.position - transform.position;
        Vector3 side2 = -transform.right;
        
        angle = Vector3.Angle(side1, side2);
        return angle < detectAngle;
    }

    private bool HasLineOfSight()
    {
        Ray ray = new(transform.position, target.transform.position-transform.position);
        return Physics.SphereCastNonAlloc(ray, 10f, playerHit, detectRange, targetLayer) > 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null)
        {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, target.transform.position - transform.position);
        Gizmos.DrawRay(transform.position, -transform.right*detectRange);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.up*detectRange);
    }
}
