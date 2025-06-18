using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float maxPitch = 10;
    [SerializeField] float maxYaw = 10;
    [SerializeField] float maxRoll = 10;

    [Range(0,1)]
    [SerializeField] float shake;

    [Range(0, 1)]
    [SerializeField] float timeFactor = 0.5f;

    float pitch, yaw, roll;
    float seed;

    float trauma;

    public float Trauma
    {
        get => trauma;
        set => trauma = Mathf.Clamp01(value);
    }

    void Start() => seed = Random.Range(0, 1000);

    public void AddTrauma(float traumaAmount)
    {
        Trauma += traumaAmount;

        if(Trauma-traumaAmount == 0)
            StartCoroutine(Shake());        
    }

    public IEnumerator Shake()
    {
        while(Trauma > 0)
        {
            shake = Mathf.Pow(Trauma, 2);

            //calculate shake
            pitch = maxPitch * shake * Mathf.PerlinNoise(seed, Time.time);
            yaw   = maxYaw   * shake * Mathf.PerlinNoise(seed + 1, Time.time);
            roll  = maxRoll  * shake * Mathf.PerlinNoise(seed + 2, Time.time);

            // apply shake
            transform.localRotation = Quaternion.Euler(pitch, yaw, roll) * transform.localRotation;

            Trauma -= Time.deltaTime * timeFactor;
            yield return new WaitForEndOfFrame();
        }
    }
}