using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleanerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
            GameObject obj = collision.gameObject;
        if (obj.tag == "Pin")
        {
            Debug.Log("cleaning pin " + obj.name);
            obj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 0.75f), ForceMode.VelocityChange);
        }
    }
}
