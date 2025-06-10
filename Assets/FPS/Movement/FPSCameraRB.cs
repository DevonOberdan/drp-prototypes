using DG.Tweening;
using UnityEngine;

public class FPSCameraRB : MonoBehaviour
{
    [SerializeField] private float lookVerticalSensitivity = 100f;
    [SerializeField] private float lookHorizontalSensitivity = 100f;

    [SerializeField] private InputReader inputReader;

    private Transform player;

    private float minAngle;
    private float maxAngle;
    private float rotationLerpSpeed;
    private float verticalRotation;

    public enum PositionStyle { Raw, Lerp }
    public PositionStyle positionBehaviour = new();

    public enum RotationStyle { Raw, Lerp }
    public RotationStyle rotationBehaviour = new();

    private float previousInput, currentInput;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {       
        minAngle = -90.0f;
        maxAngle = 90.0f;
        rotationLerpSpeed = 25.0f;

        inputReader.EnablePlayerActions();
    }

    private void Update()
    {
        CameraRotation();
    }

    private void FixedUpdate()
    {
        player.transform.Rotate(0, inputReader.LookVector.x * lookHorizontalSensitivity, 0);
    }

    public void DisableInput(bool disable)
    {
        this.enabled = !disable;
    }

    private void CameraRotation()
    {
        currentInput = inputReader.LookVector.y;

        if (Mathf.Abs(currentInput-previousInput) > 10f)
        {
            previousInput = currentInput;
            return;
        }


        previousInput = currentInput;

        verticalRotation += currentInput * lookVerticalSensitivity * Time.deltaTime;
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