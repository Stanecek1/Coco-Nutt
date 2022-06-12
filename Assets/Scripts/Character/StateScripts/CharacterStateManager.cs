using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterStateManager : MonoBehaviour
{

    CharacterBaseState currentState;

    public bool stateDebug;

    public List<GameObject> targets = new List<GameObject>();

    public CharacterIdleState IdleState = new CharacterIdleState();
    public CharacterWalkingState WalkingState = new CharacterWalkingState();
    public CharacterJumpingState JumpingState = new CharacterJumpingState();
    public CharacterDoubleJumpingState DoubleJumpState = new CharacterDoubleJumpingState();
    public CharacterCrouchingState CrouchingState = new CharacterCrouchingState();
    public CharacterGroundPoundingState GroundPoundingState = new CharacterGroundPoundingState();
    public CharacterDamagedState DamagedState = new CharacterDamagedState();
    public CharacterAirAttackingState AirAttackingState = new CharacterAirAttackingState();
    public CharacterCrouchJumpState CrouchJumpState = new CharacterCrouchJumpState();
    public CharacterKnockedBackState KnockedBackState = new CharacterKnockedBackState();
    public CharacterFallingState FallingState = new CharacterFallingState();

    public Transform cam;                    //the main camera in the scene
    public CinemachineFreeLook freeLookCam;  //the cinemachine camrea

    //materials
    public Material damagedMaterial;
    public Material normalMaterial;

    //buttons
    private KeyCode jumpBtn = Globals.BUTTON_JUMP;                   //jump button
    private KeyCode crouchBtn = Globals.BUTTON_CROUCH;               //crouch button
    private KeyCode groundPoundBtn = Globals.BUTTON_GROUND_POUND;    //ground pound button
    private KeyCode allignCameraBtn = Globals.BUTTON_ALLIGN_CAMERA;  //alligns the camear with the players direction
    private KeyCode interact = Globals.BUTTON_INTERACT;              //button to interact with items
    private KeyCode target = Globals.BUTTON_TARGET;                 //button to lock on targets

    //nuttons pressed
    public bool interactPressed;                                     //if the interact button has just been pressed
    public bool attackPressed;                                       //if the attack button was pressed
    public bool jumpPressed;                                         //if the jump button has been pressed
    public bool groundPoundPressed;                                  //if the player pressed the ground pound button
    public bool crouchPressed;                                       //if the player is pressing left cntrl
    public bool targetPressed;                                       //if the player has pressed the target button

    //layers
    public LayerMask ground;                                        //the layer we can walk on
    public LayerMask groundPoundLayer;
    public LayerMask slip;                                          //the layer will slip and fall on
    public LayerMask damageLayer;                                   //the layer that damages the player

    //movement data
    public float hor;                                               //the horizontal input by the player
    public float ver;                                               //the veritcal input by the player
    public float speed = Globals.SPEED_NORMAL;                      //speed the character will move at
    private float turnSmoothVelocity;                               //the velocity of the turn
    private float turnSmoothTime = Globals.TURN_SMOOTH_TIME;        //the time it take to turn the body from one angle to a another
    public Vector3 forward;                                         //the direction the player will move
    private Quaternion targetRotation;                              //the rotation the player will rotate to
    public Rigidbody rb;                                            //rigidbody of the character
    public Vector3 move;                                            //all the directions the player is moving in   
    public bool horizontalMovement;                                 //if the the player is moving in the horizontal direction

    //jumping data
    public Vector3 playerVelocity;                                  //the velocity of the players vertical movement
    private RaycastHit hitInfo;                                     //for detecting the ground
    private float height = Globals.HEIGHT;                          //the height of the player
    public float heightPadding = Globals.HEIGHT_PADDING;            //the padding to detect the ground
    private float maxGroundAngle = Globals.MAX_GROUND_ANGLE;        //the max angle the player can walk up
    private float groundAngle;                                      //the current angle the player is on
    public bool previouslyMoving;                                   //if the player was moving last frame
    private float currentSpeed = 0;                                 //current speed of the player
    public bool groundedPlayer = false;                             //if the player is on the ground or not
    public bool canDoubleJump;                                      // if the player can double jump
    private float jumpHeight = Globals.JUMP_HEIGHT;                 //the heigh of the player jump
    public float gravityValue = Globals.GRAVITY_NORMAL;             //how much the player is effected by gravity

    //crouch data
    public float crouchSlipSpeed = Globals.SPEED_CROUCH_SLIP;       //speed the player moves at if crouchingSliping
    public float crouchSpeed = Globals.SPEED_CROUCH;                //speed the player moves at when crouching      
    public float crouchJumpHeight = Globals.JUMP_CROUCH_HEIGHT;     //height of the crouch jump

    //damage data
    public int health;                                              //current health of the player
    public bool knockedBack;                                        //if the player is being knocked back from damage
    public bool knockedUp;                                          //if the player has been knocked up
    public float knockBackHeight = Globals.KNOCK_BACK_HEIGHT;       //how high the player is raised when hit
    public float knockBackSpeed = Globals.SPEED_KNOCK_BACK;         //the speed at which the player is knocked back;
    public float currentImmunityTime = Globals.DURATION_IMMUNITY;   //the amount of immune time left
    public bool immune = false;                                     //if the player is immune
    public Vector3 knockBackDirection;                              //the direction the player will knock back
                                                                    //
    bool once = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        currentState = IdleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        GetInputs();
        if (targetPressed) Target();

    }

    private void Target()
    {
        

        if (targets.Count > 0 && once == false)
        {
            
        }
        

    }
    private void FixedUpdate()
    {
        //calculate the direction of the player
        CalculateDirection();
        //calculatte the forward of the player
        CalculateForward();
        //calculate the angle of the ground were on
        CalculateGroundAngle();
        Immunity();
        currentState.UpdateState(this);
    }

    public void SwitchState(CharacterBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void CheckGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, height + 3, slip))
        {
            groundedPlayer = false;
            canDoubleJump = false;
        }
        else if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, height + heightPadding, ground) || Physics.Raycast(transform.position, Vector3.down, out hitInfo, height + heightPadding, groundPoundLayer) || (Physics.Raycast(transform.position, Vector3.down, out hitInfo, height + heightPadding, damageLayer)))
        {
            groundedPlayer = true;                      //were on the ground
            canDoubleJump = true;                       //let them double jump
            heightPadding = Globals.HEIGHT_PADDING;     //reset our height padding
            groundPoundPressed = false;                 //we cant groudn piund if were on the ground
            gravityValue = Globals.GRAVITY_NORMAL;      //set gravity back to normal
        }
        else
        {
            groundedPlayer = false;
        }
    }

    //calcutates the forawrd of the player
    private void CalculateForward()
    {
        //if the player is grounded
        if (!groundedPlayer)
        {
            // the forward is the forward of the player model
            forward = transform.forward;
            return;
        }
        //else, the forward is the cross vector of the normal and transform right
        forward = Vector3.Cross(hitInfo.normal, -transform.right);
    }
    //calualtes the ground angel the in front of the player
    private void CalculateGroundAngle()
    {
        //if were nto grounded
        if (!groundedPlayer)
        {
            //the ground agle is 90 degrees
            groundAngle = 90;
            return;
        }
        //if we are grounded, the ground agle to determined by the angle between the hit info (stright down) normal and transform foraward
        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    private void CalculateDirection()
    {
        Vector3 direction = new Vector3(hor, 0f, ver).normalized;

        //if were moving in any direction
        if (direction.magnitude >= 0.1f)
        {
            float targetAngel = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngel, ref turnSmoothVelocity, turnSmoothTime);
            //the rotatoion of the charcter
            targetRotation = Quaternion.Euler(0f, angel, 0f);
        }
    }

    public void ApplyGravity()
    {

        //set the player velocity to account for gravity
        if (groundedPlayer)
        {
            playerVelocity.y = 0;
        }
        else
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        transform.position += (playerVelocity * Time.deltaTime);
        //rb.MovePosition(transform.position + playerVelocity);
    }

    //moves the player by a specified speed
    public void Move(float Speed)
    {
        currentSpeed = Speed;
        //if the ground angle is to high, we can move
        if (groundAngle >= maxGroundAngle) return;
        //moves the player by the calulated forward and specified speed
        move = (forward * Speed * Time.deltaTime);
        //rb.velocity = move;
        //rb.MovePosition(transform.position + move);
        transform.position += move;
    }

    public void Rotate()
    {
        transform.rotation = targetRotation;
    }

    public void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //jumpPressed is now false after we jump so we dont keep jumping
        jumpPressed = false;
        //they are no longer on the ground
        groundedPlayer = false;
        heightPadding = 0;
        //rb.AddForce(3000f * Vector3.up);
        //playerVelocity.y += gravityValue * Time.deltaTime;
        //rb.AddForce(transform.up * 6000);
        //rb.MovePosition(transform.position + playerVelocity);

    }

    private void Damage()
    {
        if (!immune)
        {
            //liftedObject = null;
            //holdingObject = false;
            health--;
            groundPoundPressed = false;
            //set immune to true so we can let the player recover;
            immune = true;
            //set knock back to true;
        }
    }

    public void Immunity()
    {
        if (immune)
        {
            if (currentImmunityTime > 0)
            {
                currentImmunityTime -= Time.deltaTime;
            }
            else
            {
                immune = false;
                gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = normalMaterial;
                currentImmunityTime = Globals.DURATION_IMMUNITY;
            }
        }
    }


    private void GetInputs()
    {
        if (Input.GetKey(target))
        {
            targetPressed = true;
        }
        else
        {
            targetPressed = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            attackPressed = true;
        }
        //a and d key input
        hor = Input.GetAxisRaw("Horizontal");
        // w and s key input
        ver = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(hor) < 1 && Mathf.Abs(ver) < 1)
        {
            horizontalMovement = false;
        }
        else
        {
            horizontalMovement = true;
        }
        //space key input
        if (Input.GetKeyDown(jumpBtn))
        {
            jumpPressed = true;
        }

        //if the crouch button has been realeased
        if (Input.GetKeyUp(crouchBtn))
        {
            //reset the crouch slipspeed variable
            crouchSlipSpeed = Globals.SPEED_CROUCH_SLIP;
        }
        //space key input
        if (Input.GetKeyDown(groundPoundBtn))
        {
            groundPoundPressed = true;
        }

        //space key input
        if (Input.GetKeyDown(allignCameraBtn))
        {
            freeLookCam.m_RecenterToTargetHeading.m_enabled = true;
            freeLookCam.m_YAxisRecentering.m_enabled = true;
            freeLookCam.m_XAxis.m_InputAxisName = null;
            freeLookCam.m_YAxis.m_InputAxisName = null;
        }
        //space key input
        if (Input.GetKeyUp(allignCameraBtn))
        {
            freeLookCam.m_RecenterToTargetHeading.m_enabled = false;
            freeLookCam.m_YAxisRecentering.m_enabled = false;
            freeLookCam.m_XAxis.m_InputAxisName = "Mouse X";
            freeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        //left cntrl input
        crouchPressed = Input.GetKey(crouchBtn);
        //if the crouch button has been realeased
        if (Input.GetKeyUp(crouchBtn))
        {
            //reset the crouch slipspeed variable
            //crouchSlipSpeed = Globals.SPEED_CROUCH_SLIP;
        }

        if (Input.GetKeyDown(interact))
        {
            interactPressed = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == 13 && collision.gameObject.tag != "Projectile" && !immune)
        {
            //get the direction of the knockback by finding the angle between the player and damaging object
            knockBackDirection = transform.position - collision.transform.position;
            //noralize it
            knockBackDirection = knockBackDirection.normalized;
            //set the y to z to prevent moving up or down
            knockBackDirection.y = 0;
            Damage();
            SwitchState(KnockedBackState);
            return;
        }

        currentState.OnCollisionEnter(this, collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        //if we collide with a collectable
        if (other.gameObject.tag == "Star")
        {
            //destroy it
            Destroy(other.gameObject);
        }

        //if we collide with a collectable
        if (other.gameObject.tag == "Health")
        {
            //destroy it
            Destroy(other.gameObject);
            //restore players health
            health++;
        }

        //if the player collides with something in the damage layer
        if (other.gameObject.layer == 13)
        {
            //if the player is not immune
            if (!immune)
            {
                //if the player was hit by a projectile
                if (other.gameObject.tag == "Projectile")
                {
                    //get the direction of the knockback
                    knockBackDirection = other.gameObject.GetComponent<Projectile>().dir;
                    //noralize it
                    knockBackDirection = knockBackDirection.normalized;
                    //set the y to z to prevent moving up or down
                    knockBackDirection.y = 0;
                    Destroy(other.gameObject);
                    Damage();
                    SwitchState(KnockedBackState);
                    
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.layer == 13 && collision.gameObject.tag != "Projectile" && !immune)
        {
            //get the direction of the knockback by finding the angle between the player and damaging object
            knockBackDirection = transform.position - collision.transform.position;
            //noralize it
            knockBackDirection = knockBackDirection.normalized;
            //set the y to z to prevent moving up or down
            knockBackDirection.y = 0;
            Damage();
            SwitchState(KnockedBackState);
        }

        //if (collision.gameObject.layer == 14 && interacting)
        //{
        //    if (collision.gameObject.tag == "Liftable" && !holdingObject && !throwing)
        //    {
        //        interacting = false;
        //        holdingObject = true;
        //        liftedObject = collision.gameObject;
        //    }
        //}
    }

}
