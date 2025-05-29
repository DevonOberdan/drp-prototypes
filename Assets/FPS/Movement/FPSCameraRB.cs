/* SETUP:
 * 1. Set tag of camera to "MainCamera"
 * 2. Make sure camera is not attached to player object
 * 3. Attach script to camera
 * 4. See FirstPersonMovementRB script to set up player object if needed
 */

using DG.Tweening;
using UnityEngine;

public class FPSCameraRB : MonoBehaviour
{
    [SerializeField] private float lookVerticalSensitivity = 100f;
    [SerializeField] private float lookHorizontalSensitivity = 100f;

    Transform player;

    float minAngle;
    float maxAngle;
    float rotationLerpSpeed;
    float verticalRotation;

    public enum PositionStyle { Raw, Lerp }
    public PositionStyle positionBehaviour = new();

    public enum RotationStyle { Raw, Lerp }
    public RotationStyle rotationBehaviour = new();

    float previousInput,currentInput;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {       
        minAngle = -90.0f;
        maxAngle = 90.0f;
        rotationLerpSpeed = 25.0f;
    }

    private void Update()
    {
        CameraRotation();
    }

    private void FixedUpdate()
    {
        player.transform.Rotate(0, Input.GetAxis("Mouse X") * lookHorizontalSensitivity/100f, 0);
    }

    private void CameraRotation()
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

    public void DoFOV(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }
    
}
