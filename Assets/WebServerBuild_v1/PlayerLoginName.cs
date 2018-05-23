using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
//using System.Collections.Generic;
//using System.Linq;

public class PlayerLoginName : MonoBehaviour {

	public static string USER_NAME = "NewUser"; //init
	public static long StartTime = 0; //init
	// dont start until user enters name
	public static bool UserHasEnteredName = false; 

	public static string SessionID = "NONE"; //init session id
	public static bool SessionIDProcessed = false;
	public static bool StartUpload = false;

	// init public treatment array variable
	public static string[] TreatmentArray;  
	//public static string[] TreatmentArray = new string[28];  // n rounds

	// random seed, pulled from db
	public static int RandomSeed;
	// for setting starting level of returning users
	public static int LastRound = 1;

	// previous session score
	public static int PrevSessionScore = 0;

	// survey round
	//public static int SurveyRound = 17;
	public static int SurveyRound;  // set off size of treatment array+1
	public static float TotalScore4Survey = 0;
	public static bool Skip2Survey = false;
	public static bool LoadSurveyOnce = true;
	// Skip the survey (orlando version)
	public static bool SkipSurvey = false; //true;

	// Mechanical Turk Build Flag
	public static bool AMT_flag = false;  // turn on/off for turk build (true/false)
	public static bool UserNameLoginErr = false;
	public static bool UserNameCompletionErr = false;
	public static string AMT_FinalScore;
	public static string AMT_FinalCompletionCode;


	// wait for end of video to display login
	public static bool ShowLoginGui = false;
	//public static int timeToShowLogin = 103;  // show login after the movie (seconds)
	public static int shiftX = 200; //shift x position of screen text 

	// dev
	public static int timeToShowLogin = 0;  // show login at start


	// font style for title display
	GUIStyle TitleName;

	void Start(){
		// set fontstyle for gui display
		TitleName = new GUIStyle();
		//smallFont.fontSize = 10;
		TitleName.fontSize = 40;
		TitleName.normal.textColor= Color.white;

		// wait to show login until after movie
		StartCoroutine(WaitToDisplayGUI(timeToShowLogin));
	}



	IEnumerator WaitToDisplayGUI(int TimeInSeconds = 1)
	{ // wait for TimeInSeconds to display login gui (set to after movie)
		//yield return new WaitForSeconds(TimeInSeconds);
		yield return new WaitForSecondsRealtime(TimeInSeconds);
		ShowLoginGui = true;
	}




	// Update is called once per frame
	void Update () {


		if (UserHasEnteredName && !StartUpload) {

			// Get Unix Time (Total Milliseconds since 1/1/1970 
			StartTime = Convert.ToInt64 ((DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalMilliseconds);

			// Get Session ID/ insert new to SQL database
			StartCoroutine (UploadSessionID (USER_NAME, StartTime));

			// UploadSessionID (USER_NAME, t0);
			StartUpload = true;
		}
		else if (Skip2Survey){

			if (LoadSurveyOnce){
				LoadSurveyOnce = false;
				Time.timeScale = 1; //resume
				SceneManager.LoadScene ("GameSurvey");
			}


		}
		else if (SessionIDProcessed){ //database finished upload
			// start game
			Time.timeScale = 1; //resume
			SceneManager.LoadScene ("BiosecurityGame",LoadSceneMode.Single);
		}
		else {
			// pause game until user enters name
			Time.timeScale = 0;
		}
	}

	IEnumerator UploadSessionID(string user, long unixtime) {
		string ScriptURL = "https://segs.w3.uvm.edu/gamescripts/pa_DB_SessionID.php";
		// upload username, starttime, to SQL database.  Get Current SessionID
		WWWForm form = new WWWForm();
		// Push to form
		form.AddField ("time", unixtime.ToString());
		form.AddField ("user", user);
		// AMT_flag
		form.AddField ("AMT_flag", (string)PlayerLoginName.AMT_flag.ToString());
		// Push to Server
		WWW w = new WWW(ScriptURL, form);
		// wait for response
		yield return w; 
		if (!string.IsNullOrEmpty(w.error)) { //error
			print(w.error);
			SceneManager.LoadScene ("Timeout");
		}
		else { //success
			// assign SessionID
			string[] returnArr = w.text.Split(';');
			print (w.text);
			bool AMT_Auth = true;
			// check for authorized user, if not show error dialog
			if (AMT_flag) {
				//UserAuthed = (bool)returnArr [4];
				bool UserAuthed = Convert.ToBoolean (returnArr [4]);
				if (!UserAuthed) {
					SessionIDProcessed = false;
					UserHasEnteredName = false;
					StartUpload = false;
					// display login error
					//if (returnArr.Length == 6) {
					if (returnArr[5] != "NULL") {

						//DisplayUsernameCompletionErrDialog (returnArr [5], returnArr [3]);
						UserNameLoginErr = false;
						UserNameCompletionErr = true;
						AMT_FinalScore = returnArr [3];
						AMT_FinalCompletionCode = returnArr[5];


					} else { 
						UserNameLoginErr = true;
						UserNameCompletionErr = false;	
						//DisplayUsernameLoginErrDialog ();
					}

					AMT_Auth = false;
				}
			}

			// always authorized when not amt build..
			if (AMT_Auth) {
				SessionID = returnArr [0];
				print ("SessionID="+SessionID.ToString ());
				// Assign Remaining to Treatment Array
				print ("Treatments="+returnArr[1]);
				// init array size + practice rounds
				int NumRounds = returnArr[1].Split (',').Length+2;
				TreatmentArray = new string[NumRounds];
				SurveyRound = NumRounds;  //round to start survey
				int tct = 2;
				TreatmentArray [0] = "Z";  //these are demo/practice
				TreatmentArray [1] = "Z";
				// Construct Treatment Array
				for (int tmp_i=0; tmp_i< returnArr[1].Split (',').Length; tmp_i++){
					TreatmentArray[tct] = returnArr[1].Split (',')[tmp_i];
					tct += 1;
				}
				// Random Seed
				RandomSeed = Int32.Parse(returnArr[5]);
				// Find LastRound if returning user (small chance will be array duplicate, if so, catch it)
				if (Convert.ToInt32 (returnArr [2]) > 0) {
					LastRound = Convert.ToInt32 (returnArr [2]) + 2;
				} 
				// Session Score
				PrevSessionScore = Convert.ToInt32 (returnArr [3]);

				// Set Flag as finished ProcessingID
				if (LastRound == SurveyRound) {
					// start survey now!
					TotalScore4Survey += PrevSessionScore;
					Skip2Survey = true;
					SessionIDProcessed = false;
				} else {
					SessionIDProcessed = true;

				}

		
			}

		}
	}


	void OnGUI () {

		if (ShowLoginGui) {

			if (SceneManager.GetActiveScene ().name == "TitleScreen") {
				GUI.skin.label.fontSize = 30;
				GUI.skin.textField.fontSize = 30;
			}
			// display text box for user to enter name
			if (!UserHasEnteredName) {
				//GUI.Label (new Rect (10, 10, 100, 30), "Enter Name:");
				//USER_NAME = GUI.TextField (new Rect (90, 10, 200, 25), USER_NAME, 40);

				// Game Name Menu
				if (AMT_flag) { 
					GUI.Label (new Rect (shiftX+200, 50, 155, 30), "Protocol Adoption (AMT Build)", TitleName);
					//GUI.Label (new Rect (200, 200, 600, 150), "Type in your User ID and press ENTER to begin. \nComplete more tasks for a better score.  Keep your pigs alive. Good Luck.");
					GUI.Label (new Rect (shiftX+100, 200, 600, 150), "Type in a name and press ENTER to begin.");
				} else {
					GUI.Label (new Rect (shiftX+200, 50, 155, 30), "Protocol Adoption", TitleName);
					GUI.Label (new Rect (shiftX+100, 200, 600, 150), "Type in a name and press ENTER to begin.");
				}

				// Game Instructions
				//GUI.Label (new Rect (200, 150, 350, 30), "Type in a name and press ENTER to begin.\n  Go for the high score.  Keep your pigs alive. Good Luck.");
				// Interactive Label (user input)
				GUI.Label (new Rect (shiftX+100, 130, 200, 50), "Enter Name:");
				USER_NAME = GUI.TextField (new Rect (shiftX+280, 130, 400, 40), USER_NAME, 40);

				// Check that user clicks enter within the Text Field (and value has changed)
				if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && USER_NAME != "NewUser") {

					// clean up username here
					USER_NAME = USER_NAME.Replace (";", "").Replace ("\"", "").Replace ("*", "").Replace ("'", "").Replace ("\\", "");
					Console.WriteLine (USER_NAME);

					UserHasEnteredName = true;
				}
			}

			// Display Login Errors for AMT

			if (UserNameLoginErr) {
				GUI.Label (new Rect (shiftX+100, 350, 350, 100), "Error, unauthorized user name.\nPlease check that you have entered\nyour user code correctly. ", TitleName);
			}
			if (UserNameCompletionErr) {
				GUI.Label (new Rect (shiftX+100, 350, 350, 100), "You have already completed the game!\nCompletionCode = " + AMT_FinalCompletionCode + "\nTotalScore = " + AMT_FinalScore, TitleName);
			}
		}
	}

}
