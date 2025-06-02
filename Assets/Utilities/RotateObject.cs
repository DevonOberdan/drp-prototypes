using UnityEngine;
using DG.Tweening;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector;
    
    [field: SerializeField] public float RotationSpeed { get; set; }
    public float DampenFactor { get; set; } = 1;

    private Tweener sloMoTween;

    void Update()
    {
        transform.Rotate(DampenFactor * RotationSpeed * Time.deltaTime * rotationVector);
    }

    public void SetDirection(bool positive)
    {
        rotationVector *= positive ? 1 : -1;
    }

    public void SetDampenFactor(float newDampen, float shiftTime)
    {
        if (sloMoTween.IsActive())
            sloMoTween.Kill();

        sloMoTween = DOTween.To(() => DampenFactor, x => DampenFactor = x, newDampen, shiftTime);
    }
}