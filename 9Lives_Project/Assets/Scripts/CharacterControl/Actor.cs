using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor:MonoBehaviour
{
    // pamameters for state machine
    [HideInInspector] public ActorState _curState { set; get; }
    [HideInInspector] public ActorStateType _stateType;
    [HideInInspector] public Dictionary<ActorStateType, ActorState> _actorStateDic = new Dictionary<ActorStateType, ActorState>();

    void Awake()
    {
        InitState();
        InitCurState();
    }

    protected abstract void InitState();
    protected abstract void InitCurState();

    public void TransState(ActorStateType stateType)
    {
        if (_curState == null)
        {
            return;
        }
        if (_curState.StateType == stateType)
        {
            return;
        }
        else
        {
            ActorState _state;
            if (_actorStateDic.TryGetValue(stateType, out _state))
            {
                _curState.Exit();
                _curState = _state;
                _curState.Enter(this);
                _stateType = _curState.StateType;
            }
        }
    }

    private void FixedUpdate()
    {
        FixedUpdateState();
    }

    private void Update()
    {
        UpdateState();
    }
    private void FixedUpdateState()
    {
        if (_curState != null)
        {
            _curState.FixedUpdate();
        }
    }
    private void UpdateState()
    {
        if (_curState != null)
        {
            _curState.Update();
        }
    }


}





