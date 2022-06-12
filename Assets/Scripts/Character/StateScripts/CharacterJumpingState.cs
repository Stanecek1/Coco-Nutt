using UnityEngine;

public class CharacterJumpingState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character)
    {
        character.Jump();
        character.ApplyGravity();
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (character.stateDebug) Debug.Log("Jumping");
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

        if (character.jumpPressed && character.canDoubleJump)
        {
            character.SwitchState(character.DoubleJumpState);
        }
        else
        {
            character.jumpPressed = false;
        }

        if (character.attackPressed)
        {
            character.SwitchState(character.AirAttackingState);
        }

        if (character.groundPoundPressed)
        {
            character.SwitchState(character.GroundPoundingState);
        }
        character.ApplyGravity();
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
