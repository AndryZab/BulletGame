using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float forwardSpeed = 3f;
    [SerializeField] private float rayDistance;

    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private LayerMask ZoneLayer;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private Rigidbody rb;
    private bool isGrounded = false;
    private bool reachedObstacle = false;
    private Shooter shooter;
    private static bool _IsMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        shooter = GetComponent<Shooter>();
        _IsMoving = false;
    }

    private void Update()
    {
        Vector3 correctedPos = GetPositionOnLine(startPoint.position, endPoint.position, transform.position);
        transform.position = new Vector3(correctedPos.x, transform.position.y, correctedPos.z);

        if (isGrounded && !_IsMoving && !GameManager.Instance.IsGameEnd())
        {
            StartCoroutine(JumpTowardsTarget(endPoint.position));
        }
    }

    private Vector3 GetPositionOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 lineDirection = (lineEnd - lineStart).normalized;
        Vector3 projectedPoint = lineStart + Vector3.Project(point - lineStart, lineDirection);
        return projectedPoint;
    }

    private IEnumerator JumpTowardsTarget(Vector3 targetPos)
    {
        reachedObstacle = false;
        _IsMoving = true;
        while (!reachedObstacle && Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector3 dir = (targetPos - transform.position);
            dir.y = 0;
            dir.Normalize();

            bool hit = Physics.Raycast(transform.position + Vector3.up * 0.1f, dir, out RaycastHit hitInfo, rayDistance, ZoneLayer);
            if (hit)
            {
                _IsMoving = false;
                yield break;
            }

            while (!isGrounded)
                yield return null;

            Vector3 jumpVelocity = dir * forwardSpeed + Vector3.up * jumpForce;
            rb.velocity = jumpVelocity;

            isGrounded = false;

            float maxWaitTime = 2f;
            float waitTimer = 0f;
            while (!isGrounded && waitTimer < maxWaitTime)
            {
                waitTimer += Time.deltaTime;
                yield return null;
            }
        }

        _IsMoving = false;
    }

    public static bool IsMoving()
    {
        return _IsMoving;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & GroundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & GroundLayer) != 0)
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (endPoint == null)
            return;

        Vector3 dir = (endPoint.position - transform.position);
        dir.y = 0;
        dir.Normalize();
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(rayOrigin, dir * rayDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rayOrigin, 0.05f);

        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPoint.position, endPoint.position);
        }
    }
}
