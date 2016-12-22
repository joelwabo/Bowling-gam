using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		List<int[]> list = new List<int[]> ();
		int[] tab = new int[2];
		for (int i = 0; i < 11; i++) {
			tab [0] = 2;
			tab [1] = 3;
			if (i == 0) {
				tab [0] = 0;
				tab [1] = 10;
			}
			if (i == 1) {
				tab [0] = 0;
				tab [1] = 10;
			}
			if (i == 2) {
				tab [0] = 0;
				tab [1] = 10;
			}
			if (i == 3) {
				tab [0] = 0;
				tab [1] = 10;
			}
			if (i == 4) {
				tab [0] = 0;
				tab [1] = 10;
			}
			if (i == 5) {
				tab [0] = 10;
				tab [1] = 0;
			}
			if (i == 6) {
				tab [0] = 0;
				tab [1] = 10;
			}
			if (i == 7) {
				tab [0] = 0;
				tab [1] = 10;
			}
			if (i == 8) {
				tab [0] = 0;
				tab [1] = 10;
			}
			if (i == 9) {
				tab [0] = 0;
				tab [1] = 10;
			}
			if (i == 10) {
				tab [0] = 0;
				tab [1] = 10;
			}
			list.Add (tab);
			scoreToShow (list);
		}

		/*pointText = GameObject.Find("point").GetComponents<TextMesh>();
		totalText = GameObject.Find("total").GetComponents<TextMesh>();*/

		//GetComponentsInchildren

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
	public void scoreToShow(List<int[]> list)
	{
		int indice = list.Count - 1;
		int score = 0;
		if (isStrike (list[indice])) {
			pointText[2*indice].text = " ";
			if(2*indice+1 < 21)
				pointText[2*indice+1].text = "X";
			nbreStrike++;
		} else if (isSpare (list[indice])) {
			pointText[2*indice].text = list[indice][0].ToString();
			pointText[2*indice+1].text = "/";
			nbreSpare++;
		} else {	
			if ((indice >= 1) && (indice <= 10)){
				if (nbreSpare>0) {
					score = 10 + list[indice][0];
					totalText[indice-1].text = score.ToString();
					nbreSpare = 0;
				} 
				if (nbreStrike>0) {
					List<int> scoreStrike = strikeScore (list, nbreStrike);
					for (int i = 0 ; i < scoreStrike.Count ; i++)
					{
						totalText[indice-i-1].text = scoreStrike[i].ToString();
					}
					nbreStrike = 0;
				}
			}
			if (indice == 10) {
				score = list [indice - 1] [0] + list [indice - 1] [1] + list [indice] [0];	
				pointText[2*indice].text = list[indice][0].ToString();
				totalText[indice - 1].text = score.ToString();
			} else {
				score = list [indice] [0] + list [indice] [1];
				pointText[2*indice].text = list[indice][0].ToString();
				pointText[2*indice+1].text = list[indice][1].ToString();
				totalText[indice].text = score.ToString();
			}
			setTotal ();
		}
	}

	public void setTotal()
	{
		int total = 0;
			for (int i = 0; i < totalText.Count; i++){
				total = total + int.Parse(totalText[i].text);
		}
		scoreText.text = total.ToString();
	}

	public List<int> strikeScore(List<int[]> list, int nbreStrike)
	{
		int indice = list.Count - 1;
		List<int> scoreStrike = new List<int>();
		if (indice == 10) {
			for (int i = 0; i < nbreStrike; i++) {
				if (i == 0)
					scoreStrike.Add (10 + list[indice - 1][1] + list[indice][0]);
				else if (i == 1)
					scoreStrike.Add (20 + list[indice][0]);
				else if (i > 1)
					scoreStrike.Add (30);
			}
			Debug.Log ("ICI");
		} else {
			for (int i = 0; i < nbreStrike; i++) {
				if (i == 0)
					scoreStrike.Add (10 + list[indice][0] + list[indice][1]);
				else if (i == 1)
					scoreStrike.Add (20 + list[indice][0]);
				else if (i > 1)
					scoreStrike.Add (30);
			}
		}
		return scoreStrike;
	}

	public bool isStrike(int[] tab)
	{
		if ((tab [0] == 10) || (tab [1] == 10))
			return true;
		else
			return false;
	}
	public bool isSpare(int[] tab)
	{
		if ((tab[0] + tab[1]) == 10)
			return true;
		else
			return false;
	}
}
