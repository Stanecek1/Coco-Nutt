using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Transform cam;                    //the main camera in the scene
    public CinemachineFreeLook freeLookCam;  //the cinemachine camrea

    public int health;                       //the health of the player


    private List<GameObject>  collidedObjects = new List<GameObject>();
    //buttons the player can input
    private KeyCode jumpBtn = Globals.BUTTON_JUMP;                  //jump button
    public KeyCode crouchBtn;                                       //crouch button
    public KeyCode groundPoundBtn;                                  //ground pound button
    public KeyCode allignCameraBtn;                                 //alligns the camear with the players direction
    public KeyCode interact;                                        //button to interact with items

    //interact shit
    private bool interacting;                                       //if the interact button has just been pressed
    private GameObject liftedObject;                                //object being lifted by the player
    private bool holdingObject;                                     //if the player is holding anb object
    private bool throwing;                                          //if the player is throwing an object

    //attack shit
    private bool attackPressed;                                     //if the attack button was pressed
    private bool airAttacking;                                      //if the player is currently air attacking
    private float airAttackSpeed = Globals.SPEED_DASH;              //the force at which the player is pushed when dashing
    private bool wasAirAttacking = false;                           //if the plyaer was previously air attacking
    
    //movement shit
    private float hor;                                              //the horizontal input by the player
    private float ver;                                              //the veritcal input by the player
    private float speed = Globals.SPEED_NORMAL;                     //speed the character will move at
    private float turnSmoothVelocity;                               //the velocity of the turn
    private float turnSmoothTime = Globals.TURN_SMOOTH_TIME;        //the time it take to turn the body from one angle to a another
    private Vector3 forward;                                        //the direction the player will move
    private Quaternion targetRotation;                              //the rotation the player will rotate to
    private Rigidbody rb;                                           //rigidbody of the character
    private Vector3 move;                                           //all the directions the player is moving in   
    public LayerMask ground;                                        //the layer we can walk on
    public LayerMask slip;                                          //the layer will slip and fall on
    private RaycastHit hitInfo;                                     //for detecting the ground
    private float height = Globals.HEIGHT;                          //the height of the player
    private float heightPadding = Globals.HEIGHT_PADDING;           //the padding to detect the ground
    private float maxGroundAngle = Globals.MAX_GROUND_ANGLE;        //the max angle the player can walk up
    private float groundAngle;                                      //the current angle the player is on
    private bool previouslyMoving;                                  //if the player was moving last frame
    private float currentSpeed = 0;

    //jumping shit
    private bool jumping = false;                                   //if the player is jumping
    private bool sliping = false;                                   //if the player is slipping
    private bool groundPounding = false;                            //if the player is ground pounding
    private bool jumpPressed = false;                               //if the jump button has been pressed
    private Vector3 playerVelocity;                                 //the velocity of the players vertical movement
    private bool groundedPlayer = false;                            //if the player is on the ground or not
    private bool canDoubleJump = false;                             // if the player can double jump
    private float jumpHeight = Globals.JUMP_HEIGHT;                 //the heigh of the player jump
    private float gravityValue = Globals.GRAVITY_NORMAL;            //how much the player is effected by gravity
    private bool groundPoundPressed = false;                        //if the player pressed the ground pound button

    //crouching shit
    private bool crouchPressed;                                     //if the player is pressing left cntrl
    private bool crouching;                                         //if the player is crouching
    private bool crouchJumping;                                     //if the player is crouch jumping
    private float crouchSlipSpeed = Globals.SPEED_CROUCH_SLIP;      //speed the player moves at if crouchingSliping
    private float crouchSpeed = Globals.SPEED_CROUCH;               //speed the player moves at when crouching      
    private float crouchJumpHeight = Globals.JUMP_CROUCH_HEIGHT;    //height of the crouch jump
    
    //Graphics shit
    public Material idleMaterial;           //material for the player while idleMaterial
    public Material walkMaterial;           //material for the player while walkMaterialing
    public Material jumpMaterial;           //material for the player while jumping
    public Material doubleJumpMaterial;     //material for the player while double jumping
    public Material fallingMaterial;        //material for the player while falling 
    public Material slipMaterial;           //material for the player while falling 
    public Material crouchMatrial;          //material for the player when crouching
    public Material crouchSlipMatrial;      //material for the player when crouching
    public Material crouchWalkMatrial;      //material for the player when crouching
    public Material crouchJumpMaterial;     //material for the player when crouch jumping
    public Material groundPoundMaterial;    //material for the player when ground pounding
    public Material damagedMaterial;        //material for the player when damaged
    public Material airAttackMaterial;      //material for air attack
    public GameObject GFX;                  //mesh renderer for GFX

    //taking damage shit
    private bool knockedBack;                                               //if the player is being knocked back from damage
    private bool knockedUp;                                                 //if the player has been knocked up
    private float knockBackHeight = Globals.KNOCK_BACK_HEIGHT;              //how high the player is raised when hit
    private float knockBackSpeed = Globals.SPEED_KNOCK_BACK;                //the speed at which the player is knocked back;
    private float currentImmunityTime = Globals.DURATION_IMMUNITY;          //the amount of immune time left
    private bool immune = false;                                            //if the player is immune
    private Vector3 knockBackDirection;                                     //the direction the player will knock back    
    public LayerMask damageLayer;                                           //the layer that damages the player
    private bool damagePlayer;

    private void Start()
    {   
        //gets the rigidbody of the character
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //check if the player is grounded
        CheckGrounded();
        //get the inputs from the player
        GetInputs();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        bool inter = false;
        foreach (var obj in collidedObjects)
        {
            if (obj != null)
            {
                if (obj.layer == 14)
                {
                    inter = true;
                }
            }
            
        }

        if (inter == false && !holdingObject)
        {
            interacting = false;
        }
        //calculate the direction of the player
        CalculateDirection();
        //calculatte the forward of the player
        CalculateForward();
        //calculate the angle of the ground were on
        CalculateGroundAngle();
        //modify verticle velocity if we are jumping
        Damage();
        Immunity();
        //if the player is being knocked back, do the knock back commands
        if (KnockBack()) return;

        //if the player is crouching do crouch commands instead of walk commands
        if (Crouch()) return;

        //if the player is touching the ground 
        if (groundedPlayer)
        {
            //set material of character to idleMaterialMaterial
            GFX.GetComponent<MeshRenderer>().material = idleMaterial;
        }
        //if the palayer is not touching the ground
        else
        {
            if (AirAttack()) return;
            //and the player is not jumping or crouch jumping
            if ((!jumping && !crouchJumping) || wasAirAttacking){
                //then the player is falling
                GFX.GetComponent<MeshRenderer>().material = fallingMaterial;
                heightPadding = 0;
            }
            //Debug.Log(airAttacking);
            
            
        }
        Jump();
        //modify gravity and some other values if the player is ground pounding
        GroundPound();
        //apply gravity to the player
        ApplyGravity();
        //draw debig lines
        DrawDebugLines();

        if (holdingObject)
        {
            if (interacting)
            {
                throwing = true;
                if (currentSpeed != 0)
                {
                    liftedObject.GetComponent<Rigidbody>().AddForce(forward * Globals.SPEED_MOVE_THROW * Time.deltaTime, ForceMode.VelocityChange);
                }

                else
                {
                    liftedObject.GetComponent<Rigidbody>().AddForce(forward * Globals.SPEED_STILL_THROW * Time.deltaTime, ForceMode.VelocityChange);
                }
                liftedObject = null;
                holdingObject = false;
            }
            //we are holding an object
            else
            {
                liftedObject.gameObject.transform.rotation = targetRotation;
                liftedObject.gameObject.transform.position = gameObject.transform.position + new Vector3(0, GFX.GetComponent<MeshFilter>().mesh.bounds.size.y + .5f,0);
                throwing = false;
            }
        }
        //if there is no movement input
        if (Mathf.Abs(hor) < 1 && Mathf.Abs(ver) < 1)
        {
            currentSpeed = 0;
            previouslyMoving = false;
            return;
        }
        
        //rotate the player
        Rotate();
        //move the player
        Move(speed);
        previouslyMoving = true;

        //if were on the ground
        if (groundedPlayer)
        {
            GFX.GetComponent<MeshRenderer>().material = walkMaterial;
        }

    }
    
    //function that checks if were grounded
    private void CheckGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, height + 3, slip))
        {
            groundedPlayer = false;
            canDoubleJump = false;
            GFX.GetComponent<MeshRenderer>().material = slipMaterial;
            sliping = true;
        }
        else if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, height + heightPadding, ground) || (Physics.Raycast(transform.position, Vector3.down, out hitInfo, height + heightPadding, damageLayer)))
        {
            groundedPlayer = true;                      //were on the ground
            canDoubleJump = true;                       //let them double jump
            jumping = false;                            //were not jumping
            crouchJumping = false;                      //were not crouch jumping
            heightPadding = Globals.HEIGHT_PADDING;     //reset our height padding
            groundPoundPressed = false;                 //we cant groudn piund if were on the ground
            groundPounding = false;                     //were not ground pounding if were on the ground
            gravityValue = Globals.GRAVITY_NORMAL;      //set gravity back to normal
            airAttacking = false;                       //we catn air attack if were on the ground
            wasAirAttacking = false;                    //we are no longer air attacking or falling

            //if were on the ground right after slipping
            if (sliping)
            {
                rb.velocity = Vector3.zero;             //stop the slipping
                sliping = false;                        //and set slipping to false
            }
        }
        else{
            groundedPlayer = false;
        }
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

    private void GroundPound()
    {
        if (!groundedPlayer && !sliping)
        {
            if(groundPoundPressed)
            {
                GFX.GetComponent<MeshRenderer>().material = groundPoundMaterial;
                gravityValue = Globals.GRAVITY_GROUND_POUND;
                groundPounding = true;
            }
        }
    }

    public void Immunity()
    {
        if(immune)
        {
            if (currentImmunityTime > 0)
            {
                currentImmunityTime -= Time.deltaTime;
            }
            else
            {
                immune = false;
                currentImmunityTime = Globals.DURATION_IMMUNITY;
            }
        }
    }

    private void Jump()
    {
        // Changes the height position of the player..
        if (jumpPressed)
        {
            //if the player is on the ground
            if (groundedPlayer)
            {
                //set material to jump material
                GFX.GetComponent<MeshRenderer>().material = jumpMaterial;
                //let them jump
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                //jumpPressed is now false after we jump so we dont keep jumping
                jumpPressed = false;
                //they are no longer on the ground
                groundedPlayer = false;
                heightPadding = 0;
                //character is jumping
                jumping = true;
            }
            // if they are in the air
            else
            {
                //and if they can double jump
                if (canDoubleJump)
                {
                    //set material to double jump material
                    GFX.GetComponent<MeshRenderer>().material = doubleJumpMaterial;
                    //they can no longer jump
                    canDoubleJump = false;
                    //jumpPressed is now false after a double jump so we dont keep jumping
                    jumpPressed = false;
                    //set the vertical velocity to 0 before we jump again so we dont make a super double jump
                    playerVelocity.y = 0;
                    // and make them jump
                    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                    //character is jumping
                    jumping = true;
                }
            }
            //set jumppressed to false if the jump button has been pressed but we have allready double jumped
            jumpPressed = false;
        }
    }

    private bool AirAttack()
    {
        //if we are in the air and the player hits the attack button, and were not allready attacking
        if (!groundedPlayer && attackPressed && !airAttacking)
        {
            airAttacking = true;
            attackPressed = false;
            move = (forward * airAttackSpeed * Time.deltaTime);
            move.y = rb.velocity.y;
            rb.AddForce(move - rb.velocity, ForceMode.VelocityChange);
            GFX.GetComponent<MeshRenderer>().material = airAttackMaterial;
            gravityValue = Globals.GRAVITY_DASH;
            StartCoroutine(ExecuteAfterTime(Globals.DURATION_DASH));
            canDoubleJump = false;
            return true;

            //start timer
        }
        airAttacking = false;
        attackPressed = false;
        return false;
        ////if we are in the air and the player hits the attack button, and were not allready attacking
        //if (!groundedPlayer && attackPressed && !airAttacking)
        //{
        //    airAttacking = true;
        //    attackPressed = false;
        //    //start timer
        //}
        //if (airAttacking && airAttackSpeed > speed)
        //{
        //    Move(airAttackSpeed);
        //    Rotate();
        //    airAttackSpeed -= 3;
        //    GFX.GetComponent<MeshRenderer>().material = airAttackMaterial;
        //    return true;
        //}
        //airAttackSpeed = maxAirAttackSpeed;
        //if (airAttacking == true)
        //{
        //    wasAirAttacking = true;
        //}
        //airAttacking = false;
        //return false;
    }

    public bool KnockBack()
    {
        if (knockedBack)
        {
            //change to the damaged material
            GFX.GetComponent<MeshRenderer>().material = damagedMaterial;
            //if we havent knocked the player up yet
            if (!knockedUp){
                //set the veritcal velocity to 0 so we dont super jump the player
                playerVelocity.y = 0;
                //knock the player up
                playerVelocity.y += Mathf.Sqrt(knockBackHeight * -3.0f * gravityValue);
                //set knocked up to trueso we le tthe player fall back down
                knockedUp = true;
                //set the heightr padding to 0 so we can leave the ground
                heightPadding = 0;
                //the player should no longer be grounded
                groundedPlayer = false;
            }
            ApplyGravity();
            //move the player back in the direction of the knockback
            transform.position += (knockBackDirection* knockBackSpeed * Time.fixedDeltaTime);
            //if the player has landed, the knock back is over
            if (groundedPlayer)
            {
                knockedBack = false;
                jumpPressed = false;
                knockedUp = false;
            }
            //the player is knocked back
            return true;
        }
        //the player is not or no longer knocked back
        return false;
    }

    public bool Crouch()
    {
        //if the player is croching, we do a bunch of different shit
        if (crouchPressed)
        {
            if (groundedPlayer)
            {
                //we are crouching
                crouching = true;
                //apply crouch material
                GFX.GetComponent<MeshRenderer>().material = crouchMatrial;
                //if we were moving before we crouched
                if (previouslyMoving && crouchSlipSpeed > 0)
                {
                    //make the player crouch slip
                    Move(crouchSlipSpeed);
                    //reduce the speed so they slow down as they slip
                    crouchSlipSpeed -= .2f;
                    //let them rotate
                    Rotate();
                    GFX.GetComponent<MeshRenderer>().material = crouchSlipMatrial;
                }
                //if we werent moving before we crouched
                else
                {
                    //set previosuly moving to false
                    previouslyMoving = false;
                    //if the hor or ver inputs arent being pressed
                    if (Mathf.Abs(hor) < 1 && Mathf.Abs(ver) < 1)
                    {
                        //modify verticle velocity if we are jumping
                        CrouchJump();
                        //apply gravity to the player
                        ApplyGravity();
                        return true;
                    }
                    //let the player crouch walk
                    Move(crouchSpeed);
                    //assign the crouch walk material
                    GFX.GetComponent<MeshRenderer>().material = crouchWalkMatrial;
                }

                //modify verticle velocity if we are jumping
                CrouchJump();
                //apply gravity to the player
                ApplyGravity();
                Rotate();
                //Move(crouchSpeed);
                return true;
            }
        }
        // were not crouching
        crouching = false;
        return false;
    }

    public void CrouchJump()
    {
        if (jumpPressed)
        {
            if (groundedPlayer)
            {
                //set material to crouch jump material
                GFX.GetComponent<MeshRenderer>().material = crouchJumpMaterial;
                //make the player courch jump
                playerVelocity.y += Mathf.Sqrt(crouchJumpHeight * -3.0f * gravityValue);
                //jumpPressed is now false after we jump so we dont keep jumping
                jumpPressed = false;
                //they are no longer on the ground
                groundedPlayer = false;
                heightPadding = 0;
                canDoubleJump = false;
                crouching = false;
                //character is jumping
                crouchJumping = true;
            }
        }
    }

    private void ApplyGravity()
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
    }

    //moves the player by a specified speed
    public void Move(float Speed)
    {
        currentSpeed = Speed;
        //if the ground angle is to high, we can move
        if (groundAngle >= maxGroundAngle) return;
        //moves the player by the calulated forward and specified speed
        move = (forward * Speed * Time.deltaTime);
        transform.position += move;
    }

    //ratates the player in the target rotation
    void Rotate()
    {
        transform.rotation = targetRotation;
    }

    private void Damage()
    {
        if (damagePlayer && !immune)
        {
            liftedObject = null;
            holdingObject = false;
            Debug.Log(holdingObject);
            health--;
            //prevent ground pounding if the player was groundpounding before taking damage
            groundPounding = false;
            groundPoundPressed = false;
            //turn gravity back to normal if the player was groundpounding
            gravityValue = Globals.GRAVITY_NORMAL;
            //set immune to true so we can let the player recover;
            immune = true;
            //set knock back to true;
            knockedBack = true;
            damagePlayer = false;
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

    //function that gets the inputs from the player
    private void GetInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attackPressed = true;
        }
        //a and d key input
        hor = Input.GetAxisRaw("Horizontal");
        // w and s key input
        ver = Input.GetAxisRaw("Vertical");
        //space key input
        if (Input.GetKeyDown(jumpBtn))
        {
            jumpPressed = true;
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
            crouchSlipSpeed = Globals.SPEED_CROUCH_SLIP;
        }

        if (Input.GetKeyDown(interact))
        {
            interacting = true;
        }
        else
        {
            //interacting = false;
        }
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
                    damagePlayer = true;
                    //set knock back to true;
                    knockedBack = true;
                    //get the direction of the knockback
                    knockBackDirection = other.gameObject.GetComponent<Projectile>().dir;
                    //noralize it
                    knockBackDirection = knockBackDirection.normalized;
                    //set the y to z to prevent moving up or down
                    knockBackDirection.y = 0;
                }
                //turn gravity back to normal if the player was groundpounding
                gravityValue = Globals.GRAVITY_NORMAL;
            }
            if (other.gameObject.tag == "Projectile")
            {
                Destroy(other.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        collidedObjects.Add(collision.gameObject);
        //if we collide with something that can be destroyed by ground pounding 
        if (collision.gameObject.tag == "Poundable")
        {
            //and were ground pounding
            if (groundPounding)
            {
                //destroy it
                Destroy(collision.gameObject.transform.parent.gameObject);
            }
        }
        //if the player collides with something in the damage layer
        if (collision.gameObject.layer == 13 && collision.gameObject.tag != "Projectile")
        {
            //set damage player to true so we can damage the player next time they aremt immune
            damagePlayer = true;
            //get the direction of the knockback by finding the angle between the player and damaging object
            knockBackDirection = transform.position - collision.transform.position;
            //noralize it
            knockBackDirection = knockBackDirection.normalized;
            //set the y to z to prevent moving up or down
            knockBackDirection.y = 0;
        }

        if (collision.gameObject.layer == 14 && interacting)
        {
            if (collision.gameObject.tag == "Liftable" && !holdingObject && !throwing)
            {
                holdingObject = true;
                liftedObject = collision.gameObject;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collidedObjects.Remove(collision.gameObject);

        if (collision.gameObject.tag == "Liftable")
        {
            throwing = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.layer == 13 && collision.gameObject.tag != "Projectile")
        {
            damagePlayer = true;
            //get the direction of the knockback by finding the angle between the player and damaging object
            knockBackDirection = transform.position - collision.transform.position;
            //noralize it
            knockBackDirection = knockBackDirection.normalized;
            //set the y to z to prevent moving up or down
            knockBackDirection.y = 0;
        }

        if (collision.gameObject.layer == 14 && interacting)
        {
            if (collision.gameObject.tag == "Liftable" && !holdingObject && !throwing)
            {
                interacting = false;
                holdingObject = true;
                liftedObject = collision.gameObject;
                //liftedObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    private void DrawDebugLines(){
        Debug.DrawLine(transform.position, transform.position + forward * height * 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        gravityValue = Globals.GRAVITY_NORMAL;
        GFX.GetComponent<MeshRenderer>().material = fallingMaterial;
        airAttacking = false;

    }
}
