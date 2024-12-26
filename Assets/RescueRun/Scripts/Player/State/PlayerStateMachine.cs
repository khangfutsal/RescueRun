using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RescueRun
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public PlayerState currentState { get; private set; }
        public void Initialize(PlayerState startingState)
        {
            currentState = startingState;
            currentState.Enter();
        }

        public void ChangeState(PlayerState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }

}
