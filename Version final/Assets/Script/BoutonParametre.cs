using UnityEngine;
using System.Collections;

public class BoutonParametre : MonoBehaviour {

	void OnMouseEnter(){
		GetComponent<Renderer>().material.color = Color.gray;
	}

	void OnMouseExit() {
		GetComponent<Renderer>().material.color = Color.white;
	}
	void OnMouseUp(){
		
	}
}
