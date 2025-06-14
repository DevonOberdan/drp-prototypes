using FinishOne.GeneralUtilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

    [SerializeField] private InteractionBuffer detectionBuffer;
    [SerializeField] private InteractionBuffer chargeBuffer;

    [SerializeField] private UnityEvent<bool> OnDetected;

    private Quaternion startRotation;
    private RaycastHit[] playerHit;
    private RotateObject rotateObj;

    private (Quaternion Rotation, float Angle) returnPoint;

    private float accumulatedAngle;
    private bool currentlyVisible;
    private bool returning;

    private float defaultRange;

    private static OverrideFlagHandler DetectionNetwork = new();
    private int detectorIdx;

    private Vector3 TargetDir => TargetLocation.Value - transform.position;

    private bool Alerted 
    {
        get => rotateObj.enabled == false;
        set 
        {
            rotateObj.enabled = !value;
            OnDetected.Invoke(Alerted);
            alertSound.mute = !Alerted;
        }
    }

    private void Awake()
    {
        detectorIdx = DetectionNetwork.AddFlag();

        playerHit = new RaycastHit[1];
        rotateObj = GetComponentInParent<RotateObject>();

        startRotation = rotateObj.transform.rotation;

        defaultRange = detectRange;

        detectionBuffer.CooldownAndReset = false;
        detectionBuffer.OnComplete.AddListener(BeginLockOn);

        chargeBuffer.OnComplete.AddListener(() => detectRange = defaultRange * 10);
        chargeBuffer.OnReset.AddListener(() => detectRange = defaultRange);

        Alerted = false;

    }

    private void BeginLockOn()
    {
        if(!Alerted)
            Alerted = true;
    }

    void Update()
    {
        currentlyVisible = InRange() && InViewingAngle() && HasLineOfSight();

        //currently patrolling && just saw player
        if (!Alerted)
        {
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

                float dot = Quaternion.Dot(transform.rotation, returnPoint.Rotation);
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

    private (Quaternion, float) FindReturnRotation()
    {
        Quaternion bestRotation = Quaternion.identity;
        float bestAngle = accumulatedAngle;
        float smallestDifference = float.MaxValue;

        for (float testAngle = accumulatedAngle - 180f; testAngle <= accumulatedAngle + 180f; testAngle += 1f)
        {
            Quaternion testRotation = startRotation * Quaternion.AngleAxis(testAngle, rotateObj.Vector.normalized);
            float angleDiff = Quaternion.Angle(rotateObj.transform.rotation, testRotation);

            if (angleDiff < smallestDifference)
            {
                smallestDifference = angleDiff;
                bestAngle = testAngle;
                bestRotation = testRotation;
            }
        }

        return (bestRotation, bestAngle);
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

    private void OnDestroy()
    {
        Debug.Log($"{gameObject.name} - {detectorIdx}", gameObject);
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

        if (!Alerted)
        {
            rotateObj.enabled = !pause;
        }
    }
}
