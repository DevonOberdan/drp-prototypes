using FinishOne.GeneralUtilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;

public static class InterfaceFinder
{
    public static T[] FindObjectsByType<T>() where T : class
    {
        return Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Select(m => m as T)
            .Where(m => m != null)
            .ToArray();
    }
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputSystemUIInputModule inputModule;

    private bool paused;

    public UnityEvent<bool> PauseStateBroadcast;
    public UnityEvent<bool> PausableBroadcast;

    public static bool StartInLevelSelect;

    public bool Paused 
    {
        get => paused;
        set 
        {
            if (!CanPause)
                return;

            paused = value;

            PauseStateBroadcast?.Invoke(paused);

            foreach (IPausable p in InterfaceFinder.FindObjectsByType<IPausable>())
            {
                if(paused)
                    p.Pause();
                else
                    p.Unpause();
            }
        }
    }

    public bool CanPause { get; private set; }

    [Header("Request Game Events")]
    [SerializeField] private GameEvent RequestTogglePause;
    [SerializeField] private BoolGameEvent RequestPauseState;
    [SerializeField] private BoolGameEvent RequestPreventInput;

    private void Awake()
    {
        CanPause = true;

        if(inputModule == null)
        {
            Debug.LogError("InputSystemUIInputModule must be assigned to inputModule field.");
            return;
        }

        RequestTogglePause.RegisterListener(new GameEventClassListener(TogglePause));
        RequestPauseState.RegisterListener(new GameEventClassListener<bool>(SetPaused));
        RequestPreventInput.RegisterListener(new GameEventClassListener<bool>(SetDisableInput));
    }

    public void Pause() => Paused = true;
    public void Play() => Paused = false;
    public void TogglePause() => Paused = !Paused;
    public void SetPaused(bool value) => Paused = value;

    public void SetPausable(bool pausable)
    {
        CanPause = pausable;
        PausableBroadcast.Invoke(pausable);
    }

    public void SetDisableInput(bool disable)
    {
        if(inputModule != null)
        {
            inputModule.enabled = !disable;
        }
    }
}