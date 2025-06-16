using UnityEngine;
using UnityEngine.Events;

public class PlayerLaserImpact : MonoBehaviour, ILaserImpact
{
    [SerializeField] private UnityEvent OnPlayerDeath;

    private bool dead;

    public void Hit()
    {
        if (!dead)
        {
            OnPlayerDeath.Invoke();
            dead = true;
        }
    }
}
