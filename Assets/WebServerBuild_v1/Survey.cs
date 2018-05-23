using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Survey : MonoBehaviour {

	public static int QuestionNumber = 0;
	public static int HLIntroCt = 0;
	public static int HLQuestionNumber = 0;
	public static int randQind;
	public static string HLChosenOption;
	public static int HLprob;
	public static string HLrewardStr;

	public static List<string> Questions;
	public static List<string> Answers = new List<string>();
	public static List<string> AnswersHL = new List<string>();
	public static List<string> QuestionsHL = new List<string>();
	public static List<string> HLInstructions = new List<string>();
	public static List<string> HLOptionsA = new List<string>();
	public static List<string> HLOptionsB = new List<string>();


	// font style for title display
	GUIStyle TitleStyle;
	GUIStyle QuestionStyle;
	GUIStyle QuestionStyleScale;
	GUIStyle QuestionStyleScale2;
	GUIStyle ButtonStyle;
	GUIStyle ButtonStyleSmall;
	GUIStyle HLOptionStyle;
	GUIStyle RewardStyle;
	GUIStyle ButtonStyleLeft;

	// current question answer 
	public static string CurrentAnswer;

	// button flags (standard survey)
	public static bool AgreeButtons;
	public static bool AgeButtons;
	public static bool GenderButtons;
	public static bool GamingButtons;
	public static bool DoctorButtons;
	public static bool FluButtons;
	public static bool ButtonClicked = false;
	public static bool DebtButtons;
	public static bool PestButtons;
	public static bool SchoolButtons;
	public static bool SchoolButtons2;
	public static bool YesNoButtons;
	public static bool OKButton;
	public static bool FinishButton;
	public static bool HLfinished = false;
	public static bool DisplayFinished = false;
	public static bool BackButtonClicked = false;
	public static bool ShowBackButton;
	public static bool AuthorityButtons;
	public static bool ScaleButtons;  // buttons for square scale survey questions
	public static string scaleLeft; // 
	public static string scaleRight;
	public static List<string> ScaleRights  = new List<string>(); // list of scalar button descriptions 
	public static List<string> ScaleLefts = new List<string>();
	// change the screen color on question change
	public static bool FlipGuiColor = false;


	// HL Questions
	public static bool HLSurvey = false;
	public static bool HLIntro = false;
	public static bool HLIntroClicked = false;
	public static bool HLOptions = false;
	public static bool HLButtonClicked = false;
	public static bool HLReward = false;
	public static bool CalcReward = true;
	// HL Reward strings (these are non-AMT values, converted in Start)
	public static string HL_A_p1 = "$2.00";
	public static string HL_A_p2 = "$1.60";
	public static string HL_B_p1 = "$3.85";
	public static string HL_B_p2 = "$0.10";

	// shifting variable
	public static float shiftX = 125; // set to 0 for compliance..

	// tally finaly payment
	public static float TotalPayment;

	// only do this once
	public static bool PushDataFlag = true;

	//bool and float to set if we are not allowing click
	public static bool allowClick;
	public static float clickWaitTimer;
	//only allow one random completion code generation
	public static int onlyGenerateOnce;

	// Use this for initialization
	void Start () {
		Debug.Log ("SURVEY STARTING");
		onlyGenerateOnce = 0;
		allowClick = false;
		clickWaitTimer = 0f;
		// question array
		Questions = new List<string>(){
			"Click on your age bracket",
			"Click on your gender",
			"How much does each description sound like you? \n             Generally, I come across as:",
			"How much does each description sound like you? \n             Generally, I come across as:",
			"How much does each description sound like you? \n             Generally, I come across as:",
			"How much does each description sound like you? \n             Generally, I come across as:",
			"How much does each description sound like you? \n             Generally, I come across as:",
			"If a dairy cow is producing milk then its level of welfare \nmust be acceptable.  Describe how much you agree.",
			"Describe your willingness to kill vertebrate pests \nto keep your animals healthy.",
			"What is the highest degree or level of school you have \ncompleted? If currently enrolled, indicate highest degree?",
			"In the last six months, have you heard news about livestock diseases \nthat made you concerned about animal deaths, \nfor example animals dying from the avian flu?",
			"An animal's welfare is good if it has adequete \nfood, water and shelter. Describe how you agree.",
			"An animal's welfare includes adequete exercise, \nspace, and social interaction (for social animals). \nDescribe how you agree.",
			"Which way would you prefer to get information about the risk of contagion?",
			"Have you ever been a farmer or lived or worked on a farm?"

		};



		ScaleLefts = new List<string> () {
			"someone who is talkative, outgoing, is comfortable around people, but could be noisy and attention-seeking",
			"someone who is forthright,tends to be critical and find fault with others and doesn’t tolerate foolishness.", 
			"someone who is sensitive and excitable, and can be tense.",
			"someone who likes to plan things, likes to tidy up, pays attention to details but can be rigid or inflexible",
			"someone who is a practical person who is not interested in abstract ideas, prefers work that is routine, and has few artistic interests"
		};


		ScaleRights = new List<string>(){
			"someone who is reserved, private person, doesn’t like to draw attention to themselves, and can be shy around strangers",
			"someone who is generally trusting and forgiving, is interested in people, but can be taken for granted and finds it difficult to say no.",
			"someone who is relaxed, unemotional, rarely gets irritated and seldom feels blue",
			"someone who doesn’t necessarily work to a schedule, tends to be flexible, but disorganized and often forgets to put things back in their proper place.",
			"someone who spends time reflecting on things, has an active imagination and likes to think up new ways of doing things, but may lack pragmatism"
		};

		/* // Old Questions
		"On Average, how many hours to you spend\n gaming a week?",
		"Which of the following unexpected costs might cause an \nincrease in your personal debt?",
		"How would you describe yourself along a spectrum from \ntreating disease symptoms when they show up " +
		"(going to the \ndoctor when you get sick) to attempting to prevent disease by \npromoting a healthy environment" +
		" (i.e. regular doctor check-ups)",
		"When was the last time you got a flu shot?",
		"A dairy cow in a natural environment is happier and more \nproductive. Describe how you agree with this statement.",
		"Were the instructions of the game clear enough \nfor comfortable play during the experiment?",
		-*/

		// Convert Scores for Mturk play
		if (PlayerLoginName.AMT_flag) { 
			HL_A_p1 = "$0.60";
			HL_A_p2 = "$0.50";
			HL_B_p1 = "$1.10";
			HL_B_p2 = "$0.05";
		}

		HLInstructions = new List<string> () {
			"Over the next ten questions you will be asked to select one of \ntwo options by clicking on the option that appeals most to you.\n" +
			"\nFor example, you will be asked to select either: \n\n" +
			"(i.) Option A: do you prefer to select a wager that \n                       gives you "+HL_A_p1+", 10 out of 10 times\n\n" +
			"(ii.) Option B: a 5/10 chance of earning "+HL_B_p1+" and \n                        5/10 chance of earning "+HL_B_p2,

			"After you have answered the ten questions, one of the \nten questions will be randomly selected.  " +
			"The wager will be run\n using the option that you selected."+
			"  A random number will \nbe generated and the amount that you earn from the selection \nwill be added in real $US to your total earnings.",

			"For example, if Question 5 were selected and you had selected \nOption B on Question 5 which was: \n\n" +
			"     A 5/10 chance of earning "+HL_B_p1+" and\n      a 5/10 chance of earning $0.10\n\n" +
			"Then a random number would be generated, if it was 1-5 \nyou would earn "+HL_B_p1+" in real $US that would be added to your \ntotal earnings." +
			"If the random number was 6-10, you would earn \n"+HL_B_p2+" in real $US that would be added to your total earnings."

		};
		HLOptionsA = new List<string> () {
			"You have a 1/10 chance to earn "+HL_A_p1+" and a 9/10 chance to earn "+HL_A_p2,
			"You have a 2/10 chance to earn "+HL_A_p1+" and a 8/10 chance to earn "+HL_A_p2,
			"You have a 3/10 chance to earn "+HL_A_p1+" and a 7/10 chance to earn "+HL_A_p2,
			"You have a 4/10 chance to earn "+HL_A_p1+" and a 6/10 chance to earn "+HL_A_p2,
			"You have a 5/10 chance to earn "+HL_A_p1+" and a 5/10 chance to earn "+HL_A_p2,
			"You have a 6/10 chance to earn "+HL_A_p1+" and a 4/10 chance to earn "+HL_A_p2,
			"You have a 7/10 chance to earn "+HL_A_p1+" and a 3/10 chance to earn "+HL_A_p2,
			"You have a 8/10 chance to earn "+HL_A_p1+" and a 2/10 chance to earn "+HL_A_p2,
			"You have a 9/10 chance to earn "+HL_A_p1+" and a 1/10 chance to earn "+HL_A_p2,
			"You have a 10/10 chance to earn "+HL_A_p1+" and a 0/10 chance to earn "+HL_A_p2
		};

		HLOptionsB = new List<string> () {
			"You have a 1/10 chance to earn "+HL_B_p1+" and a 9/10 chance to earn "+HL_B_p2,
			"You have a 2/10 chance to earn "+HL_B_p1+" and a 8/10 chance to earn "+HL_B_p2,
			"You have a 3/10 chance to earn "+HL_B_p1+" and a 7/10 chance to earn "+HL_B_p2,
			"You have a 4/10 chance to earn "+HL_B_p1+" and a 6/10 chance to earn "+HL_B_p2,
			"You have a 5/10 chance to earn "+HL_B_p1+" and a 5/10 chance to earn "+HL_B_p2,
			"You have a 6/10 chance to earn "+HL_B_p1+" and a 4/10 chance to earn "+HL_B_p2,
			"You have a 7/10 chance to earn "+HL_B_p1+" and a 3/10 chance to earn "+HL_B_p2,
			"You have a 8/10 chance to earn "+HL_B_p1+" and a 2/10 chance to earn "+HL_B_p2,
			"You have a 9/10 chance to earn "+HL_B_p1+" and a 1/10 chance to earn "+HL_B_p2,
			"You have a 10/10 chance to earn "+HL_B_p1+" and a 0/10 chance to earn "+HL_B_p2
		};


	}

	// Update is called once per frame
	void Update () {

		//Debug.Log (clickWaitTimer.ToString());
		//Debug.Log ("dt ="+Time.deltaTime.ToString ());
		//allowClick = true;

		// show gui buttons wrt questionNumber
		if (QuestionNumber == 0) {
			AgeButtons = true;
		} else if (QuestionNumber == 1) {
			GenderButtons = true;
		} else if (QuestionNumber >= 2 && QuestionNumber<=6) {
			ScaleButtons = true;
			scaleRight = ScaleRights [QuestionNumber - 2]; // set left text
			scaleLeft = ScaleLefts[QuestionNumber-2]; // right scale text
		} else if (QuestionNumber == 7 || QuestionNumber == 11 || QuestionNumber == 12) {
			AgreeButtons = true;
		} else if (QuestionNumber == 13){
			AuthorityButtons = true;
		} else if (QuestionNumber == 8) {
			PestButtons = true;
		} else if (QuestionNumber == 9) {
			SchoolButtons = true;
		} else if (QuestionNumber == 10 || QuestionNumber == 14) {
			YesNoButtons = true;
		} else if (QuestionNumber == 15) {
			// start holt and laury
			HLSurvey = true;
			HLIntro = true;
		}


		if (HLIntro) {
			OKButton = true;

			if (HLIntroClicked){
				HLIntroCt += 1;
				HLIntroClicked = false;
			}

			if (HLIntroCt == HLInstructions.Count) {
				HLIntro = false;
				OKButton = false;
				HLOptions = true;
			}
		}
		//if we are not allowing a click increment timer
		//Debug.Log("allow click:" + allowClick);
		//Debug.Log ("school buttons: " + SchoolButtons);
		if (!allowClick && !SchoolButtons)
		{
			//Debug.Log ("you are seeing this because I dont think this is gonna be executed");
			HLButtonClicked = false;
			ButtonClicked = false;
			clickWaitTimer += Time.deltaTime;
		}
		//when enough time has passed allow click
		if (clickWaitTimer > 2f)
		{
			allowClick = true;
		}
		//dont allow click for a few seconds
		if (allowClick || SchoolButtons)
		{
			// when button clicked move to next question
			if (ButtonClicked)
			{
				bool GetNextQ = true;
				if (SchoolButtons)
				{

					if (CurrentAnswer == "Some college credit, no degree")
					{

						GetNextQ = false;
						SchoolButtons = false;
						SchoolButtons2 = true;

						// change the question depending on their school answer
						Questions[QuestionNumber] = "If appropriate, approximately how much college have\n you completed?";

					}
				}
				// for school question, ask a second question depending on answer
				if (GetNextQ)
				{
					Debug.Log(CurrentAnswer);
					//Answers.Add(CurrentAnswer);
					if (Answers.Count > QuestionNumber)
					{
						Answers[QuestionNumber] = CurrentAnswer;
					}
					else
					{
						Answers.Add(CurrentAnswer);
					}

					//foreach (string a in Answers) { Debug.Log (a + ",");}

					QuestionNumber += 1;
					//Debug.Log(CurrentAnswer);
					CurrentAnswer = "";
					ButtonClicked = false;
					ClearButtons();

					// change screen color
					if (FlipGuiColor) {
						FlipGuiColor = false;
					} else {
						FlipGuiColor = true;
					}


				}
				//dont allow click now
				allowClick = false;
				clickWaitTimer = 0f;
			}

			if (HLButtonClicked)
			{

				if (AnswersHL.Count > HLQuestionNumber)
				{
					AnswersHL[HLQuestionNumber] = CurrentAnswer;
				}
				else
				{
					AnswersHL.Add(CurrentAnswer);
				}
				//AnswersHL.Add(CurrentAnswer);
				HLQuestionNumber += 1;
				CurrentAnswer = "";
				HLButtonClicked = false;
				//dont allow click now
				allowClick = false;
				clickWaitTimer = 0f;

				// change screen color
				if (FlipGuiColor) {
					FlipGuiColor = false;
				} else {
					FlipGuiColor = true;
				}


			}
		}
		// compute reward 
		if (HLQuestionNumber == HLOptionsA.Count) {
			HLOptions = false;
			HLReward = true;
			// get the reward
			if (CalcReward){
				randQind = UnityEngine.Random.Range (0, HLOptionsA.Count - 1);
				HLChosenOption = AnswersHL [randQind];
				HLprob = UnityEngine.Random.Range (0, 10);
				// calculate reward
				bool optionA = HLChosenOption.Contains ("Option A");
				if (HLprob <= (randQind + 1)) {
					if (optionA) {
						//HLrewardStr = "$2.00";
						HLrewardStr = HL_A_p1;
					} else {
						//HLrewardStr = "$3.85";
						HLrewardStr = HL_B_p1;
					}
				} else {
					if (optionA) {
						//HLrewardStr = "$1.60";
						HLrewardStr = HL_A_p2;
					} else {
						//HLrewardStr = "$0.10";
						HLrewardStr = HL_B_p2;
					}
				}
				CalcReward = false;
			}
		}

		if (HLfinished) {
			HLReward = false;

			// push data to server
			if (PushDataFlag) {
				PushData ();
				PushDataFlag = false;
			}
			// display finished screen
			DisplayFinished = true;
			HLSurvey = false;
		}

		if ((QuestionNumber < Questions.Count && QuestionNumber > 0 && QuestionNumber < 15) || (HLQuestionNumber<HLOptionsA.Count && HLQuestionNumber>0)) {
			ShowBackButton = true;
		} else {
			ShowBackButton = false;
		}

		if (BackButtonClicked) {
			if (HLSurvey) {
				HLQuestionNumber -= 1;
			} else {
				QuestionNumber -= 1;
				ClearButtons ();
			}
			BackButtonClicked = false;
		
			// change screen color
			if (FlipGuiColor) {
				FlipGuiColor = false;
			} else {
				FlipGuiColor = true;
			}
		}



	}

	public static void ClearButtons(){

		// turn off active button flags
		if (GenderButtons) {
			GenderButtons = false;
		} 
		if (AgeButtons) {
			AgeButtons = false;
		} 
		if (GamingButtons) {
			GamingButtons = false;
		} 
		if (DebtButtons) {
			DebtButtons = false;
		} 
		if (DoctorButtons) {
			DoctorButtons = false;
		} 
		if (FluButtons) {
			FluButtons = false;
		} 
		if (AgreeButtons) {
			AgreeButtons = false;
		} 
		if (PestButtons) {
			PestButtons = false;
		} 
		if (SchoolButtons) {
			SchoolButtons = false;
		} 
		if (YesNoButtons) {
			YesNoButtons = false;
		} 
		if (AuthorityButtons) {
			AuthorityButtons = false;
		} 
		if (SchoolButtons2) {
			SchoolButtons2 = false;
		}
		if (ScaleButtons) {
			ScaleButtons = false;
		}


	}

	public static void PushData(){
		Debug.Log ("Pushing Data to server");
		// push survey data to php script on server
		string SurveyScriptURL = "https://segs.w3.uvm.edu/gamescripts/pa_DB_Survey.php";

		// retool this to push end level stats
		WWWForm form = new WWWForm();

		// Session ID
		form.AddField ("sessionId", PlayerLoginName.SessionID);
		//form.AddField ("sessionId", UnityEngine.Random.Range(10000,10000000).ToString());

		// Timestamp
		var CurrentTime = Convert.ToInt64 ((DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalMilliseconds);
		//var CurrentTime = "0";
		form.AddField ("time", CurrentTime.ToString());

		// stringify the lists
		string QuestionString = string.Join(";", Questions.ToArray()).Replace("\n","");
		string AnswerString = string.Join (";", Answers.ToArray ());
		string HLAnswerString = string.Join (";", AnswersHL.ToArray ());
		// clean \n from strings


		// post to php
		form.AddField ("Questions", QuestionString);
		form.AddField ("Answers", AnswerString);
		form.AddField ("HLAnswers", HLAnswerString);

		// HL-reward
		form.AddField ("RewardQNumber", randQind);
		form.AddField ("Rewardroll", HLprob.ToString());
		form.AddField ("Reward", HLrewardStr);

		// AMT_flag
		form.AddField ("AMT_flag", (string)PlayerLoginName.AMT_flag.ToString());
		//form.AddField ("AMT_flag", "false");

		// Push to Server
		WWW www = new WWW(SurveyScriptURL, form);

	}


	void OnGUI () {

		// flip background color on question change
		if (FlipGuiColor) {
			GUI.backgroundColor = Color.blue;
		} else {
			GUI.backgroundColor = Color.green;
		}

		// set fontstyle for gui display
		TitleStyle = new GUIStyle();
		//smallFont.fontSize = 10;
		TitleStyle.fontSize = 45;
		TitleStyle.normal.textColor= Color.white;

		RewardStyle = new GUIStyle ();
		RewardStyle.fontSize = 30;
		RewardStyle.normal.textColor= Color.white;
		RewardStyle.wordWrap = true;

		QuestionStyle =  new GUIStyle();
		QuestionStyle.fontSize = 35;
		QuestionStyle.normal.textColor= Color.white;

		QuestionStyleScale =  new GUIStyle();
		QuestionStyleScale.fontSize = 33;
		QuestionStyleScale.normal.textColor= Color.white;
		QuestionStyleScale.alignment = TextAnchor.MiddleCenter;
		QuestionStyleScale.wordWrap = true;

		QuestionStyleScale2 =  new GUIStyle();
		QuestionStyleScale2.fontSize = 20;
		QuestionStyleScale2.normal.textColor= Color.white;
	
		ButtonStyle = new GUIStyle ("button");
		ButtonStyle.fontSize = 35;

		ButtonStyleSmall = new GUIStyle ("button");
		ButtonStyleSmall.fontSize = 17;

		HLOptionStyle = new GUIStyle ("button");
		HLOptionStyle.fontSize = 30;
		HLOptionStyle.wordWrap = true;

		ButtonStyleLeft = new GUIStyle ("button");
		ButtonStyleLeft.fontSize = 35;
		ButtonStyleLeft.alignment = TextAnchor.MiddleLeft;

		// personality scale questions
		if (ScaleButtons) {

			// left choice label
			GUI.Label (new Rect (shiftX+10, 120, 450, 500),scaleLeft, QuestionStyleScale);
			// right choice label
			GUI.Label (new Rect (shiftX+675, 120, 450, 500),scaleRight, QuestionStyleScale);
			// instruction labels
			GUI.Label (new Rect (shiftX+10, 450, 1100, 500),"Choose the square on the scale to indicate how much you think each description sounds like you" , QuestionStyleScale);
			GUI.Label (new Rect (200, 620, 600, 100),"(More Like Description 1)" , QuestionStyleScale2);
			GUI.Label (new Rect (900, 620, 600, 100),"(More Like Description 2)" , QuestionStyleScale2);

			int bx0 = 100; int by0 = 500; 
			int bwid = 100; int bht = 100; int bspc = bwid;
			// buttons
			if (GUI.Button (new Rect (shiftX+bx0, by0, bwid,bht), "", ButtonStyle)){
				CurrentAnswer = "1:" + scaleLeft;
				ButtonClicked = true;

			}
			if (GUI.Button (new Rect (shiftX+bx0+bspc, by0, bwid,bht), "", ButtonStyle)){
				CurrentAnswer = "2:" + scaleLeft;
				ButtonClicked = true;

			}
			if (GUI.Button (new Rect (shiftX+bx0+2*bspc, by0, bwid,bht), "", ButtonStyle)){
				CurrentAnswer = "3:" + scaleLeft;
				ButtonClicked = true;

			}
			if (GUI.Button (new Rect (shiftX+bx0+3*bspc, by0, bwid,bht), "", ButtonStyle)){
				CurrentAnswer = "4:" + scaleLeft;
				ButtonClicked = true;

			}
			if (GUI.Button (new Rect (shiftX+bx0+4*bspc, by0, bwid,bht), "", ButtonStyle)){
				CurrentAnswer = "5:" + scaleLeft;
				ButtonClicked = true;
			}
			if (GUI.Button (new Rect (shiftX+bx0+5*bspc, by0, bwid,bht), "", ButtonStyle)){

				CurrentAnswer = "6:" + scaleRight;
				ButtonClicked = true;
			}
			if (GUI.Button (new Rect (shiftX+bx0+6*bspc, by0, bwid,bht), "", ButtonStyle)){

				CurrentAnswer = "7:" + scaleRight;
				ButtonClicked = true;
			}
			if (GUI.Button (new Rect (shiftX+bx0+7*bspc, by0, bwid,bht), "", ButtonStyle)){
				CurrentAnswer = "8:" + scaleRight;
				ButtonClicked = true;

			}
			if (GUI.Button (new Rect (shiftX+bx0+8*bspc, by0, bwid,bht), "", ButtonStyle)){
				CurrentAnswer = "9:" + scaleRight;
				ButtonClicked = true;

			}
				
		}



		if (HLSurvey) {

			if ((HLQuestionNumber + 1) <= HLOptionsA.Count && HLIntro == false) {
				GUI.Label (new Rect (shiftX+250, 15, 100, 30), "End Game Survey Part 2/2\n       (Question " + (HLQuestionNumber + 1).ToString () + "/" + HLOptionsA.Count.ToString () + ")", TitleStyle);
			}
			if (HLIntro) {
				GUI.Label (new Rect (shiftX+250, 15, 100, 30), "End Game Survey Part 2/2", TitleStyle);
				GUI.Label (new Rect (shiftX+40, 150, 100, 30), HLInstructions[HLIntroCt], QuestionStyle);

			} else if (HLOptions){
				GUI.Label (new Rect (shiftX+280, 125, 100, 30),"Select your preferred option.", QuestionStyle);
			}
		} else if (QuestionNumber<Questions.Count){
			// Title Label
			GUI.Label (new Rect (shiftX+250, 15, 100, 30), "End Game Survey Part 1/2\n (Question "+(QuestionNumber+1).ToString()+"/"+Questions.Count.ToString()+")",TitleStyle);
			GUI.Label (new Rect (shiftX+50, 150, 100, 30), Questions [QuestionNumber], QuestionStyle);

		}


		if (HLOptions) {

			int width = 400;
			int x0 = 50;
			int y0 = 220; 
			int height = 300;
			int xoff = 500;
			if (GUI.Button (new Rect (shiftX+x0, y0, width, height), "Option A: "+HLOptionsA[HLQuestionNumber], HLOptionStyle)) {
				CurrentAnswer = "Option A: " + HLOptionsA [HLQuestionNumber];
				HLButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0, width, height),"Option B: "+HLOptionsB[HLQuestionNumber], HLOptionStyle)) {
				CurrentAnswer = "Option B: "+HLOptionsB[HLQuestionNumber];
				HLButtonClicked = true;
			}

		}

		if (HLReward) {

			// display the reward
			GUI.Label (new Rect (shiftX+30, 150, 100, 30), "Question "+(randQind+1).ToString()+" was selected to complete your wager.",TitleStyle);
			// display option
			GUI.Label (new Rect (shiftX+40, 210, 900, 100), "\nYou chose "+AnswersHL[randQind],RewardStyle);
			// display random number
			GUI.Label (new Rect (shiftX+40, 280, 100, 30), "\n\nThe random number that was drawn was "+HLprob.ToString(),QuestionStyle);
			// display reward
			GUI.Label (new Rect (shiftX+40, 380, 100, 30), "\n\nYou earned an extra "+HLrewardStr+" that will be added \nto your cash earnings!",QuestionStyle);

			int width = 150;
			int x0 = 450;
			int y0 = 600; 
			if (GUI.Button (new Rect (shiftX+x0, y0, width, 50),"Ok!",ButtonStyle)) {
				HLfinished = true;
			}

		}

		if (DisplayFinished) {
			// get total payment
			float GamePay;
			// set conversion rate (different for mturk build)
			float rewardConversion;
			if (PlayerLoginName.AMT_flag) {
				rewardConversion = 50000; //100000;
			} else {
				rewardConversion = 50000;
			}

			// calculate payment
			if (PlayerLoginName.Skip2Survey) {
				GamePay = (PlayerLoginName.TotalScore4Survey/rewardConversion);
			}
			else{
				GamePay = gameController.totalProfit/rewardConversion;
			}

			// check for negative score:
			if (GamePay<0.50){ GamePay = 0.50f;} // minimum pay of $0.50
			TotalPayment = (float)Math.Round(GamePay+ float.Parse(HLrewardStr.Replace("$","")),2);

			//onlyGenerateOnce = 2; //survey build
			if (onlyGenerateOnce < 1)
			{
				//// generate random code
				GenerateCompletionCode.CompletionCode = (int)UnityEngine.Random.Range(1000, 9999);
				// Push Completion Code to DB
				GenerateCompletionCode.PushCompletionCode(GenerateCompletionCode.CompletionCode);
				onlyGenerateOnce++;
			}
			// display end (add Mturk instruction)
			if (PlayerLoginName.AMT_flag) {
				GUI.Label (new Rect (shiftX+250, 200, 100, 30), "You have finished the survey. \nThanks for participating!\nYou earned $" + TotalPayment.ToString () + "\nCompletion Code: " + GenerateCompletionCode.CompletionCode.ToString ()+ "\n\nRemember to submit this code \non the Mechanical Turk Survey!", TitleStyle);
			} else {
				GUI.Label (new Rect (shiftX+250, 200, 100, 30), "You have finished the survey. \nThanks for participating!\nYou earned $" + TotalPayment.ToString () + "\nCompletion Code: " + GenerateCompletionCode.CompletionCode.ToString (), TitleStyle);
			}
		}


		// fully skip survey (special version, no survey!)
		if (PlayerLoginName.SkipSurvey) {

			QuestionNumber = 1000;

			if (onlyGenerateOnce < 1)
			{
				ClearButtons ();
				//// generate random code
				GenerateCompletionCode.CompletionCode = (int)UnityEngine.Random.Range(1000, 9999);
				// Push Completion Code to DB
				GenerateCompletionCode.PushCompletionCode(GenerateCompletionCode.CompletionCode);
				onlyGenerateOnce++;
			}

			GUI.Label (new Rect (shiftX+250, 200, 100, 30), "Thanks for participating!\nFinal Score: " + gameController.totalProfit.ToString() + "\nCompletion Code: " + GenerateCompletionCode.CompletionCode.ToString (), TitleStyle);

		}


		if (ShowBackButton) {
			int width = 150;
			int x0 = 50;
			int y0 = 50; 
			if (GUI.Button (new Rect (shiftX+x0, y0, width, 50),"Back",ButtonStyle)) {
				BackButtonClicked = true;
			}
		}


		if (OKButton) {
			int width = 150;
			int x0 = 450;
			int y0 = 600; 
			if (GUI.Button (new Rect (shiftX+x0, y0, width, 50),"Ok!",ButtonStyle)) {
				HLIntroClicked = true;
			}
		}


		if (AgeButtons) {

			int xoff = 400;
			int yoff = 80;
			int yoffInc = yoff;

			if (GUI.Button (new Rect (shiftX+200, 200+yoff, 250, 50), "Age 18-20",ButtonStyle)) {
				CurrentAnswer = "Age 18-20";
				AgeButtons = false;
				ButtonClicked = true;
			}
			yoff += yoffInc;

			if (GUI.Button (new Rect (shiftX+200, 200+yoff, 250, 50), "Age 21-25",ButtonStyle)) {
				CurrentAnswer = "Age 21-25";
				ButtonClicked = true;
			}
			yoff+=yoffInc;


			if (GUI.Button (new Rect (shiftX+200, 200+yoff, 250, 50), "Age 26-30",ButtonStyle)) {
				CurrentAnswer = "Age 26-30";
				ButtonClicked = true;
			}
			yoff+=yoffInc;

			if (GUI.Button (new Rect (shiftX+200, 200+yoff, 250, 50), "Age 31-35",ButtonStyle)) {
				CurrentAnswer = "Age 31-35";
				ButtonClicked = true;
			}
			yoff+=yoffInc;

			if (GUI.Button (new Rect (shiftX+200, 200+yoff, 250, 50), "Age 36-40",ButtonStyle)) {
				CurrentAnswer = "Age 36-40";
				ButtonClicked = true;
			}
			yoff+=yoffInc;

			if (GUI.Button (new Rect (shiftX+200, 200+yoff, 250, 50), "Age 41-45",ButtonStyle)) {
				CurrentAnswer = "Age 41-45";
				ButtonClicked = true;
			}
			yoff+=yoffInc;


			// next column
			yoff = yoffInc;

			if (GUI.Button (new Rect (shiftX+200+xoff, 200+yoff, 250, 50), "Age 46-50",ButtonStyle)) {
				CurrentAnswer = "Age 46-50";
				ButtonClicked = true;
			}
			yoff+=yoffInc;


			if (GUI.Button (new Rect (shiftX+200+xoff, 200+yoff, 250, 50), "Age 51-55",ButtonStyle)) {
				CurrentAnswer = "Age 51-55";
				ButtonClicked = true;
			}
			yoff+=yoffInc;

			if (GUI.Button (new Rect (shiftX+200+xoff, 200+yoff, 250, 50), "Age 56-60",ButtonStyle)) {
				CurrentAnswer = "Age 56-60";
				ButtonClicked = true;
			}
			yoff+=yoffInc;


			if (GUI.Button (new Rect (shiftX+200+xoff, 200+yoff, 250, 50), "Age 61-65",ButtonStyle)) {
				CurrentAnswer = "Age 61-65";
				ButtonClicked = true;
			}
			yoff+=yoffInc;

			if (GUI.Button (new Rect (shiftX+200+xoff, 200+yoff, 250, 50), "Age 66-70",ButtonStyle)) {
				CurrentAnswer = "Age 66-70";
				ButtonClicked = true;
			}
			yoff+=yoffInc;

			if (GUI.Button (new Rect (shiftX+200+xoff, 200+yoff, 250, 50), "Age over 70",ButtonStyle)) {
				CurrentAnswer = "Age Over 70";
				ButtonClicked = true;
			}
			yoff+=yoffInc;

		}


		if (GenderButtons) {

			int yoff = 80;
			int xoff = 400;
			int width = 500;
			if (GUI.Button (new Rect (shiftX+200, 200+yoff, width, 50),"Female",ButtonStyle)) {
				CurrentAnswer = "Female";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+200, 200+(2*yoff), width, 50), "Male",ButtonStyle)) {
				CurrentAnswer = "Male";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+200, 200+(3*yoff), width, 50), "Non-binary/Third Gender",ButtonStyle)) {
				CurrentAnswer = "Non-binary/Third Gender";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+200, 200+(4*yoff), width, 50), "Prefer not to say",ButtonStyle)) {
				CurrentAnswer = "Prefer not to say";
				ButtonClicked = true;
			}

		}

		if (GamingButtons) {

			int yoff = 80;
			int xoff = 400;
			int width = 500;
			if (GUI.Button (new Rect (shiftX+200, 200+yoff, width, 50),"Less than 1 hour",ButtonStyle)) {
				CurrentAnswer = "Less than 1 hour";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+200, 200+(2*yoff), width, 50), "1 to 5 hours",ButtonStyle)) {
				CurrentAnswer = "1 to 5 hours";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+200, 200+(3*yoff), width, 50), "5 to 10 hours",ButtonStyle)) {
				CurrentAnswer = "5 to 10 hours";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+200, 200+(4*yoff), width, 50), "More than 10 hours",ButtonStyle)) {
				CurrentAnswer = "More than 10 hours";
				ButtonClicked = true;
			}

		}

		if (DebtButtons) {

			int yoff = 80;
			int xoff = 500;
			int width = 470;
			int x0 = 50;
			int y0 = 200; 
			if (GUI.Button (new Rect (shiftX+x0, y0+yoff, width, 50),"I can not afford extras",ButtonStyle)) {
				CurrentAnswer = "I can not afford extras";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0+(2*yoff), width, 50), "Parking Ticket $35",ButtonStyle)) {
				CurrentAnswer = "Parking Ticket $35";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0+(3*yoff), width, 50), "Celebration Meal $75",ButtonStyle)) {
				CurrentAnswer = "Celebration Meal $75";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0+(4*yoff), width, 50), "Appliance Repair $150",ButtonStyle)) {
				CurrentAnswer = "Appliance Repair $150";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0+(5*yoff), width, 50), "New Tires $300",ButtonStyle)) {
				CurrentAnswer = "New Tires $300";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0+(6*yoff), width, 50), "Vehicle Repair $600",ButtonStyle)) {
				CurrentAnswer = "Vehicle Repair $600";
				ButtonClicked = true;
			}

			// column 2
			if (GUI.Button (new Rect (shiftX+x0+xoff, y0+yoff, width, 50),"Home improvement $1250",ButtonStyle)) {
				CurrentAnswer = "Home improvement $1250";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0+(2*yoff), width, 50), "Doctor Bills $2500",ButtonStyle)) {
				CurrentAnswer = "Doctor Bills $2500";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0+(3*yoff), width, 50), "Damage caused by fire $5000",ButtonStyle)) {
				CurrentAnswer = "House fire damage $5000";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0+(4*yoff), width, 50), "Buy used car $10000",ButtonStyle)) {
				CurrentAnswer =  "Buy used car $10000";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0+(5*yoff), width, 50), "Buy nice car $20000",ButtonStyle)) {
				CurrentAnswer = "Buy nice car $20000";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0+(6*yoff), width, 50), "Invest in a startup $50000",ButtonStyle)) {
				CurrentAnswer = "Invest in a startup $50000";
				ButtonClicked = true;
			}

		}

		if (DoctorButtons) {

			int yoff = 80;
			int xoff = 0; //500;
			int width = 470;
			int x0 = 300;
			int y0 = 250; 
			if (GUI.Button (new Rect (shiftX+x0, y0 + yoff, width, 50), "(1) Only treat symptoms", ButtonStyleLeft)) {
				CurrentAnswer = "(1) Only treat symptoms";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (2 * yoff), width, 50), "(2) Treat Symptoms", ButtonStyleLeft)) {
				CurrentAnswer = "(2) Treat Symptoms";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (3 * yoff), width, 50), "(3) Mixed", ButtonStyleLeft)) {
				CurrentAnswer = "(3) Mixed";
				ButtonClicked = true;
			}


			if (GUI.Button (new Rect (shiftX+x0+xoff, y0 + 4*yoff, width, 50), "(4) Preventative efforts", ButtonStyleLeft)) {
				CurrentAnswer = "(4) Preventative efforts";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0 + (5 * yoff), width, 50), "(5) Only Preventative Efforts", ButtonStyleLeft)) {
				CurrentAnswer = "(5) Only Preventative Efforts";
				ButtonClicked = true;
			}

		}

		if (FluButtons) {

			int yoff = 80;
			int xoff = 500;
			int width = 600;
			int x0 = 200;
			int y0 = 150; 
			if (GUI.Button (new Rect (shiftX+x0, y0 + yoff, width, 50), "Within the last 12 months", ButtonStyle)) {
				CurrentAnswer = "Within the last 12 months";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (2 * yoff), width, 50), "One to two years ago", ButtonStyle)) {
				CurrentAnswer =  "One to two years ago";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (3 * yoff), width, 50), "More than 2 yrs or never", ButtonStyle)) {
				CurrentAnswer = "More than 2 yrs or never";
				ButtonClicked = true;
			}



		}



		if (AuthorityButtons) {

			int yoff = 100;
			int xoff = 500;
			int width = 850;
			int x0 = 80;
			int y0 = 200; 
			if (GUI.Button (new Rect (shiftX+x0, y0 + yoff, width, 50), "A probability number such as 25%", ButtonStyle)) {
				CurrentAnswer = "A probability number such as 25%";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (2 * yoff), width, 50), "A word explaining the expected level such as 'High'", ButtonStyle)) {
				CurrentAnswer = "A word explaining the expected level such as 'High'";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (3 * yoff), width, 50),"A threat level gauge", ButtonStyle)) {
				CurrentAnswer ="A threat level gauge";
				ButtonClicked = true;
			}


			if (GUI.Button (new Rect (shiftX+x0, y0 + 4*yoff, width, 50), "None of these", ButtonStyle)) {
				CurrentAnswer =  "None of these";
				ButtonClicked = true;
			}


		}


		if (AgreeButtons) {

			int yoff = 80;
			int xoff = 500;
			int width = 450;
			int x0 = 320;
			int y0 = 200; 
			if (GUI.Button (new Rect (shiftX+x0, y0 + yoff, width, 50), "Strongly Disagree", ButtonStyle)) {
				CurrentAnswer = "Strongly Disagree";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (2 * yoff), width, 50), "Disagree", ButtonStyle)) {
				CurrentAnswer =  "Disagree";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (3 * yoff), width, 50),"Undecided", ButtonStyle)) {
				CurrentAnswer ="Undecided";
				ButtonClicked = true;
			}


			if (GUI.Button (new Rect (shiftX+x0, y0 + 4*yoff, width, 50), "Agree", ButtonStyle)) {
				CurrentAnswer = "Agree";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (5 * yoff), width, 50), "Strongly Agree", ButtonStyle)) {
				CurrentAnswer = "Strongly Agree";
				ButtonClicked = true;
			}


		}

		if (PestButtons) {

			int yoff = 80;
			int xoff = 500;
			int width = 950;
			int x0 = 20;
			int y0 = 200; 
			if (GUI.Button (new Rect (shiftX+x0, y0 + yoff, width, 50), "Unwilling to kill vertebrate pests", ButtonStyle)) {
				CurrentAnswer = "Unwilling to kill vertebrate pests";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (2 * yoff), width, 50), "Willing to kill rodents found within the facility", ButtonStyle)) {
				CurrentAnswer =  "Willing to kill rodents found within the facility";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (3 * yoff), width, 50),"Willing to kill rodents & birds in the facility", ButtonStyle)) {
				CurrentAnswer ="Willing to kill rodents & birds in the facility";
				ButtonClicked = true;
			}


			if (GUI.Button (new Rect (shiftX+x0, y0 + 4*yoff, width, 50), "Willing to kill rodents & birds on all facility grounds", ButtonStyle)) {
				CurrentAnswer = "Willing to kill rodents & birds on all facility grounds";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (5 * yoff), width, 50), "Willing to kill rodents, birds, & larger animals on all grounds", ButtonStyle)) {
				CurrentAnswer = "Willing to kill rodents, birds, & larger animals on all grounds";
				ButtonClicked = true;
			}


		}

		if (SchoolButtons) {

			int yoff = 80;
			int xoff = 500;
			int width = 450;
			int x0 = 50;
			int y0 = 180; 
			if (GUI.Button (new Rect (shiftX+x0, y0 + yoff, width, 50), "Doctorate degree", ButtonStyleSmall)) {
				CurrentAnswer = "Doctorate degree";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (2 * yoff), width, 50), "Professional degree", ButtonStyleSmall)) {
				CurrentAnswer = "Professional degree";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (3 * yoff), width, 50),"Master's degree", ButtonStyleSmall)) {
				CurrentAnswer ="Master's degree";
				ButtonClicked = true;
			}


			if (GUI.Button (new Rect (shiftX+x0, y0 + 4*yoff, width, 50), "Bachelor's degree", ButtonStyleSmall)) {
				CurrentAnswer = "Bachelor's degree";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (5 * yoff), width, 50), "Associate degree", ButtonStyleSmall)) {
				CurrentAnswer = "Associate degree";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + 6*yoff, width, 50), "Trade/technical/vocational training", ButtonStyleSmall)) {
				CurrentAnswer =   "Trade/technical/vocational training";
				ButtonClicked = true;
			}

			// column 2

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0 + (2 * yoff), width, 50),  "Some college credit, no degree", ButtonStyleSmall)) {
				CurrentAnswer = "Some college credit, no degree";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0 + (3 * yoff), width, 50), "High school graduate diploma or equivalent", ButtonStyleSmall)) {
				CurrentAnswer =  "High school graduate diploma or equivalent";
				ButtonClicked = true;
			}


			if (GUI.Button (new Rect (shiftX+x0+xoff, y0 + 4*yoff, width, 50),  "Some high school, no diploma", ButtonStyleSmall)) {
				CurrentAnswer = "Some high school, no diploma";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0 + 5*yoff, width, 50),  "Nursery school to 8th grade", ButtonStyleSmall)) {
				CurrentAnswer = "Nursery school to 8th grade";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0+xoff, y0 + 6*yoff, width, 50),  "No schooling completed", ButtonStyleSmall)) {
				CurrentAnswer = "No schooling completed";
				ButtonClicked = true;
			}
		}

		if (SchoolButtons2) {

			int yoff = 100;
			int xoff = 500;
			int width = 800;
			int x0 = 65;
			int y0 = 200; 
			if (GUI.Button (new Rect (shiftX+x0, y0 + yoff, width, 50), "(i.) I am in or have finished my freshman year", ButtonStyleLeft)) {
				CurrentAnswer = "(i.) I am in or have finished my freshman year";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (2 * yoff), width, 50), "(ii.) I am in or have finished my sophomore year", ButtonStyleLeft)) {
				CurrentAnswer = "(ii.) I am in or have finished my sophomore year";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (3 * yoff), width, 50), "(iii.) I am in or have finished my junior year", ButtonStyleLeft)) {
				CurrentAnswer ="(iii.) I am in or have finished my junior year";
				ButtonClicked = true;
			}


			if (GUI.Button (new Rect (shiftX+x0, y0 + 4*yoff, width, 50), "(iv.) I am in my senior year", ButtonStyleLeft)) {
				CurrentAnswer =  "(iv.) I am in my senior year";
				ButtonClicked = true;
			}



		}



		if (YesNoButtons) {

			int yoff = 80;
			int xoff = 500;
			int width = 300;
			int x0 = 400;
			int y0 = 250; 
			if (GUI.Button (new Rect (shiftX+x0, y0 + yoff, width, 50), "Yes", ButtonStyle)) {
				CurrentAnswer = "Yes";
				ButtonClicked = true;
			}

			if (GUI.Button (new Rect (shiftX+x0, y0 + (2 * yoff), width, 50), "No", ButtonStyle)) {
				CurrentAnswer =  "No";
				ButtonClicked = true;
			}


		}




	}

}