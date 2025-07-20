using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float chargeSpeed = 0.3f;

    public int CountDestroyedObstacles;

    private PlayerBall playerBall;
    private Bullet _bullet;

    private void Awake()
    {
        playerBall = GetComponent<PlayerBall>();
    }

    public void StartCharging()
    {
        if (_bullet != null) return;
        float initialGrowth = chargeSpeed * Time.deltaTime;
        if (!playerBall.CanShoot(initialGrowth)) return;

        AudioManager.Instance.PlayOneShot(SoundType.Charge);

        GameObject obj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        _bullet = obj.GetComponent<Bullet>();
        _bullet.InitializeScaleBullet();
    }


    public void ChargingUpdate()
    {
        if (_bullet == null) return;

        float growth = chargeSpeed * Time.deltaTime;
        if (playerBall.CanShoot(growth))
        {
            _bullet.Grow(growth);
            playerBall.ReduceSize(growth);
        }
    }

    public void Release()
    {
        if (_bullet == null) return;
        AudioManager.Instance.DestroySoundTypeSource(SoundType.Charge);
        AudioManager.Instance.PlayOneShot(SoundType.Shoot);
        _bullet.ShootAtTarget("Target");
        CountDestroyedObstacles++;
        _bullet = null;
    }
}
