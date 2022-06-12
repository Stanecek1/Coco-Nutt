using System.Collections;
using UnityEngine;

public class CharacterAirAttackingState : CharacterBaseState
{
    private float currentAirAttackTime;
    public override void EnterState(CharacterStateManager character)
    {
        currentAirAttackTime = Globals.DURATION_DASH;
        character.gravityValue = Globals.GRAVITY_DASH;
        character.rb.velocity = new Vector3(character.rb.velocity.x, 0 , character.rb.velocity.z);
        character.move = (character.forward * Globals.SPEED_DASH * Time.deltaTime);
        character.move.y = character.rb.velocity.y;
        character.rb.AddForce(character.move - character.rb.velocity, ForceMode.VelocityChange);
        character.attackPressed = false;
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (character.stateDebug) Debug.Log("Air Attacking");
        character.jumpPressed = false;
        //character.Move();
        
       
        currentAirAttackTime -= Time.deltaTime;
        character.rb.velocity = character.rb.velocity * .97f;
        if (currentAirAttackTime <= 0)
        {
            character.gravityValue = Globals.GRAVITY_NORMAL;
            character.SwitchState(character.FallingState);
            //character.rb.velocity = new Vector3(character.rb.velocity.x * .98f * Time.deltaTime, character.rb.velocity.y, character.rb.velocity.z * .98f * Time.deltaTime);
            
        }
        
        if (character.groundedPlayer)
        {
            character.gravityValue = Globals.GRAVITY_NORMAL;
            //character.rb.velocity = Vector3.zero;
            //character.rb.angularVelocity = Vector3.zero;
            character.SwitchState(character.IdleState);
        }
        character.Move(Globals.SPEED_NORMAL);
        character.ApplyGravity();
        character.Rotate();

    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {

    }

}
