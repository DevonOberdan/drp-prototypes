using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PausableRigidbody : MonoBehaviour, IPausable
{
    Rigidbody rb;

    Vector3 cachedLinearVelocity;
    Vector3 cachedAngularVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetPause(bool pause)
    {
        if (pause)
            Pause();
        else
            Unpause();
    }

    public void Pause()
    {
        cachedLinearVelocity = rb.linearVelocity;
        cachedAngularVelocity = rb.angularVelocity;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Unpause()
    {
        rb.linearVelocity = cachedLinearVelocity;
        rb.angularVelocity = cachedAngularVelocity;

        cachedLinearVelocity = Vector3.zero;
        cachedAngularVelocity = Vector3.zero;
    }
}
