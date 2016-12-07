using UnityEngine;
using System.Collections;

public class pinController : MonoBehaviour {

	public bool state = true;

	// Use this for initialization
	void Start () {
		
	}

	void OnCollisionExit2D() {
		if (gameObject.Transform.position.x > gameObject.Transform.position.y) {
			state = false;
			Destroy (gameObject, 5);
		}
	}
}
