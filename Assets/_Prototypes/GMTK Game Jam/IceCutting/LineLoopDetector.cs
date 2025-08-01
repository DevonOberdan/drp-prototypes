using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineDrawer))]
public class LineLoopDetector : MonoBehaviour
{
    public UnityEvent<int> OnLoopCreated;

    [SerializeField] private int minimumPointGap = 15;
    [SerializeField] private float coolDownTotalTime = 0.2f;
    [SerializeField] private int maxloopCuts;
    [SerializeField] TextMeshProUGUI loopCountText;

    [SerializeField] private bool debug;

    private LineDrawer lineDrawer;
    private LineRenderer lineRenderer;

    private int loopsCreated = 0;

    private float coolDownTime;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineDrawer = GetComponent<LineDrawer>();
    }

    private void Start()
    {
        lineDrawer.OnAddPathPoint.AddListener(DetectLoop);
        OnLoopCreated.AddListener((i) => coolDownTime = coolDownTotalTime);
    }

    private void Update()
    {
        coolDownTime -= Time.deltaTime;
        coolDownTime = Mathf.Clamp(coolDownTime, 0, coolDownTotalTime);
        loopCountText.text = loopsCreated + "/" + maxloopCuts;
    }

    private void DetectLoop()
    {
        if (coolDownTime > 0)
            return;

        Vector3 newPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        for (int i = 0; i < lineRenderer.positionCount - minimumPointGap; i++)
        {
            if (Vector3.Distance(newPoint, lineRenderer.GetPosition(i)) < 0.15f && loopsCreated != maxloopCuts)
            {
                OnLoopCreated.Invoke(i);
                loopsCreated++;

                if (debug)
                {
                    Debug.Log("Made loop!");
                }
            }
        }
    }
}