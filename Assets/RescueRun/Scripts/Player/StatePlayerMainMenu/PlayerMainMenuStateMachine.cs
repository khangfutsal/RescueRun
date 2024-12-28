using System.Collections;
using System.Collections.Generic;
using RescueRun;
using UnityEngine;

public class PlayerMainMenuStateMachine : MonoBehaviour
{
    public PlayerMainMenuState currentState { get; private set; }
    public void Initialize(PlayerMainMenuState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(PlayerMainMenuState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
