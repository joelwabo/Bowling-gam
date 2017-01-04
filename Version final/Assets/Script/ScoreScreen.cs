using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoreScreen : MonoBehaviour {


	//Debug.Log
	public int nbrePlayer;
	public string name;
	private int total;
	int nbreStrike = 0;
	int nbreSpare = 0;
	private List<TextMesh> pointText;
	private List<TextMesh> totalText;
	private TextMesh frameTitle;
	private TextMesh scoreText;
	private TextMesh nameText;

	// Use this for initialization 
	void Start () {
		nameText = GameObject.Find("NameText").GetComponent<TextMesh>();
		frameTitle = GameObject.Find("FrameTitle").GetComponent<TextMesh>();
		scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
		pointText = getTextMesh("point");
		totalText = getTextMesh("total");
		nameText.text = name;
		List<int> list = new List<int> ();
		for (int i = 0; i < 11; i++) {
			if (i == 0) {
				list.Add (8);
				list.Add (2);
			}
			if (i == 1) {
				list.Add (5);
				list.Add (2);
			}
			if (i == 2) {
				list.Add (7);
				list.Add (0);
			}
			if (i == 3) {
				list.Add (10);
				list.Add (0);
			}
			if (i == 4) {
				list.Add (10);
				list.Add (0);
			}
			if (i == 5) {
				list.Add (6);
				list.Add (2);
			}
			if (i == 6) {
				list.Add (7);
				list.Add (3);
			}
			if (i == 7) {
				list.Add (8);
				list.Add (2);
			}
			if (i == 8) {
				list.Add (5);
				list.Add (3);
			}
			if (i == 9) {
				list.Add (4);
				list.Add (2);
			}
			if (i == 10) {
				list.Add (1);
			}
			scoreToShow (list);
		}
	}

	public List<TextMesh> getTextMesh (string name)
	{
		TextMesh[] texts = gameObject.GetComponentsInChildren<TextMesh>();
		List<TextMesh> textMesh = new List<TextMesh>();
		foreach (var text in texts) 
			if(text.name.Contains(name))
				textMesh.Add(text);
		return textMesh;
	}

	public void setScoreText (int count, int round)
	{		
		frameTitle.text = "Count: " + count.ToString();
	}

	// on effectue le cacul pour le score suivant les normes du bowling
	public void scoreToShow(List<int> list)
	{
		int indice = list.Count - 2;
		int score = 0;
		if (indice < 19) {
			if (isStrike (list [indice])) {
				pointText [indice].text = "X";
				pointText [indice + 1].text = " ";
				nbreStrike++;
			} else {
				score = list [indice] + list [indice + 1];
				pointText [indice].text = list [indice].ToString ();
				pointText [indice + 1].text = list [indice + 1].ToString ();
				totalText [indice / 2].text = score.ToString ();
				if (nbreStrike > 0) {
					List<int> scoreStrike = strikeScore (list, nbreStrike);
					for (int i = 0; i < scoreStrike.Count; i++) {
						totalText [indice / 2 - i - 1].text = scoreStrike [i].ToString ();
					}
					nbreStrike = 0;
				} 
				if (nbreSpare > 0) {
					score = 10 + list [indice];
					totalText [indice / 2 - 1].text = score.ToString ();
					nbreSpare--;
				}
				if (isSpare (list [indice], list [indice + 1])) {
					pointText [indice].text = list [indice].ToString ();
					pointText [indice + 1].text = "/";
					nbreSpare++;
				}
				setTotal (indice / 2);
			}
		} 
		if (indice == 19) {
			if (nbreStrike > 0) {
				List<int> scoreStrike = strikeScore (list, nbreStrike);
				for (int i = 0; i < scoreStrike.Count; i++) {
					totalText [indice / 2 - i].text = scoreStrike [i].ToString ();
				}
				pointText [indice + 1].text = list [indice].ToString ();
				setTotal ((indice-1) / 2);
				nbreStrike = 0;
			} else {
				score = int.Parse (totalText [(indice - 1) / 2].text) + list [indice + 1];
				pointText [indice + 1].text = list [indice + 1].ToString ();
				totalText [(indice - 1) / 2].text = score.ToString ();
				setTotal ((indice - 1) / 2);
			}
		}
	}

	public void setTotal(int index)
	{
		int total = 0;
		for (int i = 0; i <=index; i++){
				total = total + int.Parse(totalText[i].text);
		}
		scoreText.text = total.ToString();
	}

	public List<int> strikeScore(List<int> list, int nbreStrike)
	{
		int indice = list.Count - 2;
		List<int> scoreStrike = new List<int>();
		for (int i = 0; i < nbreStrike; i++) {
			if (i == 0)
				scoreStrike.Add (10 + list [indice] + list [indice+1]);
			else if (i == 1)
				scoreStrike.Add (20 + list [indice]);
			else if (i > 1)
				scoreStrike.Add (30);
		}
		return scoreStrike;
	}

	public bool isStrike(int a)
	{
		if (a == 10)
			return true;
		else
			return false;
	}
	public bool isSpare(int a, int b)
	{
		if ((a + b) == 10)
			return true;
		else
			return false;
	}
}
