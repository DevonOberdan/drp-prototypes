using UnityEngine;

public class IceSkateLevel : MonoBehaviour
{
    [SerializeField] private Transform babyRoot;
    [SerializeField] private Transform sealRoot;

    [field: SerializeField] public int MaxHoleCount { get; private set; } = 2;

    public bool LevelComplete;
    public bool LevelFailed;

    public int BabyCount => babyRoot.childCount;
    public int SealCount => sealRoot.childCount;
}
