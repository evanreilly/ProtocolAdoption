using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// Generate a random completion code after the game ends (mostly for use with mechanical turk)
// set this next to UserInformer.cs script in the game over scene


public class GenerateCompletionCode : MonoBehaviour {

	public static int CompletionCode;
	//public static string CompletionCodeText

	// Use this for initialization
	void Start () {
	
	}

	public static void PushCompletionCode(int ComCode){
		// Push Game Event to Database
		string CompletionScriptURL = "https://segs.w3.uvm.edu/gamescripts/pa_DB_CompletionCode.php";

		// Push Event to Database
		WWWForm form = new WWWForm();

		// push event to form
		form.AddField("CompletionCode", ComCode);

		// Session ID
		form.AddField ("sessionId", PlayerLoginName.SessionID);

		// AMT_flag
		form.AddField ("AMT_flag", (string)PlayerLoginName.AMT_flag.ToString());

		// final payment
		form.AddField("payment", Survey.TotalPayment.ToString());

		// Push to Server
		WWW www = new WWW(CompletionScriptURL, form);

	}

}
