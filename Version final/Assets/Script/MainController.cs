using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

    private GlobalParameters parameters;
    private List<GameObject> fallenPins;
    private GameObject pinSaverSystem;
    private GameObject pinCleanerSystem;
    private GameObject pinCleanerTarget;
    private GameObject pinSaverTarget;
    private GameObject pinCleanerRecoverTarget;
    private List<GameObject> grabbedPins;
    private bool performPinRecovery = false;
    private int turn = 1;
    private Vector3 saverInitialPosition;
    private Vector3 cleanerInitialPosition;
    private Vector3 cleanerRecoverInitialPosition;
    private bool initialPositionsSaved = false;
    private bool pinsGrabbed = false;
    private float timer = 0.0f;

    // Use this for initialization
    void Start () {
        fallenPins = new List<GameObject>();
        grabbedPins = new List<GameObject>();
        pinSaverSystem = GameObject.Find("pin saver");
        pinCleanerSystem = GameObject.Find("pin cleaner");
        pinCleanerTarget = GameObject.Find("cleaner target");
        pinSaverTarget = GameObject.Find("saver target");
        pinCleanerRecoverTarget = GameObject.Find("cleaner recover target");
        parameters = GameObject.Find("RightLane").GetComponent<GlobalParameters>();
        parameters.initNextFrame();
    }
	
	// Update is called once per frame
	void Update () {
        // Recover the fallen Pins and count them
        if (performPinRecovery)
        {
            if(timer >= 5.0f)
            {
                // Start fallen pins accounting, by recoverying them so that they could be countable
                if (recoveryFallenPins()) {
                    turn = 1;
                    if (!parameters.IsLastFrame)
                    {
                        if (fallenPins.Count >= 10)
                            fallenPins.Clear();
                        // Go to the next frame
                        parameters.initNextRound();
                    }
                    else
                    {
                        // End the game and send the player to the menu
                        endGame();
                        sendPlayerToMenu();
                    }

                    performPinRecovery = false;
                    timer = 0.0f;
                }
            }else
            {
                timer += Time.deltaTime;
            }
            
        }
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
            obj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 73, 0), ForceMode.Impulse);

            // Tell the system to recover the pin
            performPinRecovery = true;
        }

        if (obj.tag == "Pin" && !fallenPins.Contains(obj))
        {
            /***************** Pin collision control *****************/
             
            Debug.Log("Pin felt: " + obj.name);

            // Register the fallen Pin
            fallenPins.Add(obj);

            // Increment the fallen pins of the frame's round
            parameters.incrementFallenPins();
            string rounds = "";
            foreach(int r in parameters.Rounds)
            {
                rounds += r + ";";
            }
            Debug.Log("Fallen Pins rounds: " + rounds);
        }
    }

    private float clamp(float a, float b)
    {
        return (a < b) ? b : a;
    }

    /**
     * Pulling all the fallen pins in the cave to be counted
     */
    public bool recoveryFallenPins()
    {
        GameObject cleaner = GameObject.Find("pin cleaner/cleaner recover");
        GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
        Vector3 sPos = pinSaverSystem.transform.position;
        Vector3 cPos = pinCleanerSystem.transform.position;
        Vector3 pCPos = cleaner.transform.position;
        float v = 0.75f;

        Debug.Log("Recoverying Pins: Step "+turn);
        switch (turn)
        {
            case 1: // Move the saver down to grab the remaining pins, and move the cleaner down to be placed appropriatly to clean the fallen pins
                
                if (!initialPositionsSaved)
                {
                    saverInitialPosition = pinSaverSystem.transform.position;
                    cleanerInitialPosition = pinCleanerSystem.transform.position;
                    cleanerRecoverInitialPosition = cleaner.transform.position;
                     initialPositionsSaved = true;
                }                    

                if(sPos.y > pinSaverTarget.transform.position.y)
                {
                    sPos.y -= v * Time.deltaTime;
                }

                if (cPos.y > pinCleanerTarget.transform.position.y)
                {
                    cPos.y -= v * Time.deltaTime;
                }                

                if (sPos.y <= pinSaverTarget.transform.position.y && cPos.y <= pinCleanerTarget.transform.position.y)
                {
                    sPos.y = pinSaverTarget.transform.position.y;
                    cPos.y = pinCleanerTarget.transform.position.y;
                    turn++;
                }

                break;

            case 2:// Move the saver up with the remaining pins

                // Move the saver up with the remaining pins
                // Grab the remaining Pin       
                if (!pinsGrabbed)
                {
                    foreach (Collider col in Physics.OverlapBox(pinSaverSystem.GetComponent<MeshRenderer>().bounds.center, pinSaverSystem.GetComponent<MeshRenderer>().bounds.extents))
                    {
                        GameObject pin = col.gameObject.transform.parent.parent.parent.gameObject;
                        Debug.Log("Grabbing pin " + pin.name);
                        if (pin.tag == "Pin")
                        {
                            Debug.Log("Grabbing pin " + pin.name);
                            grabbedPins.Add(pin);
                        }
                    }
                    pinsGrabbed = true;
                }
                
                // Move them up
                if(sPos.y < saverInitialPosition.y)
                {
                    sPos.y += v * Time.deltaTime;
                    foreach(GameObject pin in grabbedPins)
                    {
                        //Vector3 pPos = pin.transform.position;
                        //pPos.y += v * Time.deltaTime;
                        pin.GetComponent<Rigidbody>().useGravity = false;
                        pin.transform.SetParent(pinSaverSystem.transform);
                        //pin.transform.position = pPos;
                    }
                }else
                {
                    sPos.y = saverInitialPosition.y;
                    turn++;
                }            

                break;

            case 3://  Move the cleaner front to push fallen pins into the cave
                if (pCPos.z < pinCleanerRecoverTarget.transform.position.z)
                {
                    pCPos.z += v * Time.deltaTime;
                    cleaner.transform.position = pCPos;
                }
                else
                {
                    pCPos.z = pinCleanerRecoverTarget.transform.position.z;
                    turn++;
                }
                break;

            case 4:// Move the cleaner back 
                if (pCPos.z > cleanerRecoverInitialPosition.z)
                {
                    pCPos.z -= v * Time.deltaTime;
                    cleaner.transform.position = pCPos;
                }
                else
                {
                    pCPos.z = cleanerRecoverInitialPosition.z;
                    turn++;
                }
                break;

            case 5:// Move the saver down to replace remaining pin
                if (sPos.y > pinSaverTarget.transform.position.y)
                {
                    sPos.y -= v * Time.deltaTime;
                }
                else
                {
                    if (pinsGrabbed)
                    {
                        foreach (GameObject pin in grabbedPins)
                        {
                            //Vector3 pPos = pin.transform.position;
                            //pPos.y -= v * Time.deltaTime;
                            pin.transform.SetParent(GameObject.Find("Pins").transform);
                            pin.GetComponent<Rigidbody>().useGravity = true;
                            //pin.transform.position = pPos;
                        }
                        pinsGrabbed = false;
                    }
                    sPos.y = pinSaverTarget.transform.position.y;
                    turn++;
                }
                break;

            case 6:// Move the saver up to reach the initial position, and move the cleaner top to reach the initial position
                grabbedPins.Clear();
                if (sPos.y < saverInitialPosition.y)
                {
                    sPos.y += v * Time.deltaTime;
                }

                if (cPos.y < cleanerInitialPosition.y)
                {
                    cPos.y += v * Time.deltaTime;
                }

                if (sPos.y >= saverInitialPosition.y && cPos.y >= cleanerInitialPosition.y)
                {
                    turn++;
                    sPos.y = saverInitialPosition.y;
                    cPos.y = cleanerInitialPosition.y;
                }
                break;
        }

        pinSaverSystem.transform.position = sPos;
        pinCleanerSystem.transform.position = cPos;

        return turn > 6;
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
