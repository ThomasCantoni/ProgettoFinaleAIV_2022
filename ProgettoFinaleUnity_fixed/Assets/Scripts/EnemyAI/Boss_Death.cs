using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Death : Enemy_Death
{
    private float timer;
    private bool startTimer;

    BossSM sm;
    public Boss_Death(BossSM stateMachine) : base("Boss_Death", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        timer = 0f;
        startTimer = false;

        sm.animAct += OnEndDeathAnimation;

        sm.BodyCollider.enabled = false;
        sm.MeleeAttackCollider.enabled = false;
    }

    public override void UpdateLogic()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                sm.transform.gameObject.SetActive(false);
                sm.animAct -= OnEndDeathAnimation;
            }
        }
    }

    public override void OnEndDeathAnimation(bool value)
    {
        if (value)
            startTimer = true;
    }

    public override void OnExit()
    {
        sm.animAct -= OnEndDeathAnimation;
    }
}
