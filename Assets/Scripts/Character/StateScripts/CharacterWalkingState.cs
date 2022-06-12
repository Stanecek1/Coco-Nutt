using UnityEngine;

public class CharacterWalkingState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character)
    {
        
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (character.stateDebug) Debug.Log("Walking State");
        character.previouslyMoving = true;
        if (!character.horizontalMovement && character.groundedPlayer)
        {
            character.SwitchState(character.IdleState);
            return;
        }
        if (character.crouchPressed && character.groundedPlayer)
        {
            character.SwitchState(character.CrouchingState);
        }
        if (character.jumpPressed && character.groundedPlayer)
        {
            character.SwitchState(character.JumpingState);
            return;
        }

        else
        {
            character.ApplyGravity();
            character.Rotate();
            character.Move(Globals.SPEED_NORMAL);
        }
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {

    }
}
