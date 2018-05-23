using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DotHighUncertainty : MonoBehaviour {

	private GameObject[] dots;
	private GameObject infDot;


	// Use this for initialization
	void Start () {
		Debug.Log ("High Disease Uncertainty");
		dots = GameObject.FindGameObjectsWithTag("cleanDot");
		infDot = GameObject.FindGameObjectWithTag ("infDot");
		/*
		GameObject.Find ("peekButton").GetComponent<Button> ().enabled = true;
		GameObject.Find ("peekButton").GetComponent<Image> ().enabled = true;
		GameObject.Find ("peekButton").GetComponentInChildren<Text> ().text = "Infection Status\n$" + gameController.peekCost + "/10 sites";
		*/

		//turn thenm all uncertain
		infDot.GetComponent<SpriteRenderer> ().color = Color.gray;
		foreach (GameObject dot in dots) {
			

			if (dot.name != ("myDot")) {
				
				dot.GetComponent<SpriteRenderer> ().color = Color.gray;

			}
		}

		gameController.DotInitComplete2 = true;
	
	}
	
	// Update is called once per frame

}
