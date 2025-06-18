using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] private UnityEvent<float> OnTimePassed;
    [SerializeField] private TMP_Text textField;

    [Range(0,3)]
    [SerializeField] private int msPlaces;
    [SerializeField] private bool displayHours;

    private StringBuilder builder;
    private float timePassed;

    public float TimePassed 
    {
        get => timePassed;
        set 
        {
            timePassed = value;
            textField.text = TimeSpan.FromSeconds(timePassed).ToString(builder.ToString());
        }
    }

    private void Awake()
    {
        builder = new StringBuilder();
    }

    void Update()
    {
        ConfigureFormat();
        TimePassed += Time.deltaTime;
    }

    public void StartTime() => SetTimerOn(true);
    public void StopTime() => SetTimerOn(false);

    public void SetTimerOff(bool turnOff) => SetTimerOn(!turnOff);

    public void SetTimerOn(bool turnOn)
    {
        this.enabled = turnOn;
    }

    public void SetShowTimer(bool show)
    {
        this.enabled = show;
        textField.enabled = show;
        textField.gameObject.SetActive(show);
    }

    public void ConfigureFormat()
    {
        builder.Clear();

        if (displayHours)
            builder.Append(@"hh\:");

        builder.Append(@"mm\:ss");

        if(msPlaces > 0)
            builder.Append(@"\:");

        for(int i = 0; i < msPlaces; i++)
        {
            builder.Append("f");
        }
    }
}
