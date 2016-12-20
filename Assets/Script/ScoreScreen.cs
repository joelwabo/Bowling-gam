using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreScreen : MonoBehaviour {


	//Debug.Log
	public int nbrePlayer;
	public string name;
	private int total;
	private TextMesh[] pointText;
	private TextMesh[] totalText;
	private TextMesh frameTitle;
	private TextMesh scoreText;

	// Use this for initialization 
	void Start () {
		TextMesh nameText = GameObject.Find("NameText").GetComponent<TextMesh>();
		frameTitle = GameObject.Find("FrameTitle").GetComponent<TextMesh>();
		scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
		pointText = GameObject.Find("Point").GetComponents<TextMesh>();
		totalText = GameObject.Find("Total").GetComponents<TextMesh>();

		nameText.text = name;

		/*pointText = GameObject.Find("point").GetComponents<TextMesh>();
		totalText = GameObject.Find("total").GetComponents<TextMesh>();

		//GetComponentsInchildren
		/*TextMesh[] texts = gameObject.GetComponentsInChildren<TextMesh>();
		TextMesh[] point = new TextMesh[21];
		TextMesh[] pointTotal = new TextMesh[10];
		int i = 0, j = 0; k = 0;
		foreach (var text in texts) {
			if(texts.name.Contains("point")){
				point[i] = text;
				i++;
			}
			if(texts.name.Contains("total")){
				pointTotal[j] = text;
				j++;
			}
			if(texts.name.Contains("somme")){
				totalText = text;
			}
				
		}*/

	}

	public void setScoreText (int count, int round)
	{		
		frameTitle.text = "Count: " + count.ToString();
	}

	// on effectue le cacul pour le score suivant les normes du bowling
	public void scoreToShow(List<int[]> list)
	{
		int indice = list.Capacity - 1;
		int score = 0;
		int nbreStrike = 0;
		if (isStrike (list[indice])) {
			nbreStrike++;
			pointText[2*indice].text = "X";
		} else if (isSpare (list[indice])) {
			pointText[2*indice+1].text = "/";
		} else {
			if ((indice > 1) && (indice < 10)){
				if (isSpare (list[indice - 1])) {
					score = 10 + list[indice][0];
					totalText[indice-1].text = score.ToString();
				} else if (nbreStrike != 0) {
					List<int> scoreStrike = strikeScore (list, nbreStrike);
					for (int i = scoreStrike.Capacity; i > 0; i--)
					{
						totalText[indice-i].text = scoreStrike[i].ToString();
					}
					nbreStrike = 0;
				}
			}
			if (indice == 10)
				score = list[indice - 1][0] + list[indice - 1][1] + list[indice - 1][0];		
			else
				score = list[indice][0] + list[indice][1];
			setTotal (indice);
			totalText[indice].text = score.ToString();
		}

	}

	public void setTotal(int indice)
	{
		int total = 0;
		foreach (var text in totalText) {
			total = total + int.Parse(text.text);
		}
		scoreText.text = total.ToString();
	}

	public List<int> strikeScore(List<int[]> list, int nbreStrike)
	{
		int indice = list.Capacity - 1;
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
		if (tab [0] + tab [1] == 10)
			return true;
		else
			return false;
	}
}
