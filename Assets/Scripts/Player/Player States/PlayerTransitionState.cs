using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransitionState : PlayerState
{
    public PlayerTransitionState(PlayerStateController newController) : base(newController) { }

    // called when entering this state
    override public void Entry()
    {
        base.Entry();
        controller.movement.LockMovement();
    }

    // called when leaving this state
    override public void Exit()
    {
        base.Exit();
    }

    // called by movement script after completing move to new screen
    public override void Callback()
    {
        ChangeState(StateChangeInstruction.ChangeToRoam);
    }

    public override void ChangeState(StateChangeInstruction instruction)
    {
        if (instruction == StateChangeInstruction.ChangeToRoam)
            controller.SetCurrentState(controller.roamingState);
    }

    // called by controller on current state
    public override void ControlledUpdate()
    {
        Debug.Log("in transition state");
    }
}
