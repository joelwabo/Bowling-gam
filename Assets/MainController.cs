using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

    private GlobalParameters parameters;
	// Use this for initialization
	void Start () {
        parameters = GameObject.Find("RightLane").GetComponent<GlobalParameters>();
        parameters.initNextFrame();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Manage score computation when the ball enters the cave
    void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        
        if (obj.tag == "Ball")
        {
            /***************** Ball collision control *****************/

            // Return the ball to the storage
            Debug.Log("Ball " + obj.name + " in the cave");
            obj.transform.position = GameObject.Find("RightLane/Lane/ball_recovery_vertical_tunel_001/ball return gate").transform.position;
            obj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 60, 0), ForceMode.Impulse);

            // Start fallen pins accounting, by recoverying them so that they could be countable
            recoveryFallenPins();
            if (!parameters.IsLastFrame)
            {
                // Go to the next frame
                parameters.initNextRound();
            }
            else
            {
                // End the game and send the player to the menu
                endGame();
                sendPlayerToMenu();
            }
        }

        if (obj.tag == "Pin")
        {
            /***************** Pin collision control *****************/
             
            Debug.Log("Pin felt: " + obj.name);

            // Increment the fallen pins of the frame's round
            parameters.incrementFallenPins();
        }
    }

    /**
     * Pullin all the fallen pins in the cave to be counted
     */
    public void recoveryFallenPins()
    {

    }

    /**
     * Disabling the playing area and lock pins in order to end the current game
     */
    public void endGame()
    {

    }

    /**
     * Displays a message on the playing area's screen to the player telling him to go to the menu
     */
    private void sendPlayerToMenu()
    {

    }
}
