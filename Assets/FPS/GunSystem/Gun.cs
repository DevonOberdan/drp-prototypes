using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    [SerializeField] UnityEvent OnFired;
    [SerializeField] UnityEvent OnEmpty;

    [SerializeField] List<GunConfig> gunModes;

    [SerializeField] Transform barrelPoint;
    [SerializeField] private float relativeVelocityFactor = 5f;

    GunConfig data;
    Camera mainCamera;
    BeamEffect newBeam;

    delegate void FireRound();
    FireRound Shoot;

    KeyCode fireButton = KeyCode.Mouse0;

    float timeHeld;
    float timeSinceFired;

    bool midFire;
    bool charging, charged, chargeFired;

    public GunConfig Data
    {
        get => data;
        set
        {
            if (data == value)
                return;

            data = value;

            if (data.fireType == FireType.SINGLE)
                Shoot = ShootSingle;
            else if (data.fireType == FireType.BURST)
                Shoot = ShootBurst;
            else if (data.fireType == FireType.AUTOMATIC)
                Shoot = ShootAutomatic;
            else if (data.fireType == FireType.BEAM)
                Shoot = ShootBeam;
        }
    }

    public bool Restricted { get; set; }

    public bool FireInput => Data.CanHoldFire ? Input.GetKey(fireButton) : Input.GetKeyDown(fireButton);

    bool CanShoot => (!data.RequiresAmmo || !Restricted) &&
                     timeSinceFired > data.TimeBetweenShots &&
                     !midFire && (data.CanHoldFire || !chargeFired);

    bool IsChargeTriggered => data.InstantRelease || Input.GetKeyUp(fireButton);
    bool ShouldFireSingleChargeShot => data.ChargeSingleShot && Input.GetKeyUp(fireButton);

    private void Start()
    {
        mainCamera = Camera.main;
        Data = gunModes[0];
        fireButton = KeyCode.Mouse0;
    }

    private void GetGunMode()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Data = gunModes[0];
            fireButton = KeyCode.Mouse0;
        }
        else if (gunModes.Count > 1 && Input.GetKeyDown(KeyCode.Mouse1))
        {
            Data = gunModes[1];
            fireButton = KeyCode.Mouse1;
        }
    }

    private void Update()
    {
        timeSinceFired += Time.deltaTime;

        GetGunMode();

        if(Data.fireType == FireType.BEAM)
        {
            if (!HoldingFire() && newBeam != null)
            {
                newBeam.EndBeam();
                newBeam = null;
            }
        }

        if (data.ChargeUp)
        {
            if (CanShoot)
            {
                ProcessChargeTime();
            }
            else if(Restricted)
            {
                charged = false;
                chargeFired = false;
                charging = false;
                timeHeld = 0;
            }
        }
    }

    private void LateUpdate()
    {
        if (CanShoot)
        {
            if (data.ChargeUp)
            {
                if (data.CanHoldFire)
                    ProcessHoldCharge();
                else
                    ProcessNonHoldCharge();
            }
            else if (FireInput)
            {
                Shoot();
            }
        }
        else if (FireInput)
        {
            OnEmpty.Invoke();
        }

        // set charge states
        if (data.ChargeUp)
            ProcessChargeInput();
    }

    private void ProcessChargeTime()
    {
        if (charging && Input.GetKey(fireButton))
            timeHeld += Time.deltaTime;
        else
            timeHeld = 0f;

        if (timeHeld >= data.ChargeTime)
            charged = true;
    }

    private void ProcessChargeInput()
    {
        if (Input.GetKeyDown(fireButton))
        {
            charging = true;
            chargeFired = false;
        }
        else if (Input.GetKeyUp(fireButton))
        {
            if (chargeFired)
                charged = false;
            chargeFired = false;
            charging = false;
        }
    }

    private bool HoldingFire()
    {
        bool chargeBool = Data.ChargeUp ? charged : true;

        return CanShoot && chargeBool && FireInput;
    }

    private void ProcessHoldCharge()
    {
        if (charged)
        {
            Shoot();
            chargeFired = true;
            charging = false;
        }
    }

    private void ProcessNonHoldCharge()
    {
        if (charged)
        {
            if (IsChargeTriggered)
            {
                Shoot();
                chargeFired = data.InstantRelease;
                charged = false;
                charging = false;
            }
        }
        else if (ShouldFireSingleChargeShot)
        {
            Shoot();
        }
    }

    #region Shoot Mode Functions
    private void ShootSingle()
    {
        Vector3 parentVel = GetComponentInParent<FPSMovementRB>().CurrentVelocity;

        Projectile p = Instantiate(Data.Projectile, barrelPoint.position, Quaternion.identity).GetComponent<Projectile>();

        Vector3 shotDir = FindShotLine();

        float magInDirection = Vector3.Dot(parentVel, shotDir);

        p.Fire(FindShotLine(), data.BulletSpeed + (magInDirection* relativeVelocityFactor));

        OnFired.Invoke();
        timeSinceFired = 0;

        Vector3 FindShotLine()
        {
            Vector3 cursorPoint = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2, mainCamera.nearClipPlane));
            Ray rayFromCursor = new(cursorPoint, mainCamera.transform.forward);

            if (Physics.Raycast(rayFromCursor, out RaycastHit hit, 1000, ~LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore))
                return (hit.point - barrelPoint.position).normalized;
            else
                return barrelPoint.transform.forward;
        }
    }

    private void ShootBurst()
    {
        StartCoroutine(ProcessBurst());
    }

    private void ShootAutomatic()
    {
        if (timeSinceFired > data.TimeBetweenRounds)
            ShootSingle();
    }

    private void ShootBeam()
    {
        Vector3 rayCastStart = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2, mainCamera.nearClipPlane));

        if (newBeam == null)
        {
            newBeam = Instantiate(Data.Projectile, barrelPoint.transform.parent).GetComponent<BeamEffect>();
        }

        if (timeSinceFired > data.TimeBetweenRounds)
        {
            if (Physics.Raycast(rayCastStart, mainCamera.transform.forward, out RaycastHit hit, 10000, ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject.TryGetComponent(out IBeamable beamable))
                {
                    beamable.HitByBeam(hit);
                }
            }
            timeSinceFired = 0;
        }
    }

    private IEnumerator ProcessBurst()
    {
        midFire = true;
        ShootSingle();
        
        int i = 1;
        while(i<data.BurstCount && !Restricted)
        {
            yield return new WaitForSeconds(data.TimeBetweenRounds);
            ShootSingle();
            i++;
        }
        midFire = false;
    }
    #endregion
}
