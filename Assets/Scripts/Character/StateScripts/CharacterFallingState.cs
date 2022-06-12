using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFallingState : CharacterBaseState
{

    public override void EnterState(CharacterStateManager character)
    {
            
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (character.stateDebug) Debug.Log("Falling State");
        character.ApplyGravity();

        if (character.horizontalMovement)
        {
            character.Move(Globals.SPEED_NORMAL);
            character.Rotate();
        }


        if (character.canDoubleJump && character.jumpPressed)
        {
            character.SwitchState(character.DoubleJumpState);
        }

        
        character.jumpPressed = false;

        if (character.groundedPlayer)
        {
            character.SwitchState(character.IdleState);
        }

        
        
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {

    }
    
}
