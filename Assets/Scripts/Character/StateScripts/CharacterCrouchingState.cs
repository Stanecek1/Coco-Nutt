using UnityEngine;

public class CharacterCrouchingState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character)
    {
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (character.stateDebug) Debug.Log("Crouching State");
        //if the player is croching, we do a bunch of different shit
        if (character.crouchPressed)
        {
            if (character.groundedPlayer)
            {
                //if we were moving before we crouched
                if (character.previouslyMoving && character.crouchSlipSpeed > 0)
                {
                    //make the player crouch slip
                    character.Move(character.crouchSlipSpeed);
                    //reduce the speed so they slow down as they slip
                    character.crouchSlipSpeed -= .2f;
                    //let them rotate
                    character.Rotate();

                }
                //if we werent moving before we crouched
                else
                {
                    character.previouslyMoving = false;

                    if (character.horizontalMovement)
                    {
                        //let the player crouch walk
                        character.Move(character.crouchSpeed);
                    }
                    

                }

                if (character.jumpPressed)
                {
                    //switch to crouchJump state
                    character.SwitchState(character.CrouchJumpState);
                    return;
                }
                character.ApplyGravity();
                character.Rotate();
            }
            else
            {
                character.SwitchState(character.IdleState);
            }
        }
        else
        {
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
        
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {

    }
}
