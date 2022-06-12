using UnityEngine;

public class CharacterIdleState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character)
    {
        //play idle animation
        

    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (character.stateDebug) Debug.Log("Idle State");
        character.ApplyGravity();
        character.previouslyMoving = false;
        if (character.jumpPressed && character.groundedPlayer)
        {
            character.SwitchState(character.JumpingState);
        }

        if (character.horizontalMovement)
        {
            character.SwitchState(character.WalkingState);
        }

        if (character.crouchPressed && character.groundedPlayer)
        {
            character.SwitchState(character.CrouchingState);
        }
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {

    }
}
