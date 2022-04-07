using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdle : StateMachineBehaviour
{
    readonly float MinTime = 10f;
    readonly float MaxTime = 17f;

    float Timer = 10f;

    string[] Triggers = { "Idle1", "Idle3", "Idle4" };

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Timer <= 0f)
        {
            RndIdle(animator);
            Timer = Random.Range(MinTime, MaxTime);
        }
        else
        {
            Timer -= Time.deltaTime;
        }
    }

    void RndIdle(Animator animator)
    {
        System.Random rnd = new System.Random();
        int idle = rnd.Next(Triggers.Length);
        string Trigger = Triggers[idle];
        animator.SetTrigger(Trigger);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
