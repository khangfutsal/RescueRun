using System.Collections;
using System.Collections.Generic;
using RescueRun;
using UnityEngine;

public class PlayerMainMenuMoveState : PlayerMainMenuState
{
    public PlayerMainMenuMoveState(PlayerMainMenuStateMachine stateMachine, PlayerMainMenu player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        bool isMoving = player.CheckMoving();
        if (!isMoving)
        {
            stateMachine.ChangeState(player.playerIdle);
        }
        Vector3 curVelocity = player.GetCurrentVelocity();
        player.anim.SetFloat("Velocity", Mathf.Abs(curVelocity.magnitude));
    }
}
