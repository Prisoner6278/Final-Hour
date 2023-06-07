using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueState : PlayerState
{
    public PlayerDialogueState(PlayerStateController newController) 
        : base(newController) 
    {
        DialogueDisplay.Instance().OnDialogueCompletion.AddListener(FinishedDialogue);
    }

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

    public override void Callback()
    {

    }

    public override void ChangeState(StateChangeInstruction instruction)
    {
        if (instruction == StateChangeInstruction.ChangeToRoam)
            controller.SetCurrentState(controller.roamingState);
    }

    private void FinishedDialogue()
    {
        ChangeState(StateChangeInstruction.ChangeToRoam);
    }

    // called by controller on current state
    public override void ControlledUpdate()
    {
    }
}
