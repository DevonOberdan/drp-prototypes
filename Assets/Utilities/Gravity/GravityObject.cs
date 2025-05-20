using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityObject : MonoBehaviour
{
    public enum CommonSources { Planet, Player };
    [SerializeField] CommonSources defaultSource = CommonSources.Planet;

    [SerializeField] GravitySource gSource;
    [SerializeField] bool keepStanding = true;

    [SerializeField] bool gravityFlipped;
    [SerializeField] bool capForce;
    [SerializeField] float rotationSpeed = 5f;

    Vector3 gravityDirection;

    public Vector3 GravityDirection
    {
        get => gravityDirection;
        set 
        {
            gravityDirection = value;
        }
    }

    [field: SerializeField] public float GravityFactor { get; set; } = 1;
    public int GravityDir { get; set; }

    public bool GravityFlipped
    {
        get => gravityFlipped;
        set
        {
            gravityFlipped = value;
            GravityDir = gravityFlipped ? -1 : 1;
        }
    }

    public bool OverrideDirection { get; set; }

    //public void SetGrabSourceDir(bool stand) => grabSourceDir = stand;
    public void SetStanding(bool stand) => keepStanding = stand;

    public GravitySource Source => gSource;

    public void SetSource(GravitySource source) => gSource = source;

    void Awake()
    {
        if (gSource == null)
            gSource = GameObject.FindGameObjectWithTag(Enum.GetName(typeof(CommonSources), defaultSource)).GetComponentInChildren<GravitySource>();

        gravityDirection = Vector3.zero;
        GravityFlipped = gravityFlipped;
      //  OverrideDirection = false;
    }

    public Vector3 FinalGravityDirection => gSource.BodyGravityDirection(transform, GravityFlipped);
    public Quaternion CurrentGravityRotation => Quaternion.FromToRotation(-transform.up, gSource.BodyGravityDirection(transform, GravityFlipped)) * transform.rotation;

    private void FixedUpdate()
    {
        Vector3 sourceDir = gSource.Attract(transform, GravityFlipped, GravityFactor);

        if (!OverrideDirection)
        {
            GravityDirection = sourceDir;
        }

        if (keepStanding)
        {
            //Quaternion desiredRotation = Quaternion.FromToRotation(-transform.up, GravityDirection) * transform.rotation;
            //transform.rotation = desiredRotation;

            Quaternion desiredRotation = Quaternion.FromToRotation(-transform.up, sourceDir) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
