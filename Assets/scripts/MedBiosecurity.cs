using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MedBiosecurity : MonoBehaviour {


	float x;
	float y;
	float z;
	Vector3 pos;
	private float[] xArray;
	float xPos;
	private float[] yArray;
	float yPos;
	private GameObject[] dots;
	private Transform bioSquare;

	private Color brown = new Color32(139,69,19,255);
	private Color darkGreen = new Color32 (0, 100, 0,255);

	// Use this for initialization
	void Start () {

		xPos = -8.3f;
		xArray = new float[38];
		yPos = -4.6f;
		yArray = new float[30];
		float rand;

		int count = 0;


		// arrays of all x and y positions
		for (int i = 0; i < 38; i++) 
		{
			xArray [i] = xPos;
			xPos = xPos + .45f;

		}

		for (int j = 0; j < 30; j++) 
		{
			yArray [j] = yPos;
			yPos = yPos + .26f;
		}

		//randomize arrays
		ShuffleArray(xArray);
		ShuffleArray (yArray);


		//randomize dots 
		dots = GameObject.FindGameObjectsWithTag("cleanDot");
		foreach (GameObject dot in dots) {
			bioSquare = dot.gameObject.transform.GetChild (0);

			x = xArray[count%xArray.Length];
			y = yArray[count%yArray.Length];
			z = 0;
			pos = new Vector3 (x, y, z);

			dot.transform.position = pos;
			count = count + 1;


			if (dot.name != ("myDot"))
			{
				rand = Random.Range(0.0f,1.0f);

				if (dot.name ==("dot (1)"))
				{
					dot.GetComponent<SpriteRenderer> ().color = Color.red;
					dot.tag = "infDot";

				}

				if (rand <= .25) {
					bioSquare.GetComponent<SpriteRenderer> ().color = Color.black;
					bioSquare.tag = "black";
					bioSquare.GetComponentInChildren<Text> ().text = "N";
				} 
				else if (rand > .25 && rand <= .5) {
					bioSquare.GetComponent<SpriteRenderer> ().color = brown;
					bioSquare.tag = "brown";
					bioSquare.GetComponentInChildren<Text> ().text = "L";
				}
				else if (rand > .5 && rand <=.75) {
					bioSquare.GetComponent<SpriteRenderer> ().color = darkGreen;
					bioSquare.tag = "darkGreen";
					bioSquare.GetComponentInChildren<Text> ().text = "M";
				}
				else if (rand > .75) {
					bioSquare.GetComponent<SpriteRenderer> ().color = Color.green;
					bioSquare.tag = "green";
					bioSquare.GetComponentInChildren<Text> ().text = "H";
				}



			}


		}


		// set trigger for completion of script
		gameController.DotInitComplete = true;


	}

	public static void ShuffleArray<T>(T[] arr) {
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0, i + 1);
			T tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}


}
