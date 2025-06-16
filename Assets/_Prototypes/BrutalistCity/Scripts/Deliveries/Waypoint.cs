using System;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private Action<Transform> collisionHandler;

    public void Init(Action<Transform> callback)
    {
        collisionHandler = callback;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collisionHandler?.Invoke(transform);
        }
    }
}
