using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour, IPausable
{
    public UnityEvent OnCountdownComplete;

    [SerializeField] private float startSeconds;
    [SerializeField] private bool startTimerOnStart;

    private float countdownTimer;

    private bool paused;

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
        if (paused)
            return;

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

    public void Pause() => SetPause(true);
    public void Unpause() => SetPause(false);
    public void SetPause(bool pause) => paused = pause;
}
