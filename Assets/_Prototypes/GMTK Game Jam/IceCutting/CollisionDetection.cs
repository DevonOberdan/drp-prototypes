using UnityEngine;
using UnityEngine.Events;

public class CollisionDetection : MonoBehaviour
{
    public UnityEvent OnCollisionDetected;

    [SerializeField] private string collisionTag;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag(collisionTag))
        {
            OnCollisionDetected.Invoke();
        }
    }
}
