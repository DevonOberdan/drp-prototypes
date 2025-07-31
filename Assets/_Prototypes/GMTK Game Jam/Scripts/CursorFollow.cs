using UnityEngine;

public class ObjectFollowMouse : MonoBehaviour
{
    public LayerMask groundLayer; // Assign the layer of your ground in the Inspector
    public float moveForce = 10f;
    public float turnSpeed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Perform a raycast, specifically targeting the ground layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // Move the object to the hit point on the ground
            transform.position = Vector3.Lerp(transform.position, hit.point, Time.deltaTime);

            Vector3 targetPosition = hit.point;

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;

            Vector3 moveDirection = transform.forward * verticalInput;

            // Apply force for movement
            rb.AddForce(moveDirection * moveForce, ForceMode.Acceleration);

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(-90f, lookRotation.eulerAngles.y, 0f);
                Quaternion targetRotation = Quaternion.Euler(-90f, transform.eulerAngles.y + moveDirection.x * turnSpeed, 0);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed));
            }
        }
    }
}
