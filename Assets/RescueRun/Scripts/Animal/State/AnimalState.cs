using System.Collections;
using System.Collections.Generic;
using RescueRun;
using UnityEngine;
namespace RescueRun
{
    public class AnimalState : MonoBehaviour
    {
        protected AnimalStateMachine stateMachine;
        protected Animal animal;
        protected float startTime;
        protected string animBoolName;

        public AnimalState(AnimalStateMachine stateMachine, Animal animal, string animBoolName)
        {
            this.stateMachine = stateMachine;
            this.animal = animal;
            this.animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            DoChecks();
            startTime = Time.time;
            animal.anim.SetBool(animBoolName, true);
        }

        public virtual void Exit()
        {
            animal.anim.SetBool(animBoolName, false);
        }

        public virtual void Update() { }

        public virtual void FixedUpdate() { }


        public virtual void DoChecks() { }
    }

}

