using FinishOne.GeneralUtilities;
using UnityEngine;

public enum FireType
{
    SINGLE,
    BURST,
    AUTOMATIC,
    BEAM
}

[CreateAssetMenu(fileName = "GunConfig", menuName = "Gun Config", order = 0)]
public class GunConfig : ScriptableObject
{
    public FireType fireType;

    [SerializeField] GameObject projectile;

    [field: SerializeField] public bool RequiresAmmo { get; private set; } = true;

    [SerializeField] float timeBetweenShots;
    [SerializeField] float bulletSpeed = 3000;

    //[Range(0, 1)]
    //[SerializeField] float shakeAmount = 0.15f;
    //bool IsSingle = fireType == FireType.SINGLE;
   // [DrawIf(nameof(IsSingle), false)]
    [SerializeField] int rpm = 600;

    [DrawIf(nameof(fireType), FireType.BURST)]
    [Range(2,10)]
    [SerializeField] int burstCount = 3;

    [SerializeField] bool chargeUp;

    [DrawIf(nameof(chargeUp), true)]
    [SerializeField] float chargeTime;

    [DrawIf(nameof(chargeUp), true)]
    [SerializeField] bool chargeSingleShot;

    [DrawIf(nameof(chargeUp), true)]
    [SerializeField] bool instantRelease;

    #region Public Properties

    public GameObject Projectile => projectile;

    public float TimeBetweenShots => timeBetweenShots;
    public float BulletSpeed => bulletSpeed;
    //public float ShakeAmount => shakeAmount;
    public float TimeBetweenRounds => 1f / (rpm/60f);
    public int BurstCount => burstCount;

    public bool ChargeUp => chargeUp;
    public float ChargeTime => chargeTime;
    public bool ChargeSingleShot => chargeSingleShot;
    public bool InstantRelease => instantRelease;

    public bool CanHoldFire => fireType == FireType.AUTOMATIC || fireType == FireType.BEAM;
    //public bool FireInput => CanHoldFire ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
    #endregion

    private void OnValidate()
    {
        if (CanHoldFire)
        {
            chargeSingleShot = false;
            instantRelease = true;
        }
    }
}