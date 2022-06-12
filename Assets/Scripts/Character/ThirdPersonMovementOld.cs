using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThirdPersonMovementOld : MonoBehaviour
{
    public CharacterController controller;  //the Character Controller attached to this objectS
    public Transform cam;                   //the main camera in the scene
    private Vector3 playerVelocity;         //the velocity of the players vertical movement
    private Vector3 moveDir;

    //public variables
    public float speed = 6f;                //the movement speed of the character
    public float turnSmoothTime = .05f;     //the time it take to turn tg ebody from one angle to a another
    public float jumpHeight = 4.0f;         //the heigh of the player jump
    public float gravityValue = -40f;       //how much the player is effected by gravity

    //private Variables
    private bool groundedPlayer;            //if the player is on the ground or not
    private float horizontal;               //the horizontal input from the player
    private float vertical;                 //the vertical input from the player
    float turnSmoothVelocity;               //the velocity of the turn
    private bool canDoubleJump = true;      // if the player can double jump

    //graphics of the player model
    public GameObject GFX;                  //mesh renderer for GFX
    public Material idle;                   //material for the player while Idle
    public Material walk;                   //material for the player while walking
    public Material jump;                   //material for the player while jumping
    public Material doubleJump;             //material for the player while double jumping


    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //set the inputs
        setInputs();

        //if the player is touching the ground 
        if (groundedPlayer)
        {
            GFX.GetComponent<MeshRenderer>().material = idle;
            //let them double jump
            canDoubleJump = true;
            //and their y velocity is less than 0.
            if (playerVelocity.y < 0)
            {
                //set the velocity to 0
                playerVelocity.y = 0f;
            }
        }
        
        //set the horizontal (x and z) movment of the player to 0
        moveDir = Vector3.zero;

        //gets the direction the player is moving on the x and z axis
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //if the charcter is moving
        if (direction.magnitude >= 0.1f)
        {
            //if were on the ground
            if (groundedPlayer)
            {
                GFX.GetComponent<MeshRenderer>().material = walk;
            }

            //math
            float targetAngel = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngel, ref turnSmoothVelocity, turnSmoothTime);
            //the rotatoion of the charcter
            transform.rotation = Quaternion.Euler(0f, angel, 0f);

            //the horizontal movement and 
            moveDir = Quaternion.Euler(0f, targetAngel, 0f) * Vector3.forward;
        }


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump"))
        {
            //if the player is on the ground
            if (groundedPlayer)
            {
                GFX.GetComponent<MeshRenderer>().material = jump;
                //let them jump
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                //they are no longer on the ground
                groundedPlayer = false;
            }
            // if they are in the air
            else
            {
                //and if they can double jump
                if (canDoubleJump)
                {
                    GFX.GetComponent<MeshRenderer>().material = doubleJump;
                    //they can no longer jump
                    canDoubleJump = false;
                    //reset their vertical velocity
                    playerVelocity.y = 0;
                    // and make them jump
                    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                }
            }
        }

        //add gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        //move the character in all directions the user inputs
        controller.Move(moveDir * speed * Time.deltaTime + playerVelocity * Time.deltaTime);
    }

    //function that gets all the user inputs for later use
    void setInputs()
    {
        groundedPlayer = controller.isGrounded;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Star"){
            Destroy(other.gameObject);
        }


    }
}
