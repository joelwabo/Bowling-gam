using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballController : MonoBehaviour {

    public bool useControllers = false;
    
    public float playerForce;
    private Vector3[] positions = new Vector3[3];
    private float[] times = new float[3];

    private Camera cam;
    private GameObject hand;
    private GameObject pickedBall = null;
    private bool ballPickedUp = false;
    private bool testing = false;
    

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        hand = GameObject.Find("hand");
        playerForce = 200;
    }
	
	// Update is called once per frame
	void Update () {
              
        if (useControllers)
        {
            /*********************************************
             ******* HTC Vive controllers usage***********
             *********************************************/
            pickupBallWithController();
        }
        else
        {
            /*********************************************
             *************** Keyboard usage***************
             *********************************************/
            /*if (!testing)
            {
                GameObject.Find("ball (1)").GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 6.0f), ForceMode.VelocityChange);
                testing = true;
            }*/
            // Picking up the ball
            pickupOrReleaseBallWithMouse();
            if(pickedBall != null)
            {
                //pickedBall.transform.position = hand.transform.position;
                pickedBall.GetComponent<Rigidbody>().isKinematic = true;
                pickedBall.transform.SetParent(hand.transform);
            }

            // Throwing the ball
            throwBallWithMouse();

            // For camera view purpose
            float tiltAroundY = Input.GetAxis("Horizontal");
            float tiltAroundX = Input.GetAxis("Vertical");
             //cam.transform.LookAt(new Vector3(hand.transform.position.x, hand.transform.position.y, 0.0f));
            cam.transform.RotateAround(cam.transform.position, new Vector3(0, 1, 0), tiltAroundY);
            cam.transform.RotateAround(cam.transform.position, new Vector3(1, 0, 0), -tiltAroundX);
        }

        // Update the measures
        updateMeasurements();
	}

    /**
     * Picking up or release the ball using mouse
     */
    private void pickupOrReleaseBallWithMouse()
    {
        if (Input.GetMouseButtonUp(1))
        {            
            if (!ballPickedUp)
            {
                /* Pick it up */
                pickupBallWithMouse();
                
            }else
            {
                /* Release it */
                releaseBallWithMouse();
            }
            
        }
    }

    /**
     * Pick up the ball using mouse
     */
    private void pickupBallWithMouse()
    {
        Ray ray = cam.ScreenPointToRay(hand.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(hand.transform.position, ray.direction, out hit, Mathf.Infinity, 1 << transform.gameObject.layer))
        {
            Debug.Log("Hit: " + hit.collider.tag);
            if (hit.collider.tag == "Ball")
            {
                ballPickedUp = true;
                pickedBall = hit.transform.gameObject;
                pickedBall.GetComponent<Rigidbody>().freezeRotation = true;
            }
        }
    }

    /**
     * Relase the ball using mouse
     */
    private void releaseBallWithMouse()
    {
        if (pickedBall != null)
        {
            pickedBall.GetComponent<Rigidbody>().freezeRotation = false;
            ballPickedUp = false;
            pickedBall.GetComponent<Rigidbody>().isKinematic = false;
            pickedBall.transform.SetParent(GameObject.Find("Balls").transform);
            pickedBall = null;
        }
    }


    /**
     * Throwing the ball using mouse
     */
    private void throwBallWithMouse()
    {        
        if (Input.GetMouseButtonUp(0) && ballPickedUp)
        {
            Vector3 dir = cam.transform.forward;
            dir.Normalize();

            // Compute the initial speed
            Vector3 initialSpeed = computeInitialSpeedTest();

            // Apply the speed
            pickedBall.GetComponent<Rigidbody>().isKinematic = false;
            pickedBall.GetComponent<Rigidbody>().AddForce(initialSpeed, ForceMode.VelocityChange);

            // Release the ball
            releaseBallWithMouse();
        }
    }

    // Only for test
    private Vector3 computeInitialSpeedTest()
    {
        return new Vector3(0.0f, 0.0f, 10.0f);
    }
    /**
     * Compute the initial speed to throw the ball
     */
    private Vector3 computeInitialSpeed()
    {
        float m = pickedBall.GetComponent<Rigidbody>().mass;
        Vector3 p = m * Physics.gravity;

        // The  current instant
        int t = 2;
        // The velocity at instant t-1
        Vector3 V1 = (positions[t - 1] - positions[t - 2]) / (times[t - 1] - times[t - 2]);
        Debug.Log("V1: " + V1);
        // The velocity at instant t
        Vector3 V2 = (positions[t] - positions[t - 1]) / (times[t] - times[t - 1]);
        Debug.Log("V2: " + V2);
        // The instantaneous acceleration
        Vector3 a = (V2 - V1) / (times[t] - times[t - 2]);
        Debug.Log("a: " + a);
        // The force component to compute the final initial velocity
        // The player force f must not exceed maximum player force
        Vector3 f = clamp(m * a - p, playerForce);
        //Vector3 f = m * a - p;
        //f.Normalize();
        //f *= playerForce;
        Debug.Log("f: " + f);
        Debug.Log("Player force: " + playerForce);
        // The move vector
        Vector3 d = positions[t] - positions[t - 2];
        // Unit directionnal vector
        Vector3 u = V1;
        u.Normalize();

        /* Te final velocity */
        // Dot products
        // f dot d
        Vector3 tmp = f;
        tmp.Scale(d);
        float fDotd = tmp.x + tmp.y + tmp.z;
        // p dot d
        tmp = p;
        tmp.Scale(d);
        float pDotd = tmp.x + tmp.y + tmp.z;

        // Computation
        Vector3 V = Mathf.Sqrt(Mathf.Abs(2 * (fDotd + pDotd) / m)) * u;

        Debug.Log("Initial speed: " + V);
        return V;
    }

    /**
     * Clamps the force vector to the maxForce module. 
     * If the module of force is highter thant maxForce, then we return a vector with the same direction as force with maxForce module
     */
    private Vector3 clamp(Vector3 force, float maxForce)
    {
        Vector3 result = force;
        if (force.magnitude > maxForce)
        {
            result.Normalize();
            result *= maxForce;
        }
        return result;
    }

    private void pickupBallWithController()
    {
        
    }

    private void releaseBallWithController()
    {

    }

    /**
     * Update all the measurements required for physics computation
     */
    private void updateMeasurements()
    {
        if (ballPickedUp)
        {
            // Position data
            positions[0] = positions[1];
            positions[1] = positions[2];
            positions[2] = pickedBall.transform.position;

            // Time data
            times[0] = times[1];
            times[1] = times[2];
            times[2] = Time.time;
        }
    }
}
