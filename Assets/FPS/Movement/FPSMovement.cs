using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class FPSMovement : MonoBehaviour
{
    public abstract bool IsGrounded { get; }
    public abstract bool IsInAir { get; }
    public abstract bool IsFalling { get; }
    public abstract bool IsIdle { get; }
    public abstract bool IsSprinting { get; }
    public abstract bool IsRunning { get; }
}
