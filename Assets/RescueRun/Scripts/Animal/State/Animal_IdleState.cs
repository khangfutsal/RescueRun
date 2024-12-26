using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RescueRun
{
    public class Animal_IdleState : AnimalState
    {
        private Vector3 velocity;
        private bool _isHolder;
        private bool _isCat01;

        private float timeRandom;
        private float curTime;
        private float start = 1;
        private float end = 3;
        public Animal_IdleState(AnimalStateMachine stateMachine, Animal animal, string animBoolName) : base(stateMachine, animal, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            _isHolder = animal.isHolder;
            _isCat01 = animal.isCat01;
            timeRandom = Random.Range(start, end);
            curTime = timeRandom;
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
            if (_isHolder && _isCat01)
            {
                stateMachine.ChangeState(animal.animalMove);
                return;
            }
            else if(_isHolder && !_isCat01)
            {
                return;
            }
            else
            {
                curTime -= Time.deltaTime;
                if (curTime <= 0)
                {
                    stateMachine.ChangeState(animal.animalMove);
                }
                
            }

        }
    }
}

