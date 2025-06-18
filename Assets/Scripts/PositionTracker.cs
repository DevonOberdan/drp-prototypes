using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    [SerializeField] Vector3Atom positionData;

    private void Update()
    {
        positionData.Value = transform.position;
    }
}
