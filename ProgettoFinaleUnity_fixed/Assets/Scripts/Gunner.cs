using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UI;



public class Gunner : MonoBehaviour, IHittable
{
    public int Health;
    public UnityEvent<Collider> OnHitEvent;
    public UnityEvent<float> OnAttackHitted;
    public Animator anim;
    public NavMeshAgent agent;
    //public Slider HP_Slider;
    public UnityEvent<bool> HandleAnim;

    float timer = 4.7f;
    public virtual void OnHit(Collider sender)
    {
        OnHitEvent?.Invoke(sender);
        Health--;
        //HP_Slider.value = Health;
        if (Health <= 0)
        {
            agent.speed = 0;
            timer += Time.deltaTime;
            anim.SetTrigger("rip");
            StartCoroutine(animDie(timer));
            //HP_Slider.transform.parent.gameObject.SetActive(false);
        }
    }
    public virtual void OnAttackStart()
    {
        HandleAnim?.Invoke(true);
    }

    public virtual void OnAttackEnd()
    {
        HandleAnim?.Invoke(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("success attacck");
        OnAttackHitted?.Invoke(20f);
    }
    public IEnumerator animDie(float timer)
    {
        yield return new WaitForSeconds(timer);
        this.gameObject.SetActive(false);
    }
}
