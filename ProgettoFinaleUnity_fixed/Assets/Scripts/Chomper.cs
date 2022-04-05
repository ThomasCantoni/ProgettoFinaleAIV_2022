using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UI;



public class Chomper : MonoBehaviour, IHittable
{
    public int Health;
    public UnityEvent<Collider> OnHitEvent;
    public Animator anim;
    public NavMeshAgent agent;
    public Slider HP_Slider;

    float timer = 1.35f;
    public virtual void OnHit(Collider sender)
    {
        OnHitEvent?.Invoke(sender);
        Health--;
        HP_Slider.value = Health;
        if (Health <= 0)
        {
            agent.speed = 0;
            timer += Time.deltaTime;
            anim.SetTrigger("rip");
            StartCoroutine(animDie(timer));
            HP_Slider.transform.parent.gameObject.SetActive( false);
        }
    }
    public IEnumerator animDie(float timer)
    {
        yield return new WaitForSeconds(timer);
        this.gameObject.SetActive(false);
    }
}
