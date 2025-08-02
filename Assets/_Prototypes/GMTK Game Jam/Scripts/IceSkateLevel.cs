using UnityEngine;

public class IceSkateLevel : MonoBehaviour
{
    [SerializeField] private Transform babyRoot;
    [SerializeField] private Transform sealRoot;

    public bool LevelComplete;
    public bool LevelFailed;

    public int BabyCount => babyRoot.childCount;
    public int SealCount => sealRoot.childCount;
}
