using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IHittable
{
    public int Health;
    public float MeleeDamage;
    public UnityEvent<Collider> OnHitEvent;
    public Animator Anim;
    public NavMeshAgent Agent;
    public Slider HP_Slider;
    public Image HP_Bar;
    public UnityEvent<bool> HandleAnim;
    public UnityEvent OnDeath;
    public UnityEvent<bool> OnReachHealthThreshold;
    protected EllenHealthScript ellenHealthScript;

    private bool reached75Percent = false;
    private bool reached50Percent = false;
    private bool reached25Percent = false;

    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        GetEllen();
        HP_Slider.maxValue = Health;
        HP_Slider.value = Health;
        HP_Bar.color = Color.green;
    }

    public void GetEllen()
    {
        GameObject ellen = GameObject.Find("Ellen PLAYER");
        ellenHealthScript = ellen.GetComponent<EllenHealthScript>();
    }

    public virtual HittableType OnHit(Collider sender)
    {
        OnHitEvent?.Invoke(sender);
        Health--;
        HP_Slider.value = Health;
        CheckHealthThresholds();
        HP_Bar.color = Color.Lerp(Color.red, Color.green, HP_Slider.value / HP_Slider.maxValue);
        if (Health <= 0)
        {
            Agent.speed = 0;
            Anim.SetTrigger("rip");
            HP_Slider.transform.parent.gameObject.SetActive(false);
            OnDeath?.Invoke();
        }
        return HittableType.Boss;
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
        HandleAnim?.Invoke(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ellenHealthScript == null)
        {
            GetEllen();
        }
        ellenHealthScript.DamagePlayer(MeleeDamage);
    }

    public void OnPlayerDeath()
    {
        HP_Slider.value = HP_Slider.maxValue;
        Health = (int)HP_Slider.maxValue;
        HP_Bar.color = Color.green;
        reached75Percent = false;
        reached50Percent = false;
        reached25Percent = false;
        HP_Slider.transform.parent.GetComponent<Canvas>().enabled = false;
    }

    protected void CheckHealthThresholds()
    {
        float ratio = HP_Slider.value / HP_Slider.maxValue;
        if (!reached75Percent && ratio <= 0.75f)
        {
            reached75Percent = true;
            OnReachHealthThreshold?.Invoke(false);
            return;
        }
        if (!reached50Percent && ratio <= 0.5f)
        {
            reached50Percent = true;
            OnReachHealthThreshold?.Invoke(true);
            return;
        }
        if (!reached25Percent && ratio <= 0.25f)
        {
            reached25Percent = true;
            OnReachHealthThreshold?.Invoke(false);
            return;
        }
    }
}
