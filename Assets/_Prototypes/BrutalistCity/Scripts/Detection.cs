using FinishOne.GeneralUtilities;
using UnityEngine;
using UnityEngine.Events;

public class Detection : MonoBehaviour, IPausable
{
    [SerializeField] private Vector3Atom TargetLocation;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private AudioSource alertSound;

    [Header("Configuration")]
    [SerializeField] private float detectRange = 50.0f;
    [SerializeField] private float detectAngle = 45f;

    [SerializeField] private float lockOnSpeed = 30f;
    [SerializeField] private float returnSpeed = 1.5f;

    [Range(0f, 1f)]
    [SerializeField] private float speedFactorOnVisible = .5f;

    [SerializeField] private InteractionBuffer detectionBuffer;
    [SerializeField] private InteractionBuffer chargeBuffer;

    [SerializeField] private UnityEvent<bool> OnDetected;
    [SerializeField] private UnityEvent OnAlerted;
    [SerializeField] private UnityEvent OnUnalerted;

    private RotateObject rotateObj;

    private (Quaternion Rotation, float Angle) returnPoint;
    private Vector3 startRotationAxis;
    private Quaternion startRotation;
    private LayerMask lineOfSightMask;

    private float accumulatedAngle;
    private float defaultRange;

    private bool currentlyVisible;
    private bool returning;

    private static OverrideFlagHandler DetectionNetwork = new();
    private int detectorIdx;

    private bool wasAlerted;

    private Vector3 TargetDir => TargetLocation.Value - transform.position;

    private bool Alerted 
    {
        get => rotateObj.enabled == false;
        set 
        {
            rotateObj.enabled = !value;
            OnDetected.Invoke(Alerted);

            if (Alerted)
            {
                OnAlerted.Invoke();
            }
            else
            {
                OnUnalerted.Invoke();
                rotateObj.SetDampenFactor(1, 0.5f);
            }
        }
    }

    private void Awake()
    {
        detectorIdx = DetectionNetwork.AddFlag();

        rotateObj = GetComponentInParent<RotateObject>();
        startRotation = rotateObj.transform.rotation;

        defaultRange = detectRange;

        detectionBuffer.CooldownAndReset = false;
        detectionBuffer.OnComplete.AddListener(BeginLockOn);

        chargeBuffer.OnComplete.AddListener(ChargeComplete);
        chargeBuffer.OnReset.AddListener(ChargeReset);

        lineOfSightMask = Physics.AllLayers;
        Alerted = false;
    }

    private void Start()
    {
        startRotation = rotateObj.transform.rotation;
        startRotationAxis = rotateObj.transform.up;
    }

    private void Update()
    {
        currentlyVisible = InRange() && InViewingAngle() && HasLineOfSight();

        //currently patrolling && just saw player
        if (!Alerted)
        {
            ProcessSpeedByVisibility();

            detectionBuffer.Interacting = currentlyVisible;
            accumulatedAngle += rotateObj.RotationSpeed * rotateObj.DampenFactor * Time.deltaTime;

            if (DetectionNetwork.AnyFlags)
            {
                Alerted = true;
            }
        }
        else
        {
            DetectionNetwork.SetFlag(detectorIdx, currentlyVisible);

            // keep all other Detectors on alert
            if (!currentlyVisible && DetectionNetwork.AnyFlags)
            {
                detectionBuffer.Complete();
                chargeBuffer.Interacting = false;
                FocusOnTarget();
                return;
            }

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
                FocusOnTarget();
            }
            else if(detectionBuffer.Percentage == 0)
            {
                if (!returning)
                {
                    returnPoint = FindReturnRotation();
                }

                returning = true;

                float dot = Quaternion.Dot(rotateObj.transform.rotation, returnPoint.Rotation);
                rotateObj.transform.rotation = Quaternion.Slerp(rotateObj.transform.rotation, returnPoint.Rotation, returnSpeed * Mathf.Abs(dot) * Time.deltaTime);

                if(Mathf.Abs(dot) >= 0.999999)
                {
                    rotateObj.transform.rotation = returnPoint.Rotation;
                    accumulatedAngle = returnPoint.Angle;
                    ReturnToPatrol();
                }
            }
        }
    }

    private void ProcessSpeedByVisibility()
    {
        if (!detectionBuffer.Interacting && currentlyVisible)
        {
            rotateObj.SetDampenFactor(speedFactorOnVisible, 0.5f);
        }
        else if (detectionBuffer.Interacting && !currentlyVisible)
        {
            rotateObj.SetDampenFactor(1, 0.5f);
        }
    }

    private void BeginLockOn()
    {
        if (!Alerted)
        {
            alertSound.Play();
            Alerted = true;
        }
    }

    private void ChargeComplete()
    {
        detectRange = defaultRange * 10;
        lineOfSightMask = targetLayer;
    }

    private void ChargeReset()
    {
        detectRange = defaultRange;
        lineOfSightMask = Physics.AllLayers;
    }

    private void FocusOnTarget()
    {
        returning = false;
        Quaternion newRot = Quaternion.LookRotation(TargetDir, Vector3.up);
        rotateObj.transform.rotation = Quaternion.Slerp(rotateObj.transform.rotation, newRot, lockOnSpeed * Time.deltaTime);
    }

    private void ReturnToPatrol()
    {
        Alerted = false;
        DetectionNetwork.SetFlag(detectorIdx, false);
    }
    
    private bool InRange() => Vector3.Distance(transform.position, TargetLocation.Value) < detectRange;
    private bool InViewingAngle() => Vector3.Dot(TargetDir.normalized, transform.forward) > Mathf.Cos(detectAngle * Mathf.Deg2Rad);

    private bool HasLineOfSight()
    {
        return Physics.Raycast(transform.position, TargetDir, out RaycastHit hitInfo, detectRange, lineOfSightMask)
               && targetLayer.Contains(hitInfo.collider.gameObject.layer);
    }

    private (Quaternion, float) FindReturnRotation()
    {
        Quaternion bestRotation = Quaternion.identity;
        float bestAngle = accumulatedAngle;
        float smallestDifference = float.MaxValue;

        for (float testAngle = accumulatedAngle - 180f; testAngle <= accumulatedAngle + 180f; testAngle += 1f)
        {
            Quaternion testRotation = startRotation * Quaternion.AngleAxis(testAngle, startRotationAxis);
            float angleDiff = Quaternion.Angle(rotateObj.transform.rotation, testRotation);

            if (angleDiff < smallestDifference)
            {
                smallestDifference = angleDiff;
                bestAngle = testAngle;
                bestRotation = testRotation;
            }
        }

        if(rotateObj.TryGetComponent(out RotationClamp clamp))
        {
            bestRotation.eulerAngles = clamp.ClampRotation(bestRotation.eulerAngles);
        }

        return (bestRotation, bestAngle);
    }

    private void OnDrawGizmosSelected()
    {
        if (TargetLocation == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, TargetDir);
        Gizmos.DrawRay(transform.position, transform.forward*detectRange);
    }

    private void OnDestroy()
    {
        DetectionNetwork.RemoveFlag(detectorIdx);
    }

    public void Pause()
    {
        SetPause(true);
    }

    public void Unpause()
    {
        SetPause(false);
    }

    public void SetPause(bool pause)
    {
        this.enabled = !pause;
        detectionBuffer.enabled = !pause;
        chargeBuffer.enabled = !pause;

        // ensure RotateObject script set back properly between pause states
        if (pause)
        {
            wasAlerted = Alerted;
            rotateObj.enabled = false;
        }
        else
        {
            if (!wasAlerted)
            {
                rotateObj.enabled = true;
            }

            wasAlerted = false;
        }
    }
}
