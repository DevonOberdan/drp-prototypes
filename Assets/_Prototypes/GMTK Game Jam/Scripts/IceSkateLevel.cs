using UnityEngine;

public class IceSkateLevel : MonoBehaviour
{
    [SerializeField] private Transform babyRoot;
    [SerializeField] private Transform sealRoot;


    public int BabyCount => babyRoot.childCount;
    public int SealCount => sealRoot.childCount;
}
