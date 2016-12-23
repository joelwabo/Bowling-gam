using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handController : MonoBehaviour {

    private Camera cam;
    private bool depthOn = false;
    private Vector3 keyPoint;
    // Use this for initialization
    void Start () {
        cam = Camera.main;
        keyPoint = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 storagePos = GameObject.Find("RightLane/Lane/ball_storage_001").transform.position;
        Vector3 mouse = Input.mousePosition;
        //mouse.z = -cam.transform.position.z + storagePos.z-0.5f;
        mouse.z = -cam.transform.position.z + keyPoint.z;
        Vector3 mWorldPos = cam.ScreenToWorldPoint(mouse);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            depthOn = !depthOn;
            keyPoint = transform.position;
            Debug.Log("Depth On: " + depthOn);
        }
        if (depthOn)
        {
            mWorldPos.z += -keyPoint.y + mWorldPos.y;
            mWorldPos.y = keyPoint.y;
        }else
        {
            mWorldPos.z = keyPoint.z;
        }
        transform.position = mWorldPos;
    }
}
