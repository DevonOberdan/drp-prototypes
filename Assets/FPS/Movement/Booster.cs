using FinishOne.GeneralUtilities;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Booster : MonoBehaviour
{
    public enum BoosterMovement { VERTICAL_ONLY, LEAP}
    public BoosterMovement movement;

    public enum VerticalBoostMode { KILL_MOMENTUM, FORCE_STILL }
    public VerticalBoostMode verticalMode;

    [SerializeField] private InputReader inputReader;

    [SerializeField] UnityEvent OnBoost;

    [SerializeField] float boostFactor = 4f;
    [SerializeField] float boostMovingFactor = 0.5f;

    [SerializeField] bool canBoostInAir;

    [SerializeField] private bool canRepeat;

    [SerializeField] bool hasAfterburn;

    [DrawIf(nameof(hasAfterburn), true)]
    [SerializeField] float afterBurnerTime = 2.5f;
    [DrawIf(nameof(hasAfterburn), true)]
    [SerializeField] float afterburnFactor = 0.5f;



    FPSMovementRB playerController;
    GravityObject playerGravity;
    bool boosting, afterBurnersActive, afterBurnerUsed;

    public GravityObject PlayerGravity => playerGravity = playerGravity != null ? playerGravity : GetComponentInParent<GravityObject>();
    public FPSMovementRB PlayerController => playerController = playerController != null ? playerController : GetComponentInParent<FPSMovementRB>();

    bool AfterBurnerNeedsTurnedOn => !AfterburnersActive && boosting && PlayerController.IsFalling && !afterBurnerUsed;
    bool CanBoost => !Restricted && (!boosting || canRepeat)  && (canBoostInAir || PlayerController.IsGrounded);

    public bool Restricted { get; set; }

    bool AfterburnersActive
    {
        get => afterBurnersActive;
        set
        {
            afterBurnersActive = value;
            if (afterBurnersActive)
            {
                PlayerGravity.GravityFactor = afterburnFactor;
                PlayerController.OldAirSpeedQuotient = playerController.AirSpeedQuotient;
                PlayerController.AirSpeedQuotient = 1.5f;
            }
            else
            {
                PlayerGravity.GravityFactor = 1;
                PlayerController.AirSpeedQuotient = playerController.OldAirSpeedQuotient;
            }
        }
    }

    private void Start()
    {
        
    }

    void Update()
    {
        CheckBoost();
    }

    private void CheckBoost()
    {
        if (inputReader.IsAbilityOnePressed && CanBoost)
        {
            if (movement == BoosterMovement.VERTICAL_ONLY)
            {
                if (verticalMode == VerticalBoostMode.FORCE_STILL)
                {
                    if (playerController.IsIdle)
                        Boost(boostFactor);
                }
                else
                {
                    if (PlayerGravity.GravityDir == 1)
                        StartCoroutine(DampenMomentum());
                    else
                        Boost(boostFactor * boostMovingFactor);
                }
            }
            else
            {
                //inside
                if (PlayerGravity.GravityDir==1)
                {
                    Boost(boostFactor * boostMovingFactor);
                }
                else
                {
                    if(!PlayerController.IsIdle)
                        PlayerController.ForceForwardJump(PlayerController.JumpFactor * boostFactor * boostMovingFactor);
                    Boost(boostFactor * boostMovingFactor);
                }
            }
        }
        else if (boosting && PlayerController.IsGrounded)
        {
            boosting = false;
            if (hasAfterburn && (AfterburnersActive || afterBurnerUsed))
            {
                StopAllCoroutines();
                AfterburnersActive = false;
                afterBurnerUsed = false;
            }
        }
        else if (hasAfterburn && AfterBurnerNeedsTurnedOn)
        {
            AfterburnersActive = true;
            StartCoroutine(AfterburnTimer());
        }
    }

    IEnumerator DampenMomentum()
    {
        playerController.StallInput = true;
        Boost(boostFactor * boostMovingFactor);

        yield return new WaitForSeconds(.25f);
        playerController.StallInput = false;
    }

    [Range(0, 1)]
    [SerializeField] private float yVelocityDampening;

    void Boost(float factor)
    {
        Rigidbody rb = PlayerController.GetComponent<Rigidbody>();

        rb.linearVelocity = rb.linearVelocity.WithNew(y:rb.linearVelocity.y* yVelocityDampening);

        PlayerController.ForceJump(PlayerController.JumpFactor * factor, minimumVelocity:true);
        boosting = true;
        OnBoost.Invoke();
    }

    IEnumerator AfterburnTimer()
    {
        AfterburnersActive = true;

        float time = 0;
        while (time < afterBurnerTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        AfterburnersActive = false;
        afterBurnerUsed = true;
    }
}