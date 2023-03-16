using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerState currentState;

    [HideInInspector]
    public PlayerRoamingState roamingState;
    [HideInInspector]
    public PlayerDialogueState dialogueState;
    [HideInInspector]
    public PlayerTransitionState transitionState;

    // public references for states to access
    [HideInInspector]
    public PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();

        // create states
        roamingState = new PlayerRoamingState(this);
        dialogueState = new PlayerDialogueState(this);
        transitionState = new PlayerTransitionState(this);

        currentState = roamingState;
        SetCurrentState(roamingState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.ControlledUpdate();
    }

    // this can be called from anywhere
    public void ChangeState(PlayerState.StateChangeInstruction instruction)
    {
        currentState.ChangeState(instruction);
    }

    // this should only be called within one of the states
    public void SetCurrentState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Entry();
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }
}
