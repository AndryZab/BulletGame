using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] private float size = 1f;
    [SerializeField] private float minSize = 0.2f;
    [SerializeField] private Transform Zones;
    [SerializeField] private Transform TargetToWin;
    [SerializeField] private GameObject[] VisualPlayerParts;
    public bool CanShoot(float projectileSize) => size - projectileSize >= minSize - 0.03f;
    private void Start()
    {
        LookAtTargetGameWin();   
    }
    private void LookAtTargetGameWin()
    {
        if (TargetToWin == null)
            return;

        Vector3 direction = TargetToWin.position - transform.position;

        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f) 
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Zones != null)
        {
            Vector3 zonesDir = TargetToWin.position - Zones.position;
            zonesDir.y = 0;
            if (zonesDir.sqrMagnitude > 0.001f)
                Zones.rotation = Quaternion.LookRotation(zonesDir);
        }
    }

    public void ReduceSize(float amount)
    {
        size -= amount;
        transform.localScale = Vector3.one * size;

        Vector3 scale = Zones.localScale;
        scale.x = transform.localScale.x;
        Zones.localScale = scale;
        CheckSizeToLosing();
    }
    private void CheckSizeToLosing()
    {
        if (size <= minSize)
        {
            GameManager.Instance.Losing();
            foreach (GameObject PlayerParts in VisualPlayerParts)
            {
                PlayerParts.SetActive(false);
            }
        }
    }
    public float GetCurrentSize()
    {
        return transform.localScale.x;
    }

}
