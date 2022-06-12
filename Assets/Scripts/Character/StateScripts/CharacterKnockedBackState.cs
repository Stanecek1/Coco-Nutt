using UnityEngine;

public class CharacterKnockedBackState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character)
    {
        character.gravityValue = Globals.GRAVITY_NORMAL;
        character.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = character.damagedMaterial;
        
    } 

    public override void UpdateState(CharacterStateManager character)
    {
        if(character.stateDebug) Debug.Log("Knocked Back State");
        if (!character.knockedUp)
        {
            //set the veritcal velocity to 0 so we dont super jump the player
            character.playerVelocity.y = 0;
            //knock the player up
            character.playerVelocity.y += Mathf.Sqrt(character.knockBackHeight * -3.0f * character.gravityValue);
            //set knocked up to trueso we le tthe player fall back down
            character.knockedUp = true;
            //set the heightr padding to 0 so we can leave the ground
            character.heightPadding = 0;
            //the player should no longer be grounded
            character.groundedPlayer = false;
        }

        character.transform.position += (character.knockBackDirection * character.knockBackSpeed * Time.fixedDeltaTime);
        character.ApplyGravity();

        if (character.groundedPlayer)
        {
            character.knockedBack = false;
            character.jumpPressed = false;
            character.knockedUp = false;
            character.SwitchState(character.IdleState);
        }

    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {

    }
}
