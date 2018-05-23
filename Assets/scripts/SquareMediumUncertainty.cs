using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SquareMediumUncertainty : MonoBehaviour {

	private GameObject[] dots;
	private GameObject infDot;
	private Transform bioSquare;
	private Transform bioSquare2;
	private int count;

	private int[] randomizeUncertainty;

	// Use this for initialization
	void Start () {
		GameObject.Find ("peekButtonBio").GetComponent<Button> ().enabled = true;
		GameObject.Find ("peekButtonBio").GetComponent<Image> ().enabled = true;
		GameObject.Find ("peekButtonBio").GetComponentInChildren<Text> ().text = "Biosecurity Status\n$" + gameController.peekCost + "/10 sites";
		randomizeUncertainty = new int[49];
		for (int i = 0; i < 49; i++) 
		{
			randomizeUncertainty[i] = i;


		}

		ShuffleArray (randomizeUncertainty);

		dots = GameObject.FindGameObjectsWithTag("cleanDot");


		count = 0;
		foreach (GameObject dot in dots) {
			bioSquare = dot.gameObject.transform.GetChild (0);

			for (int j = 0; j < 23; j++) {
				if (randomizeUncertainty [j] == count && dot.name != ("myDot")) {

					bioSquare.GetComponent<SpriteRenderer> ().color = Color.gray;
					bioSquare.GetComponentInChildren<Text> ().text = "?";
				}
			
		}
			count++;
	}

		gameController.DotInitComplete3 = true;

	}

	public static void ShuffleArray<T>(T[] arr) {
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range (0, i + 1);
			T tmp = arr [i];
			arr [i] = arr [r];
			arr [r] = tmp;
		}
	}
	// Update is called once per frame

}