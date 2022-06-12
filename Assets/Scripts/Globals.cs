using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    //GRAVITY
    public const float GRAVITY_NORMAL = -40;
    public const float GRAVITY_DASH = 0;
    public const float GRAVITY_GROUND_POUND = -200;

    //SPEED
    public const float SPEED_NORMAL = 15;
    public const float SPEED_KNOCK_BACK = 10;
    public const float SPEED_CROUCH_SLIP = 10;
    public const float SPEED_CROUCH = 5;
    public const float SPEED_DASH = 1000;

    //VERTICAL
    public const float JUMP_CROUCH_HEIGHT = 9F;
    public const float JUMP_HEIGHT = 4F;
    public const float KNOCK_BACK_HEIGHT = 9F;

    //DURATION
    public const float DURATION_DASH = .3f;
    public const float DURATION_IMMUNITY = 2f;

    //GROUND
    public const float HEIGHT = 2;
    public const float HEIGHT_PADDING = 1f;
    public const float MAX_GROUND_ANGLE = 120f;
    public const float TURN_SMOOTH_TIME = 0.04f;

    //KEYBOARD KEYS
    public const KeyCode BUTTON_JUMP = KeyCode.Space;
    public const KeyCode BUTTON_CROUCH = KeyCode.LeftShift;
    public const KeyCode BUTTON_GROUND_POUND = KeyCode.LeftShift;
    public const KeyCode BUTTON_ALLIGN_CAMERA = KeyCode.Tab;
    public const KeyCode BUTTON_INTERACT = KeyCode.E;
    public const KeyCode BUTTON_TARGET = KeyCode.LeftControl;
    public const KeyCode BUTTON_REVERSE_CAMERA = KeyCode.F1;
    

    //THROWING
    public const float SPEED_MOVE_THROW = 1100;
    public const float SPEED_STILL_THROW = 500;

    public static Material damagedMaterial = Resources.Load("Damaged", typeof(Material)) as Material;

}
