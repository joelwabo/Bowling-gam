using UnityEngine;
using System.Collections;

public class pinController : MonoBehaviour {

	public bool state = true;
    private GlobalParameters parameters;

	// Use this for initialization
	void Start () {
        parameters = GameObject.Find("RightLane").GetComponent<GlobalParameters>();
	}

    // Count out fallen pins
    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "cave floor")
        {
            Debug.Log("Pin felt: " +  name);

            // Increment the fallen pins of the frame's round
            parameters.incrementFallenPins();
        }
    }*/
}
