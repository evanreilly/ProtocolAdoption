using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SquareHighUncertainty : MonoBehaviour {

	private GameObject[] dots;
	private GameObject infDot;
	private Transform bioSquare;
	private Transform bioSquare2;

	// Use this for initialization
	void Start () {
		Debug.Log ("High Bio Uncertainty");
		dots = GameObject.FindGameObjectsWithTag("cleanDot");
		infDot = GameObject.FindGameObjectWithTag ("infDot");
		bioSquare = infDot.gameObject.transform.GetChild (0);
		bioSquare.GetComponent<SpriteRenderer> ().color = Color.gray;
		bioSquare.GetComponentInChildren<Text> ().text = "?";
		/*
		GameObject.Find ("peekButtonBio").GetComponent<Button> ().enabled = true;
		GameObject.Find ("peekButtonBio").GetComponent<Image> ().enabled = true;
		GameObject.Find ("peekButtonBio").GetComponentInChildren<Text> ().text = "Biosecurity Status\n$" + gameController.peekCost + "/10 sites";
		*/

		foreach (GameObject dot in dots) {
			bioSquare = dot.gameObject.transform.GetChild (0);

			if (dot.name != ("myDot")) {
				bioSquare.GetComponent<SpriteRenderer> ().color = Color.gray;
				bioSquare.GetComponentInChildren<Text> ().text = "?";


			}
		}


		// set trigger for completion of script
		gameController.DotInitComplete3 = true;

	}

	// Update is called once per frame

}