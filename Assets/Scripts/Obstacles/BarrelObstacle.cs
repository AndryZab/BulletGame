using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelObstacle : Obstacle
{
    public override void Infect()
    {
        base.Infect();
    }
    public override void Explodes()
    {
        GameObject prefabExpl = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Destroy(prefabExpl, 1f);
        AudioManager.Instance.PlayOneShot(SoundType.BarrelExplosion);
        Destroy(gameObject);
    }
}
