using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class CheckConnection : MonoBehaviour {

	public static bool ConnectFlag;
	public static int ConnectTime;
	public static float Time2PushConnection = 10f;
	public static float Timer = 0f;
	public static bool CONNECTION_FAILURE = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//Time.deltaTime

		Timer += Time.deltaTime;

		if (Timer >= Time2PushConnection) {

			Timer = 0;
			StartCoroutine (PushConnectionTime());

		}

		if (CONNECTION_FAILURE) {
			SceneManager.LoadScene ("Timeout");
		}
			
	}



	IEnumerator PushConnectionTime() {

		string ScriptURL = "https://segs.w3.uvm.edu/gamescripts/pa_DB_CheckConnection.php";

		WWWForm form = new WWWForm();
		// Push to form
		var time = Convert.ToInt64 ((DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalMilliseconds);
		form.AddField ("time", time.ToString());
		// Session ID
		form.AddField ("sessionId", PlayerLoginName.SessionID);
		// AMT_flag
		form.AddField ("AMT_flag", (string)PlayerLoginName.AMT_flag.ToString());
		// Push to Server
		WWW w = new WWW(ScriptURL, form);
		// wait for response
		yield return w; 
		if (!string.IsNullOrEmpty(w.error)) { //error
			print(w.error);
			CONNECTION_FAILURE = true;

		}
		else { //success
			CONNECTION_FAILURE  = false;

			}


		}








}
