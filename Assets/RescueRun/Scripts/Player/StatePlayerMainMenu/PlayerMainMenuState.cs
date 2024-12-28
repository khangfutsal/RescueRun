using System.Collections;
using System.Collections.Generic;
using RescueRun;
using UnityEngine;

public class PlayerMainMenuState : MonoBehaviour
{
    protected PlayerMainMenuStateMachine stateMachine;
    protected PlayerMainMenu player;
    protected float startTime;
    protected string animBoolName;

    public PlayerMainMenuState(PlayerMainMenuStateMachine stateMachine, PlayerMainMenu player, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }


    public virtual void DoChecks() { }
}
