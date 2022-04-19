using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spitter : Enemy
{
    public GameObject BiteEffect;
    public Transform TonguePosition;

    public void OnBite()
    {
        GameObject biteSpit = Instantiate(BiteEffect, TonguePosition);
        Destroy(biteSpit, 0.5f);
    }
}
