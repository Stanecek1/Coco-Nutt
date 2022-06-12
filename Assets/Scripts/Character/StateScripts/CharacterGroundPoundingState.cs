using UnityEngine;

public class CharacterGroundPoundingState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character)
    {
        if (!character.groundedPlayer)
        {
            if (character.groundPoundPressed)
            {
                character.groundPoundPressed = false;
                character.gravityValue = Globals.GRAVITY_GROUND_POUND;
                character.ApplyGravity();
            }
        }
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (character.stateDebug) Debug.Log("Ground Pounding State");
        character.ApplyGravity();
        character.jumpPressed = false;
        if (character.groundedPlayer)
        {
            character.SwitchState(character.IdleState);
        }
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {
        if (collision.gameObject.tag == "Poundable")
        {
            //destroy it
            Object.Destroy(collision.gameObject.transform.parent.gameObject);
            
        }
    }
}
