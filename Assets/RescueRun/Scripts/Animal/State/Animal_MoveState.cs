using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RescueRun
{
    public class Animal_MoveState : AnimalState
    {
        private Vector3 randomPos;
        private bool _isHolder;
        public Animal_MoveState(AnimalStateMachine stateMachine, Animal animal, string animBoolName) : base(stateMachine, animal, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            _isHolder = animal.isHolder;
        }

        public override void Enter()
        {
            base.Enter();
            randomPos = animal.RandomPos();
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
            if (_isHolder) return;

            animal.Moving(randomPos);
            var velocity = animal.navMeshAgent.velocity;
            if (velocity == Vector3.zero)
            {
                stateMachine.ChangeState(animal.animalIdle);
            }

        }
    }

}
