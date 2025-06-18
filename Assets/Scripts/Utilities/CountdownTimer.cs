using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    public UnityEvent OnCountdownComplete;

    [SerializeField] private float startSeconds;
    [SerializeField] private bool startTimerOnStart;

    private float countdownTimer;

    void Start()
    {
        countdownTimer = startSeconds;

        if (!startTimerOnStart)
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        countdownTimer -= Time.deltaTime;
        
        if (countdownTimer < 0)
        {
            OnCountdownComplete.Invoke();
            this.enabled = false;
        }
    }

    public void StartTimer()
    {
        countdownTimer = startSeconds;
        this.enabled = true;
    }
}
