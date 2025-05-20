/* SETUP:
 * 1. Set tag of camera to "MainCamera"
 * 2. Make sure camera is not attached to player object
 * 3. Attach script to camera
 * 4. See FirstPersonMovementRB script to set up player object if needed
 */

using UnityEngine;

public class FPSCameraRB : MonoBehaviour
{
    [SerializeField] private float lookVerticalSensitivity = 100f;
    [SerializeField] private float lookHorizontalSensitivity = 100f;

    Transform player;
    CapsuleCollider playerCollider;

    Vector3 cameraRotationInputVect;
    Vector3 velocity;

    float minAngle;
    float maxAngle;
    float rotationLerpSpeed;
    float positionLerpSpeed;
    float cameraHeightOffset;


    float verticalRotation;

    Vector3 playerTop;

    public enum PositionStyle { Raw, Lerp }
    public PositionStyle positionBehaviour = new PositionStyle();

    public enum RotationStyle { Raw, Lerp }
    public RotationStyle rotationBehaviour = new RotationStyle();

    float previousInput,currentInput;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCollider = player.GetComponent<CapsuleCollider>();
    }

    void Start()
    {       
        cameraRotationInputVect = Vector3.zero;

        minAngle = -90.0f;
        maxAngle = 90.0f;
        rotationLerpSpeed = 25.0f;
        //positionLerpSpeed = 3.0f;
        cameraHeightOffset = 0.1f;

        playerTop = (player.transform.localPosition + Vector3.up * (playerCollider.height / 2));
    }

    void Update()
    {
        CameraRotation();
    }

    private void FixedUpdate()
    {
        player.transform.Rotate(0, Input.GetAxis("Mouse X") * lookHorizontalSensitivity/100f, 0);
    }

    void CameraRotation()
    {
        currentInput = Input.GetAxisRaw("Mouse Y");

        if(Mathf.Abs(currentInput-previousInput) > 10f)
        {
            previousInput = currentInput;
            return;
        }

        previousInput = currentInput;

        verticalRotation += Input.GetAxisRaw("Mouse Y") * lookVerticalSensitivity * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, minAngle, maxAngle);


        if (rotationBehaviour == RotationStyle.Raw)
            transform.localEulerAngles = Vector3.left * verticalRotation;
        else
            transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, Quaternion.Euler(Vector3.left*verticalRotation), rotationLerpSpeed * Time.deltaTime);
    }
    
}
