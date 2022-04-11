using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;



public class Chomper : MonoBehaviour, IHittable
{
    public int Health;
    public UnityEvent<Collider> OnHitEvent;
    //public UnityEvent<float> OnAttackHitted;
    public Animator anim;
    public NavMeshAgent agent;
    public Slider HP_Slider;
    public UnityEvent<bool> HandleAnim;
    public UnityEvent OnDeath;
    EllenHealthScript EHS;

    void Start()
    {
        GetEllen();
        HP_Slider.maxValue = Health;
        HP_Slider.value = Health;
    }

    public void GetEllen()
    {
        GameObject ellen = GameObject.Find("Ellen PLAYER");
        EHS = ellen.GetComponent<EllenHealthScript>();
    }
    public virtual void OnHit(Collider sender)
    {
        OnHitEvent?.Invoke(sender);
        Health--;
        HP_Slider.value = Health;
        if (Health <= 0)
        {
            agent.speed = 0;
            anim.SetTrigger("rip");
            HP_Slider.transform.parent.gameObject.SetActive(false);
            OnDeath?.Invoke();
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
    public virtual void OnDeathEndAnim()
    {
        HandleAnim?.Invoke(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (EHS == null)
        {
            GetEllen();
        }
        EHS.DamagePlayer(20f);
        //Debug.Log("20 DAMAGE");
        //OnAttackHitted?.Invoke(20f);
    }
}
