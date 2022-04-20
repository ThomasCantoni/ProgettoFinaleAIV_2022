using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    BaseState currentState;

    public BaseState CurrentState
    {
        get { return currentState; }
    }

    void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.OnEnter();
    }

    void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }

    void LateUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }

    public virtual void ChangeState(BaseState newState)
    {
        if (currentState != null)
            currentState.OnExit();

        currentState = newState;
        currentState.OnEnter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }
}