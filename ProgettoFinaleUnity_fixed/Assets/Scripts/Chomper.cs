using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;


public class Chomper : MonoBehaviour, IHittable
{
    public int Health;
    public UnityEvent<Collider> OnHitEvent;
    public Animator anim;
    public NavMeshAgent agent;

    float timer = 0f;
    public virtual void OnHit(Collider sender)
    {
        OnHitEvent?.Invoke(sender);
        Health--;
    }

    void rip()
    {
        //Destroy(transform.parent.gameObject);
        if (Health <= 0)
        {
            agent.speed = 0;
            timer += Time.deltaTime;
            anim.SetTrigger("rip");
            if (Health <= 0 && timer >= 1.35f)
            {
                gameObject.SetActive(false);
            }
        }
        
    }
    private void Update()
    {
        rip();
    }
}
