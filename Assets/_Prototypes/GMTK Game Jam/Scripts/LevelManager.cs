using FinishOne.GeneralUtilities;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public UnityEvent OnLevelChanged;
    public UnityEvent OnLevelComplete;
    public UnityEvent OnLastLevelComplete;
    public UnityEvent OnMaxHolesReached;

    [SerializeField] private Transform player;
    [SerializeField] private List<IceSkateLevel> levels;
    
    
    [SerializeField] private TMP_Text holeCountText;
    [SerializeField] private Transform holeRoot;


    private IceSkateLevel currentLevel;
    private int currentLevelIndex;
    private float playerStartY;

    public int CurrentLevelIndex 
    {
        get => currentLevelIndex;
        set 
        {
            value = Mathf.Clamp(value, 0, levels.Count);

            for(int i = transform.childCount-1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            currentLevelIndex = value;

            currentLevel = Instantiate(levels[currentLevelIndex], transform);

            player.position = currentLevel.transform.GetChild(currentLevel.transform.childCount-1).position;
            player.position = player.position.NewY(playerStartY);

            OnLevelChanged.Invoke();
        }
    }

    private void Awake()
    {
        playerStartY = player.position.y;
    }

    private void Start()
    {
        CurrentLevelIndex = 0;
    }

    private bool checkingForSeals;

    private void LateUpdate()
    {
        int holesRemaining = currentLevel.MaxHoleCount - holeRoot.childCount;

        holeCountText.text = holesRemaining.ToString();

        if (!currentLevel.LevelFailed && !currentLevel.LevelComplete)
        {
            if(currentLevel.SealCount == 0)
            {
                currentLevel.LevelComplete = true;
                OnLevelComplete.Invoke();
            }
            else if (!checkingForSeals && !currentLevel.LevelFailed && holesRemaining <= 0)
            {
                checkingForSeals = true;
                Invoke(nameof(AllHolesCut), 0.1f);
            }
        }
    }

    private void AllHolesCut()
    {
        checkingForSeals = false;

        if (currentLevel.LevelComplete)
        {
            return;
        }

        currentLevel.LevelFailed = true;
        OnMaxHolesReached.Invoke();
    }

    public void SetLevelFailed()
    {
        currentLevel.LevelFailed = true;
    }

    public void NextLevel()
    {
        if(currentLevelIndex == levels.Count - 1)
        {
            OnLastLevelComplete.Invoke();
            return;
        } 

        CurrentLevelIndex++;
    }

    public void ResetCurrentLevel()
    {
        CurrentLevelIndex = currentLevelIndex;
    }
}
