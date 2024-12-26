using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RescueRun
{
    public class PlayerState : MonoBehaviour
    {
        protected PlayerStateMachine stateMachine;
        protected Player player;
        protected float startTime;
        protected string animBoolName;

        public PlayerState(PlayerStateMachine stateMachine, Player player, string animBoolName)
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
}

