using UnityEngine;

public abstract class Obstacle : MonoBehaviour, IDestructible
{
    public bool IsInfected { get; set; } = false;
    public GameObject ExplosionEffect;
    protected Animator animatorInfect;
    protected AudioClip ExplosionSound;
    public void Start()
    {
        animatorInfect = GetComponent<Animator>();
    }
    public abstract void Explodes();
    public virtual void Infect()
    {
        animatorInfect.SetTrigger("IsInfected");
        IsInfected = true;
    }
}
