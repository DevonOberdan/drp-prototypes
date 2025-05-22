using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 fireDir, float fireSpeed = 2000)
    {
        rb.AddForce(fireDir * fireSpeed);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    private IEnumerator OnCollisionEnter(Collision collision)
    {
        yield return new WaitForEndOfFrame();
        Kill();
    }
}
