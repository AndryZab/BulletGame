using UnityEngine;

public class ZoneObstacles : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;

    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

    }

    private void Update()
    {
        if (boxCollider == null)
            return;

        Vector3 center = boxCollider.bounds.center;

        Vector3 globalScale = transform.lossyScale;
        Vector3 halfExtents = new Vector3(
            boxCollider.size.x * 0.5f * globalScale.x,
            boxCollider.size.y * 0.5f * globalScale.y,
            boxCollider.size.z * 0.5f * globalScale.z
        );

        Quaternion orientation = transform.rotation;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, orientation, obstacleLayer);

        if (hits.Length == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
            return;

        Vector3 center = boxCollider.bounds.center;
        Quaternion orientation = transform.rotation;
        Vector3 globalScale = transform.lossyScale;

        Vector3 scaledSize = new Vector3(
            boxCollider.size.x * globalScale.x,
            boxCollider.size.y * globalScale.y,
            boxCollider.size.z * globalScale.z
        );

        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(center, orientation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, scaledSize);
    }
}
