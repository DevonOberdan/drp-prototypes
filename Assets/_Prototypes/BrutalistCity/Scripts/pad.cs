using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class pad : MonoBehaviour
{
    public float force;
    private ParticleSystem steamVolume;
    public float particleTimer = 5f;
    public UnityEvent OnParticleStart;
    

    void Start()
    {
        steamVolume = GetComponentInChildren<ParticleSystem>();
        particleTimer = Mathf.Clamp(particleTimer, 0, 5f);
        InvokeRepeating("TurnOff", particleTimer, particleTimer);
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    private void TurnOff()
    {
        steamVolume.gameObject.SetActive(!steamVolume.gameObject.activeInHierarchy);
        if (steamVolume.gameObject.activeInHierarchy)
        {
            OnParticleStart.Invoke();
        }
    }

    private void FixedUpdate()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody>() != null && steamVolume.gameObject.activeInHierarchy)
        {

            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Impulse);
        }
    }
}
