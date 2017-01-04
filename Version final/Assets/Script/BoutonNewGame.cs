using UnityEngine;
using System.Collections;

public class BoutonNewGame : MonoBehaviour {
	private string playerName;
	public string stringToEdit = "Hello World";

	void OnMouseEnter(){
		GetComponent<Renderer>().material.color = Color.gray;
	}

	void OnMouseExit() {
		GetComponent<Renderer>().material.color = Color.white;
	}

	void OnMouseUp(){
		Debug.Log (playerName);
	}

	void OnGUI() {
		name = GUI.TextField(new Rect(10, 10, 200, 20), name, 25);
	}
}
