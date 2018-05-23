using UnityEngine;
using System.Collections;

public class SceneOrganizer : MonoBehaviour {

    private int[] scene;
	//create a random scene order
	void Start () {


		scene = new int[17];


		for (int i = 0; i < 17; i++) 
		{
			scene[i] = i;


		}

		ShuffleArray (scene);

		PlayerPrefsX.SetIntArray("scene",scene);


	
	}

	public static void ShuffleArray<T>(T[] arr) {
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0, i + 1);
			T tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}
	
	// Update is called once per frame

}
