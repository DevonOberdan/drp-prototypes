using FinishOne.GeneralUtilities;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private List<GameObject> levels;

    private int currentLevel;

    private float playerStartY;


    public int CurrentLevel 
    {
        get => currentLevel;
        set 
        {
            for(int i = transform.childCount-1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            currentLevel = value;

            GameObject level = Instantiate(levels[currentLevel], transform);

            player.position = level.transform.GetChild(level.transform.childCount-1).position;
            player.position = player.position.NewY(playerStartY);
        }
    }

    private void Awake()
    {
        playerStartY = player.position.y;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
