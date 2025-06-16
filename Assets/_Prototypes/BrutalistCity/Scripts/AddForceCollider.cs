using UnityEngine;

public class AddForceCollider : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private bool whenDisabled;


    private void OnTriggerEnter(Collider other)
    {
        if(this.enabled || whenDisabled)
        {
            Debug.Log("Launch: "+other.gameObject.name);
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Impulse);
        }
    }
}
