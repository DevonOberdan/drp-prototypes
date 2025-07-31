using UnityEngine;

public class ObjectFollowMouse : MonoBehaviour
{
    public LayerMask groundLayer; // Assign the layer of your ground in the Inspector
    public float playerCursorOffset;

    void Update()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform a raycast, specifically targeting the ground layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // Move the object to the hit point on the ground
            transform.position = Vector3.Lerp(transform.position, hit.point, playerCursorOffset * Time.deltaTime);
        }
    }
}
