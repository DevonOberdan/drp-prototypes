using FinishOne.GeneralUtilities;
using UnityEngine;

public class ObjectFollowMouse : MonoBehaviour
{
    public LayerMask groundLayer; // Assign the layer of your ground in the Inspector
    public float moveForce = 10f;
    public float turnSpeed = 5f;
    private Rigidbody rb;
    private AudioSource audioSource;

    private float startY;

    RigidbodyConstraints startConstraints;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        startY = transform.position.y;

        startConstraints = rb.constraints;
    }

    void FixedUpdate()
    {
        transform.position = transform.position.NewY(startY);

        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Perform a raycast, specifically targeting the ground layer
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;

            Vector3 moveDirection = transform.up * verticalInput;

            // Apply force for movement
            rb.AddForce(-transform.up * moveForce, ForceMode.Acceleration);

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(-90f, lookRotation.eulerAngles.y, 0f);
                Quaternion targetRotation = Quaternion.Euler(-90f, transform.eulerAngles.y + moveDirection.x * turnSpeed, 0);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed));
            }
        }
    }

    public void SetFollow(bool follow)
    {
        rb.constraints = follow ? startConstraints : RigidbodyConstraints.FreezeAll;
        this.enabled = follow;
    }

    public void SetFreeze(bool freeze)
    {
        SetFollow(!freeze);
    }

    public void PlaySkateSound(AudioClip clip)
    {
        audioSource.Play();
    }
}
