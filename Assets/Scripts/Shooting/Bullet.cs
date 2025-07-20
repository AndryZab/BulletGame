using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float size = 0.3f;
    [SerializeField] protected float speed = 7f;
    private float infectionRadius => sphereCollider.radius * transform.localScale.x + 0.03f;

    [SerializeField] private LayerMask obstacleLayer;
    private Transform TargetToWin;
    private SphereCollider sphereCollider;

    private Vector3 moveDirection;
    private bool hasInfected = false;
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void InitializeScaleBullet()
    {
        size = 0.1f;
        UpdateVisual();
    }

    public void Grow(float amount)
    {
        size += amount;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        transform.localScale = Vector3.one * size;
    }

    
    public void ShootAtTarget(string target)
    {

        TargetToWin = GameObject.FindGameObjectWithTag(target)?.transform;
        if (TargetToWin != null)
        {
            moveDirection = (TargetToWin.position - transform.position).normalized;
        }
    }

    void Update()
    {
        MoveBullet();
        CheckBulletHit();
        CheckDistanceToTarget();
        
    }
    private void CheckDistanceToTarget()
    {
        if (TargetToWin == null) return;

        if (Vector3.Distance(transform.position, TargetToWin.position) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void MoveBullet()
    {
        if (moveDirection != Vector3.zero && !hasInfected)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }
    private void CheckBulletHit()
    {
        if (hasInfected) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, infectionRadius, obstacleLayer);
        bool infectedSomeone = false;

        foreach (var col in hits)
        {
            if (((1 << col.gameObject.layer) & obstacleLayer) != 0 &&
                col.TryGetComponent<IDestructible>(out var destructible) &&
                !destructible.IsInfected)
            {
                destructible.Infect();
                infectedSomeone = true;
            }
        }

        if (infectedSomeone)
        {
            hasInfected = true;

            bool allInfected = true;
            foreach (var col in hits)
            {
                if (((1 << col.gameObject.layer) & obstacleLayer) != 0 &&
                    col.TryGetComponent<IDestructible>(out var destructible) &&
                    !destructible.IsInfected)
                {
                    allInfected = false;
                    break;
                }
            }

            if (allInfected)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, infectionRadius);
    }
}