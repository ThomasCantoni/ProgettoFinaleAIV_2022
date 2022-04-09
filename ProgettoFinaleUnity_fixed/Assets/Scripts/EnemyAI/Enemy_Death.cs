using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Death : BaseState
{
    public Enemy_Death(string name, StateMachine stateMachine) : base(name, stateMachine)
    {
    }

    public virtual void OnEndDeathAnimation(bool value)
    {
    }
}
