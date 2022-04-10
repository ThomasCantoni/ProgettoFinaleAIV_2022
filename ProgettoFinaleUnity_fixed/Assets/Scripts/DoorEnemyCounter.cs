using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorEnemyCounter : MonoBehaviour
{
    [SerializeField]
    public List<Chomper> Enemies;
    public UnityAction OnEnemyDeath;
    public int AmountDead =0;
    void Start()
    {
        OnEnemyDeath = IncreaseDead;
        foreach(Chomper x in Enemies)
        {
            x.OnDeath.AddListener(OnEnemyDeath);
        }
    }
    void IncreaseDead()
    {
        AmountDead++;
        if(AmountDead >= Enemies.Count)
        {
            this.GetComponent<Animator>().SetBool("isDoorOpen",true);

            this.GetComponent<Animator>().SetTrigger("OpenDoor");
        }
    }
    
}
