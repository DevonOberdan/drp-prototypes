using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Detection : MonoBehaviour
{
    public float detectRange = 50.0f;
    public float detectAngle = 45f;
    public GameObject Player;
    public Image detectImg;

    private FPSMovementRB playerMovement;
    float detectTime = Mathf.Clamp(0.00000f, 0.00000f, 1.00000f);
    bool isInAngle, isinInRange, isNotHidden;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GameObject.FindAnyObjectByType<FPSMovementRB>();
    }

    // Update is called once per frame
    void Update()
    {
        isInAngle = false;
        isinInRange = false;
        isNotHidden = false;
        detectImg.fillAmount = detectTime;

        if (Vector3.Distance(transform.position, Player.transform.position) < detectRange)
        {
            isinInRange = true;

        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, (Player.transform.position - transform.position), out hit, Mathf.Infinity))
        {
            if(hit.transform == Player.transform)
            {
                isNotHidden=true;
            }
        }
        DetectionStatus();

    }
    public void DetectionStatus()
    {
        Vector3 side1 = Player.transform.position - transform.position;
        Vector3 side2 = transform.forward;
        float angle = Vector3.SignedAngle(side1, side2, Vector3.up);
        if (angle < detectAngle && angle < -1 * detectAngle)
        {
            isInAngle = true;
        }
        if (isInAngle && isinInRange && isNotHidden)
        {
            detectTime += 0.0008f;
            Debug.Log("They are tracking you. Hide!");
            //Debug.Log("Increasing to: " + detectTime);

        }
        if (!isInAngle && detectTime > 0)
        {
            detectTime = detectTime - 0.0001f;
            //Debug.Log("Decreasing to: " + detectTime);
        }
        if (detectTime >= 1.0f)
        {
            playerMovement.Freeze = true;
            detectTime = 0.0f;
            Debug.Log("you were spotted, but we will help you out with that for now");
        }
    }
}
