using UnityEngine;

public class CharacterCrouchJumpState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character)
    {
        character.playerVelocity.y += Mathf.Sqrt(character.crouchJumpHeight * -3.0f * character.gravityValue);
        //jumpPressed is now false after we jump so we dont keep jumping
        character.jumpPressed = false;
        //they are no longer on the ground
        character.groundedPlayer = false;
        character.heightPadding = 0;
        character.canDoubleJump = false;
        character.crouchPressed = false;
        character.ApplyGravity();
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (character.stateDebug) Debug.Log("crouch jump state");
        character.jumpPressed = false;
        character.ApplyGravity();
        if (character.horizontalMovement)
        {
            character.Move(character.speed);
            character.Rotate();
        }

        if (character.groundPoundPressed)
        {
            character.SwitchState(character.GroundPoundingState);
        }

        if (character.groundedPlayer)
        {
            if (character.horizontalMovement)
            {
                character.SwitchState(character.WalkingState);
            }
            else
            {
                character.SwitchState(character.IdleState);
            }
        }
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {

    }
}
