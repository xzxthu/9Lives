using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorState
{
    public abstract ActorStateType StateType { get; }
    public abstract void Enter(params object[] param);
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();
}

public enum ActorStateType
{
    Idle,
    Run,
    Jump,
    Hang,
}
