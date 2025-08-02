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

    [SerializeField] private Transform player;
    [SerializeField] private List<IceSkateLevel> levels;
    [SerializeField] private TMP_Text holeCountText;

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

    private void LateUpdate()
    {
        if (!currentLevel.LevelFailed && !currentLevel.LevelComplete && currentLevel.SealCount == 0)
        {
            currentLevel.LevelComplete = true;
            OnLevelComplete.Invoke();
        }
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
