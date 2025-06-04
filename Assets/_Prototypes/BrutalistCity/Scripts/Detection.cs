using DG.Tweening;
using FinishOne.GeneralUtilities;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask targetLayer;

    [Header("Configuration")]
    [SerializeField] private float detectRange = 50.0f;
    [SerializeField] private float detectAngle = 45f;

    [SerializeField] private float lockOnSpeed = 30f;
    [SerializeField] private float timeToReturn = 1f;

    [SerializeField] private InteractionBuffer detectionBuffer;

    private RaycastHit[] playerHit;
    private InteractionBuffer chargeBuffer;
    private RotateObject rotateObj;
    private Vector3 rotationAtDetection;
    private bool currentlyVisible;
    private float dot;
    private Vector3 TargetDir => target.position - transform.position;

    private bool Detected 
    {
        get => rotateObj.enabled == false;
        set => rotateObj.enabled = !value;
    }

    private void Awake()
    {
        playerHit = new RaycastHit[1];
        chargeBuffer = GetComponent<InteractionBuffer>();
        rotateObj = GetComponent<RotateObject>();

        detectionBuffer.CooldownAndReset = false;
        detectionBuffer.OnComplete.AddListener(BeginLockOn);
    }

    private void BeginLockOn()
    {
        if (!Detected)
        {
            rotationAtDetection = transform.eulerAngles;
            Detected = true;
        }
    }

    void Update()
    {
        currentlyVisible = InRange() && InViewingAngle() && HasLineOfSight();

        //currently patrolling && just saw player
        if(!Detected)
        {
            detectionBuffer.Interacting = currentlyVisible;
        }
        else
        {
            //been seen and fully detected, charge laser
            if(detectionBuffer.Percentage == 1)
            {
                chargeBuffer.Interacting = currentlyVisible;
            }

            if (chargeBuffer.Percentage == 0)
            {
                detectionBuffer.Interacting = currentlyVisible;

                if (currentlyVisible)
                    detectionBuffer.Complete();
            }

            if (currentlyVisible)
            {
                Quaternion newRot = Quaternion.LookRotation(TargetDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRot, lockOnSpeed * Time.deltaTime);
            }

            if (detectionBuffer.Percentage == 0)
            {
                transform.DORotate(rotationAtDetection, timeToReturn, RotateMode.Fast).SetEase(Ease.Linear).OnComplete(ReturnToPatrol);
            }
        }
    }

    private void ReturnToPatrol()
    {
        Detected = false;
    }
    
    private bool InRange()
    {
        return Vector3.Distance(transform.position, target.position) < detectRange;
    }

    private bool InViewingAngle()
    {
        dot = Vector3.Dot(TargetDir.normalized, transform.forward);
        return dot > Mathf.Cos(detectAngle * Mathf.Deg2Rad);
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
        Gizmos.DrawRay(transform.position, TargetDir);
        Gizmos.DrawRay(transform.position, transform.forward*detectRange);
    }
}
