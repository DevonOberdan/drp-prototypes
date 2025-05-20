using UnityEngine;

public class GravitySource : MonoBehaviour
{
    [SerializeField] bool pullToPosition = true;

    float gravityForce = 14f;

    public Vector3 BodyGravityDirection(Transform body, bool repel = false)
    {
        Vector3 gravityDir = pullToPosition ? (body.position - transform.position).normalized : Vector3.down;
        gravityDir *= repel ? -1 : 1;

        return gravityDir;
    }

    public Vector3 Attract(Transform body, bool repel = false, float boostFactor = 1)
    {
        Vector3 gravityDir = BodyGravityDirection(body, repel);
        body.GetComponent<Rigidbody>().AddForce(gravityForce * boostFactor * gravityDir);

        return gravityDir;
    }
}