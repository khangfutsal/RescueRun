using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RescueRun
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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
            player.MovingWithJoyStick();
            bool isMoving = player.CheckMoving();
            if (!isMoving)
            {
                stateMachine.ChangeState(player.playerIdle);
            }
            Vector3 curVelocity = player.GetCurrentVelocity();
            player.anim.SetFloat("Velocity", Mathf.Abs(curVelocity.magnitude));
        }
    }
}

