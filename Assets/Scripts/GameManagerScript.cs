using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    //public ThirdPersonMovement player;
    public CharacterStateManager player;
    public int starsRemaining;
    public int updatedStarsRemianing;
    public Text healthText;
    public Text startCountText;
    public Text eventText;
    public int starsCollectedCount;
    // Start is called before the first frame update
    void Start()
    {
        starsCollectedCount = 0;
        Cursor.visible = false;
        starsRemaining = GameObject.FindGameObjectsWithTag("Star").Length;
        eventText.text = "Stars Remainig: " + starsRemaining;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.health;
        updatedStarsRemianing = GameObject.FindGameObjectsWithTag("Star").Length;
        //if a star has been collected
        if (updatedStarsRemianing < starsRemaining){
            //*************** make collectable pick up noise here *****************
            //increment the star count
            starsCollectedCount++;
            //update the amount of stars remaining
            starsRemaining = updatedStarsRemianing;
            //if there are more stars
            if (starsRemaining > 0)
                eventText.text = "Stars Remainig: " + starsRemaining;
            //if the last star was collected
            else
                eventText.text = "You Collected All The Stars!";
        }
        //display the stars collected count
        startCountText.text = "Stars: " + starsCollectedCount;
        startCountText.text = "Stars: " + starsCollectedCount;
    }
}
