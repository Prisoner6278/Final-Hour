using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateController controller;

    public enum StateChangeInstruction
    {
        ChangeToRoam,
        ChangeToDialogue,
        ChangeToTransition
    }

    // assigns controller to state when its created
    public PlayerState(PlayerStateController newController)
    {
        controller = newController;
    }

    // called when entering this state
    public virtual void Entry()
    {
        // anything here applies when any state is entered
    }

    // called when leaving this state
    public virtual void Exit()
    {
        // anything here applies when any state is exited
    }

    public abstract void ChangeState(StateChangeInstruction instruction);
    public abstract void Callback(); // general use function for other classes to use with a state
    public abstract void ControlledUpdate(); // called by controller on current state
}
