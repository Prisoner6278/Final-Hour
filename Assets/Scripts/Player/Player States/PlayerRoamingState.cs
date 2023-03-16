using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoamingState : PlayerState
{
    public PlayerRoamingState(PlayerStateController newController) : base(newController) { }

    // called when entering this state
    override public void Entry()
    {
        base.Entry();
        controller.movement.UnlockMovement();
    }

    // called when leaving this state
    override public void Exit()
    {
        base.Exit();
    }

    public override void Callback()
    {

    }

    public override void ChangeState(StateChangeInstruction instruction)
    {
        if (instruction == StateChangeInstruction.ChangeToDialogue)
            controller.SetCurrentState(controller.dialogueState);
        else if (instruction == StateChangeInstruction.ChangeToTransition)
            controller.SetCurrentState(controller.transitionState);
    }

    // called by controller on current state
    public override void ControlledUpdate()
    {
        //Debug.Log("in roaming state");

    }
}
