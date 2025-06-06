using FinishOne.GeneralUtilities;
using UnityEngine;
using UnityEngine.Events;

public class Detection : MonoBehaviour
{
    [SerializeField] private Vector3Atom TargetLocation;
    [SerializeField] private LayerMask targetLayer;

    [Header("Configuration")]
    [SerializeField] private float detectRange = 50.0f;
    [SerializeField] private float detectAngle = 45f;

    [SerializeField] private float lockOnSpeed = 30f;
    [SerializeField] private float returnSpeed = 1.5f;

    [SerializeField] private InteractionBuffer detectionBuffer;
    [SerializeField] private InteractionBuffer chargeBuffer;

    [SerializeField] private UnityEvent<bool> OnDetected;

    private Quaternion startRotation;
    private RaycastHit[] playerHit;
    private RotateObject rotateObj;

    private PathPoint returnPoint;
    private float accumulatedAngle;
    private bool currentlyVisible;
    private bool returning;

    private float defaultRange;

    private Vector3 TargetDir => TargetLocation.Value - transform.position;

    private bool Detected 
    {
        get => rotateObj.enabled == false;
        set {
            rotateObj.enabled = !value;
            OnDetected.Invoke(Detected);
        }
    }

    private void Awake()
    {
        playerHit = new RaycastHit[1];
        rotateObj = GetComponent<RotateObject>();

        startRotation = transform.rotation;

        defaultRange = detectRange;

        detectionBuffer.CooldownAndReset = false;
        detectionBuffer.OnComplete.AddListener(BeginLockOn);

        chargeBuffer.OnComplete.AddListener(() => detectRange = defaultRange * 10);
        chargeBuffer.OnReset.AddListener(() => detectRange = defaultRange);

        Detected = false;
    }

    private void BeginLockOn()
    {
        if (!Detected)
        {
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
            accumulatedAngle += rotateObj.RotationSpeed * rotateObj.DampenFactor * Time.deltaTime;
        }
        else
        {
            //been seen and fully detected, charge laser
            if(detectionBuffer.Percentage == 1)
            {
                chargeBuffer.Interacting = currentlyVisible;
            }

            // was seen, but is now hidden and not at all charged up
            if (chargeBuffer.Percentage == 0)
            {
                detectionBuffer.Interacting = currentlyVisible;

                if (currentlyVisible)
                {
                    detectionBuffer.Complete();
                }
            }

            if (currentlyVisible)
            {
                returning = false;
                Quaternion newRot = Quaternion.LookRotation(TargetDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRot, lockOnSpeed * Time.deltaTime);
            }
            else if(detectionBuffer.Percentage == 0)
            {
                if (!returning)
                {
                    returnPoint = FindReturnRotation();
                }

                returning = true;

                float dot = Quaternion.Dot(transform.rotation, returnPoint.Rotation);
                transform.rotation = Quaternion.Slerp(transform.rotation, returnPoint.Rotation, returnSpeed * Mathf.Abs(dot) * Time.deltaTime);

                if(Mathf.Abs(dot) >= 0.999999)
                {
                    transform.rotation = returnPoint.Rotation;
                    accumulatedAngle = returnPoint.Angle;
                    ReturnToPatrol();
                }
            }
        }
    }

    private void ReturnToPatrol()
    {
        Detected = false;
    }
    
    private bool InRange()
    {
        return Vector3.Distance(transform.position, TargetLocation.Value) < detectRange;
    }

    private bool InViewingAngle()
    {
        return Vector3.Dot(TargetDir.normalized, transform.forward) > Mathf.Cos(detectAngle * Mathf.Deg2Rad);
    }

    private bool HasLineOfSight()
    {
        return Physics.SphereCastNonAlloc(new Ray(transform.position, TargetDir), 10f, playerHit, detectRange, targetLayer) > 0;
    }

    private struct PathPoint
    {
        public Quaternion Rotation;
        public float Angle;

        public PathPoint(Quaternion Rotation, float Angle)
        {
            this.Rotation = Rotation;
            this.Angle = Angle;
        }
    }

    private PathPoint FindReturnRotation()
    {
        Quaternion bestRotation = Quaternion.identity;
        float bestAngle = accumulatedAngle;
        float smallestDifference = float.MaxValue;

        for (float testAngle = accumulatedAngle - 180f; testAngle <= accumulatedAngle + 180f; testAngle += 1f)
        {
            Quaternion testRotation = startRotation * Quaternion.AngleAxis(testAngle, rotateObj.Vector.normalized);
            float angleDiff = Quaternion.Angle(transform.rotation, testRotation);

            if (angleDiff < smallestDifference)
            {
                smallestDifference = angleDiff;
                bestAngle = testAngle;
                bestRotation = testRotation;
            }
        }

        return new PathPoint(bestRotation, bestAngle);
    }

    private void OnDrawGizmosSelected()
    {
        if (TargetLocation == null)
        {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, TargetDir);
        Gizmos.DrawRay(transform.position, transform.forward*detectRange);
    }
}
