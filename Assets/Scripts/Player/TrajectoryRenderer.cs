using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    private PlayerBall playerBall;

    [SerializeField] private int segmentCount = 30;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerBall = FindObjectOfType<PlayerBall>();
    }

    private void Update()
    {
        DrawTrajectory();
    }

    private void DrawTrajectory()
    {
        if (playerBall == null || startPoint == null || endPoint == null)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = segmentCount;

        float playerSize = playerBall.GetCurrentSize();
        lineRenderer.startWidth = playerSize;
        lineRenderer.endWidth = playerSize;

        for (int i = 0; i < segmentCount; i++)
        {
            float t = (float)i / (segmentCount - 1); 
            Vector3 point = Vector3.Lerp(startPoint.position, endPoint.position, t);
            lineRenderer.SetPosition(i, point);
        }
    }
}
