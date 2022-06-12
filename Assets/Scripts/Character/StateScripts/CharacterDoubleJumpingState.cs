using UnityEngine;

public class CharacterDoubleJumpingState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character)
    {
        character.playerVelocity.y = 0;
        character.Jump();
        character.ApplyGravity();
        character.canDoubleJump = false;
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (character.stateDebug) Debug.Log(" Double Jumping");
        if (character.horizontalMovement)
        {
            character.previouslyMoving = true;
            character.Move(Globals.SPEED_NORMAL);
            character.Rotate();
        }
        else
        {
            character.previouslyMoving = false;
        }
        character.ApplyGravity();
        character.jumpPressed = false;

        if (character.groundPoundPressed)
        {
            character.SwitchState(character.GroundPoundingState);
            return;
        }

        if (character.attackPressed)
        {
            character.SwitchState(character.AirAttackingState);
        }

        if (character.groundedPlayer)
        {
            if (character.horizontalMovement)
            {
                character.SwitchState(character.WalkingState);
                return;
            }
            else
            {
                character.SwitchState(character.IdleState);
                return;
            }

        }
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {

    }
}
