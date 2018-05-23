using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;



public class gameController : MonoBehaviour
{

	// array of all dots
	private GameObject[] dots;

	// square
	private Transform bioSquare;

	public Color brown;
	public Color darkGreen;


	// Game objects
	private GameObject myDot;

	//infectd dot
	private GameObject infDot;

	private GameObject mySquare;

	// Game text objects
	public GUIText profitText;
	public GUIText monthText;
	public int profit = 0;
	//int for base profit
	private int baseProfit;
	public static int profit2;
	public static int totalProfit;
	public static int infectionCost;
	public static int biosecurityCost;
	public static string[] month = new string[] {
		"January",
		"February",
		"March",
		"April",
		"May",
		"June",
		"July",
		"August",
		"September",
		"October",
		"November",
		"December",
		"Next Year"
	};
	public static Color[] colors = new Color[] {
		Color.yellow,
		Color.cyan
	};
	public static int monthCounter = 0;
	public GUIText roundText;
	public GUIText nextRoundText;
	public GUIText biosecurityText;
	public GUIText infectionCostText;
	public GameObject profitObject;

	//Button objects
	private bool nextRound;
	public Button nextButton;
	public Button biobutton;

	//index for treatment array
	public static int sceneCount = 0;
	//treatments with highBio
	private string[] highBioTreatmentArray = {
		"Z",
		"FQ",
		"FR",
		"FS",
		"FT",
		"FU",
		"FV",
		"FW",
		"FX",
		"FY",
		"FZ",
		"GA",
		"GB",
		"GC",
		"GD",
		"GE",
		"GF",
	};
	//treatments with lowBio
	private string[] lowBioTreatmentArray = {
		"FA",
		"FB",
		"FC",
		"FD",
		"FE",
		"FF",
		"FG",
		"FH",
		"FI",
		"FJ",
		"FK",
		"FL",
		"FM",
		"FN",
		"FO",
		"FP",
	};
	//treatments with medBio
	private string[] medBioTreatmentArray = {
	};
	//high biosecurity uncertainty treatments
	private string[] highSquareUncertainty = {
		"FC",
		"FD",
		"FG",
		"FH",
		"FK",
		"FL",
		"FO",
		"FP",
		"FS",
		"FT",
		"FW",
		"FX",
		"GA",
		"GB",
		"GE",
		"GF",
	};
	//medium biosecurity uncertainty treatments
	private string[] medSquareUncertainty = {
	};
	//high infection uncertainty treatments
	private string[] highDotUncertainty = {
		"FB",
		"FD",
		"FF",
		"FH",
		"FJ",
		"FL",
		"FN",
		"FP",
		"FR",
		"FT",
		"FV",
		"FX",
		"FZ",
		"GB",
		"GD",
		"GF",
	};
	//medium infection uncertainty treatments
	private string[] medDotUncertainty = {
	};
	//low biosecurity cost treatments
	private string[] lowBioCost = {
	};
	//TREATMENT ARRAY TO BE PASSED IN FROM DATABASE
	public static string[] treatmentArray = {
		"FI",
		"FJ",
		"FK",
		"FL",
		"FM",
		"FN",
		"FO",
		"FP",
		"FY",
		"FZ",
		"GA",
		"GB",
		"GC",
		"GD",
		"GE",
		"GF",
	};
	//treatment with a gauge message
	public static string[] gaugeArray = {
		"FE",
		"FF",
		"FG",
		"FH",
		"FM",
		"FN",
		"FO",
		"FP",
		"FU",
		"FV",
		"FW",
		"FX",
		"GC",
		"GD",
		"GE",
		"GF",
	};
	//treatment with a high contagion rate
	public static string[] highContagionArray = {
		"FI",
		"FJ",
		"FK",
		"FL",
		"FM",
		"FN",
		"FO",
		"FP",
		"FY",
		"FZ",
		"GA",
		"GB",
		"GC",
		"GD",
		"GE",
		"GF",
	};
	private float[] distanceTable;
	float probInfect = 1f;
	private GameObject[] cleanDots;
	private GameObject[] infDots;
	public static int peekCost;


	// data tracking variables (push these gamestats to webdb per round...)
	public static int InfectionBool = 0;
	public static string MonthInfected = "None";
	public static string LowBioAdopted = "None";
	public static string MediumBioAdopted = "None";
	public static string HighBioAdopted = "None";
	public static string ProbInfNoneStr = "";
	public static string ProbInfLowStr = "";
	public static string ProbInfMedStr = "";
	public static string ProbInfHighStr = "";
	public static int TotalFarms;
	public static int NumFarmsBioVisible;
	public static int NumVisibleNoBio;
	public static int NumVisibleLowBio;
	public static int NumVisibleMedBio;
	public static int NumVisibleHighBio;
	public static string MessageType = "None";
	public static float ContagionRate = .08f;
	public static float LowBioScalar = .25f;       // multipiers for adding biosecurity (may change...)
	public static float MedBioScalar = .5f;
	public static float HighBioScalar = .75f;
	public static string DiseaseInfoTreatment;
	public static string BiosecurityInfoTreatment;
	public static string peekButtonMonthClicked;
	public static string biopeekButtonMonthClicked;
	public static string CurrentMonthText;
	public static string InformationTreatment;  // probability inf and gauges
	public  static bool PushDataOnce;
	public static bool CountDotsOnce;
	public static bool SetLastRoundOnce = true;
	public static bool DotInitComplete = false;  // trigger for after dots are finished initializing 
	public static bool DotInitComplete2 = false;  // trigger for after dots are finished initializing 
	public static bool DotInitComplete3 = false;  // overkill but very hard to test, so lets be safe...

	// look up table for agent table (resets every round)
	public static Dictionary<string, Dictionary<string, string>> Agents;

	void Start ()
	{
		// Low-certain treatments: "FA","FE","FI","FM",
		//set treatment array from login screen
		treatmentArray = PlayerLoginName.TreatmentArray;


		if (SetLastRoundOnce) {
			// setting last round, if necessary (returning users)
			sceneCount = PlayerLoginName.LastRound;
			SetLastRoundOnce = false;

		}


		// set round variables
		monthCounter = 6;
		ContagionRate = .08f;
		peekCost = 2000;
		baseProfit = 10000;
		profit2 = baseProfit;
		infectionCost = 25000; //40000;  // this will eventually depend on treatment...
		biosecurityCost = 1000; //2000;
		/*
		//for bisoecurity cost treatment
		foreach (string treatment in lowBioCost) {
			if (treatmentArray [sceneCount] == treatment) {
				bio	securityCost = 1000;
				break;
			}
		}
		*/
		infectionCostText.text = "Infection Cost: $" + infectionCost;
		biobutton.GetComponentInChildren<Text> ().text = "Increase Protection $" + biosecurityCost;
		if (treatmentArray [sceneCount] == "Z") {
			GameObject.Find("PracticeText").GetComponent<GUIText>().text = "Practice\nRound";
		} else {
			roundText.text = "";
		}


		DotInitComplete = false; // trigger for counting dots when init finishes (set in Biosecurity scripts...)
		DotInitComplete2 = false; bool CertaintyFlag = true;
		DotInitComplete3 = false; bool CertaintyFlag2 = true;
		//set biosecurity script
		foreach (string treatment in highBioTreatmentArray) {
			if (treatmentArray [sceneCount] == treatment) {
				GetComponent<highBiosecurity> ().enabled = true;
				break;
			}
		}
		//set biosecurity script
		foreach (string treatment in medBioTreatmentArray) {
			if (treatmentArray [sceneCount] == treatment) {
				GetComponent<MedBiosecurity> ().enabled = true;
				break;
			}
		}
		foreach (string treatment in lowBioTreatmentArray) {
			if (treatmentArray [sceneCount] == treatment) {
				GetComponent<LowBiosecurity> ().enabled = true;
				break;
			}
		}

		//set square uncertainty
		foreach (string treatment in highSquareUncertainty) {
			if (treatmentArray [sceneCount] == treatment) {
				GetComponent<SquareHighUncertainty> ().enabled = true;
				CertaintyFlag2 = false; 
				break;
			}
		}
		foreach (string treatment in medSquareUncertainty) {
			if (treatmentArray [sceneCount] == treatment) {
				GetComponent<SquareMediumUncertainty> ().enabled = true;
				CertaintyFlag2 = false; 
				break;
			}
		}
		//set dot uncertainty
		foreach (string treatment in highDotUncertainty) {
			if (treatmentArray [sceneCount] == treatment) {
				GetComponent<DotHighUncertainty> ().enabled = true;
				CertaintyFlag = false; 
				break;
			}
		}
		foreach (string treatment in medDotUncertainty) {
			if (treatmentArray [sceneCount] == treatment) {
				GetComponent<DotMediumUncertainty> ().enabled = true;
				CertaintyFlag = false; 
				break;
			}
		}
		foreach (string treatment in highContagionArray) {
			if (treatmentArray [sceneCount] == treatment) {
				ContagionRate = .3f;
				break;
			}
		}

		// on rounds with no uncertainty, set true
		if (CertaintyFlag) {  DotInitComplete2 = true;} // no certainty to init
		if (CertaintyFlag2) {  DotInitComplete3 = true;} // no certainty to init

		//store message state
		bool messageStateGauge=false;
		foreach (string treatment in gaugeArray) {
			if (treatmentArray [sceneCount] == treatment) {
				messageStateGauge = true;
				GameObject.Find ("ProbabilityText").GetComponent<Text> ().enabled = true;
				if (ContagionRate > .08f) {
					GameObject.Find ("HighGauge").GetComponent<Image> ().enabled = true;
					InformationTreatment = "GaugeHigh";
				}  else {
					GameObject.Find ("LowGauge").GetComponent<Image> ().enabled = true;
					InformationTreatment = "GaugeLow";
				}
				break;
			}
		}
		if (messageStateGauge == false && treatmentArray [sceneCount] != "Z") {
			GameObject.Find ("ProbabilityText").GetComponent<Text> ().enabled = true;
			if (ContagionRate > .08f) {
				GameObject.Find ("Verbal Messaging").GetComponent<Text> ().text = "High";
				InformationTreatment = "VerbalHigh";
			}  else {
				GameObject.Find ("Verbal Messaging").GetComponent<Text> ().text = "Low";
				InformationTreatment = "VerbalLow";
			}
		}
		// save the profit
		profit = PlayerPrefs.GetInt ("profit");
		profit = baseProfit;
		PlayerPrefs.DeleteKey ("profit");

		//initialize text elements
		nextRound = false;
		nextRoundText.text = "";
		biosecurityText.text = "Protection: None";

		UpdateProfit ();
		UpdateMonth ();

		// reset relevant data variables
		PushDataOnce = true;
		InfectionBool = 0;
		MonthInfected = "None";
		LowBioAdopted = "None";
		MediumBioAdopted = "None";
		HighBioAdopted = "None";
		ProbInfNoneStr = "";
		ProbInfLowStr = "";
		ProbInfMedStr = "";
		ProbInfHighStr = "";
		TotalFarms = 0;
		NumFarmsBioVisible = 0;
		NumVisibleNoBio = 0;
		NumVisibleLowBio = 0;
		NumVisibleMedBio = 0;
		NumVisibleHighBio = 0;
		MessageType = "None";
		DiseaseInfoTreatment = "Full";  // info treatments are "Full" or "None", default is "Full" 
		BiosecurityInfoTreatment = "Full";
		peekButtonMonthClicked="None";
		biopeekButtonMonthClicked = "None";
		CountDotsOnce = true;
		// re-init dictionary
		Agents = new Dictionary<string, Dictionary<string, string>>();


		// DEV: output treatment per lvl 
		print("Treatment= "+treatmentArray [sceneCount]);
		print("sceneCount= "+sceneCount.ToString());
	
	
	}


	void Update ()
	{
		// keep track of current month text
		CurrentMonthText = monthText.text;  // set this in update
	
		// get stats on all agents
		if (CountDotsOnce && DotInitComplete && DotInitComplete2 && DotInitComplete3) { 
			CountDots ();
			CountDotsOnce = false;
		}
			
		//move to next round
		if (nextRound) {

			// PUSH DATA HERE: (push regardless of whether they opt to start next round)
			if (PushDataOnce) {
				//Debug.Log ("NumFarmsBioVis="+NumFarmsBioVisible.ToString());
				Debug.Log("NumVisibleNoBio="+NumVisibleNoBio);

				PushDataOnce = false;
				DataHandler.PushEndRoundStats();
			}


			if (Input.GetKeyDown (KeyCode.N)) {
				//if not a practice round update profit
				if (treatmentArray [sceneCount] != "Z") {
					PlayerPrefs.SetInt ("profit", profit);
				}
					

				if (sceneCount >= treatmentArray.Length - 1) {
					Debug.Log ("Game Finished, Load Survey");
					//Time.timeScale = 0;
					SceneManager.LoadScene ("GameSurvey");
				} else {
					//RELOAD THE SCENE
					sceneCount++;
					SceneManager.LoadScene ("BiosecurityGame");
				}
			}
		}
	}

	// go through current game dots for data tracking stats
	void CountDots ()
	{
		cleanDots = GameObject.FindGameObjectsWithTag("cleanDot");
		infDots = GameObject.FindGameObjectsWithTag ("infDot");
		GameObject[] allDots = new GameObject[cleanDots.Length + infDots.Length];
		cleanDots.CopyTo(allDots, 0);
		infDots.CopyTo(allDots, cleanDots.Length);
		foreach (GameObject dot in allDots) {
			bool bvis = false;
			// init agent dictionary 
			Agents.Add(dot.name,new Dictionary<string,string>()); // init dictionary entry
			Agents[dot.name].Add("Position_X",dot.transform.position[0].ToString());	
			Agents[dot.name].Add("Position_Y",dot.transform.position[1].ToString());	
			Agents[dot.name].Add("MonthInfected","\"None\""); // init	
			Agents[dot.name].Add("BioHiddenBool","0"); // init	
			Agents[dot.name].Add("InfHiddenBool","0"); // init	
		
			// count biosecurity statuses
			bioSquare = dot.gameObject.transform.GetChild (0);

			// check if biosecurity is hidden
			if (bioSquare.GetComponent<SpriteRenderer> ().color == Color.gray) {
				Agents [dot.name] ["BioHiddenBool"] = "1";
				BiosecurityInfoTreatment = "None";
			} else {
				bvis = true;
			}

			// check if infect is hidden
			if (dot.GetComponent<SpriteRenderer> ().color == Color.gray) {
				Agents [dot.name] ["InfHiddenBool"] = "1";
				DiseaseInfoTreatment = "None"; // binary all or none
			}

			// check if seed infection
			if (dot.tag == "infDot") {
				Agents [dot.name] ["MonthInfected"] = "\""+CurrentMonthText+"\"";  // first month of play;
			}

			//Debug.Log ("bio= " + bioSquare.tag); //Dev

			//print (bioSquare.tag); //dev
			if (bioSquare.tag == "black") {
				if (bvis) {
					NumVisibleNoBio += 1;
					NumFarmsBioVisible += 1;
				}
				Agents[dot.name].Add("Biosecurity","\"None\""); // init	(add extra quotes for sql varchar entries)
	
			} else if (bioSquare.tag == "brown") {
				if (bvis) {
					NumVisibleLowBio += 1;
					NumFarmsBioVisible += 1;
				}
				Agents[dot.name].Add("Biosecurity","\"Low\""); // init	

			} else if (bioSquare.tag == "darkGreen") {
				if (bvis) {
					NumVisibleMedBio += 1;
					NumFarmsBioVisible += 1;
				}
				Agents[dot.name].Add("Biosecurity","\"Medium\""); // init	

			} else if (bioSquare.tag == "green") {
				if (bvis) {
					NumVisibleHighBio += 1;
					NumFarmsBioVisible += 1;
				}
				Agents[dot.name].Add("Biosecurity","\"High\""); // init	

			}
			//else{ Debug.Log("CountDots: Unidentified Dot Type !!");} 

		}
		// keep track of total farms
		TotalFarms = cleanDots.Length + infDots.Length;

	}

	void UpdateProfit ()
	{

		profitText.text = "Profit: $" + profit;
	}


	void UpdateMonth ()
	{
		monthText.text = "" + month [monthCounter];
		monthText.color = colors [monthCounter % colors.Length];
	}

	public void infectionPeek(){
		cleanDots = GameObject.FindGameObjectsWithTag("cleanDot");
		infDots = GameObject.FindGameObjectsWithTag ("infDot");
		GameObject[] allDots = new GameObject[cleanDots.Length + infDots.Length];
		cleanDots.CopyTo(allDots, 0);
		infDots.CopyTo(allDots, cleanDots.Length);
		ShuffleArray (allDots);
		int numRevealed = 0;
		foreach (GameObject dot in allDots) {
			if (dot.GetComponent<SpriteRenderer> ().color == Color.gray && (numRevealed < 10)) {
				if (dot.tag == "infDot") {
					dot.GetComponent<SpriteRenderer> ().color = Color.red;
				} else{
					dot.GetComponent<SpriteRenderer> ().color = Color.black;
				}
				numRevealed++;
			}
		}
		profit = profit - peekCost;
		profit2 = profit2 - peekCost;
		UpdateProfit ();
		GameObject.Find ("peekButton").GetComponent<Button> ().enabled = false;
		GameObject.Find ("peekButton").GetComponent<Image> ().enabled = false;
		GameObject.Find ("peekButton").GetComponentInChildren<Text> ().text = "";
		// update data variable
		peekButtonMonthClicked = monthText.text;

	}
	public void bioPeek(){
		cleanDots = GameObject.FindGameObjectsWithTag("cleanDot");
		infDots = GameObject.FindGameObjectsWithTag ("infDot");
		GameObject[] allDots = new GameObject[cleanDots.Length + infDots.Length];
		cleanDots.CopyTo(allDots, 0);
		infDots.CopyTo(allDots, cleanDots.Length);
		ShuffleArray (allDots);
		int numRevealed = 0;
		foreach (GameObject dot in allDots) {
			bioSquare = dot.gameObject.transform.GetChild (0);
			if (bioSquare.GetComponent<SpriteRenderer> ().color == Color.gray && (numRevealed < 10)) {
				if (bioSquare.tag == "black") {
					bioSquare.GetComponent<SpriteRenderer> ().color = Color.black;
					bioSquare.GetComponentInChildren<Text> ().text = "N";
				}
				//dark brown	
				else if (bioSquare.tag == "brown") {
					bioSquare.GetComponent<SpriteRenderer> ().color = brown;
					bioSquare.GetComponentInChildren<Text> ().text = "L";
				} else if (bioSquare.tag == "darkGreen") {
					bioSquare.GetComponent<SpriteRenderer> ().color = darkGreen;
					bioSquare.GetComponentInChildren<Text> ().text = "M";
				} else if (bioSquare.tag == "green") {
					bioSquare.GetComponent<SpriteRenderer> ().color = Color.green;
					bioSquare.GetComponentInChildren<Text> ().text = "H";
				}
				numRevealed++;
			}
		}
		profit = profit - peekCost;
		profit2 = profit2 - peekCost;
		UpdateProfit ();
		GameObject.Find ("peekButtonBio").GetComponent<Button> ().enabled = false;
		GameObject.Find ("peekButtonBio").GetComponent<Image> ().enabled = false;
		GameObject.Find ("peekButtonBio").GetComponentInChildren<Text> ().text = "";

		// update data variable
		biopeekButtonMonthClicked = monthText.text;

	}


	public void Infected (GameObject dot)
	{
		// month text is not updated while this is calculated... 
		string infMonth = month [monthCounter+1]; // seen on next month for player
		if (infMonth == "Next Year")
			infMonth = "End December"; // for facilites who get infected after final decision month
		
		//Initialize
		float rand;
		probInfect = 1f;
		List<float> distanceTable = new List<float> ();
		dots = GameObject.FindGameObjectsWithTag ("infDot");

		// save infection probabilties for myDot
		bool SaveProbs = false;
		//print (dot.name);
		if (dot.name == "myDot") {
			SaveProbs = true;
		}

		// Find distance between dots and infected dots excpet for themselves
		foreach (GameObject infect in dots) {
			

			if (infect.tag == "infDot" && Vector3.Distance (dot.transform.position, infect.transform.position) != 0) {
				//Debug.Log (dot.transform.position[0].ToString ());
				var distance = Vector3.Distance (dot.transform.position, infect.transform.position);
				//add to list
				distanceTable.Add (distance);


			}
				
		}
		// Turn list into array
		distanceTable.ToArray ();

		// 1- 1/distnace sqared multiplied to all values in array
		foreach (float infNode in distanceTable) {

			probInfect = probInfect * (1 - (ContagionRate / (infNode * infNode)));
		}

		probInfect = 1 - probInfect;

		rand = Random.Range (0.0f, 1.0f);

		// square objects 
		bioSquare = dot.gameObject.transform.GetChild (0);

		// infections based off square bio security level
		if (bioSquare.tag == "black") {
			
			if (rand < (probInfect)) {

				if (dot.tag == "cleanDot"){ // first time infected, save month
					Agents [dot.name] ["MonthInfected"] = "\""+infMonth+"\"";
				}
					
				dot.tag = "infDot";

				if (dot.GetComponent<SpriteRenderer> ().color != Color.gray) {
					dot.GetComponent<SpriteRenderer> ().color = Color.red;
				}
				
			}
		}
		//dark brown	
		else if (bioSquare.tag == "brown") {
			
			if (rand < (probInfect * 0.75f)) {

				if (dot.tag == "cleanDot"){ // first time infected, save month
					Agents [dot.name] ["MonthInfected"] = "\""+infMonth+"\"";
				}

				dot.tag = "infDot";

				if (dot.GetComponent<SpriteRenderer> ().color != Color.gray) {
					dot.GetComponent<SpriteRenderer> ().color = Color.red;
				}
			}
		} else if (bioSquare.tag == "darkGreen") {

			if (rand < (probInfect * 0.5f)) {

				if (dot.tag == "cleanDot"){ // first time infected, save month
					Agents [dot.name] ["MonthInfected"] = "\""+infMonth+"\"";
				}

				dot.tag = "infDot";

				if (dot.GetComponent<SpriteRenderer> ().color != Color.gray) {
					dot.GetComponent<SpriteRenderer> ().color = Color.red;
				}
			}
		} else if (bioSquare.tag == "green") {

			if (rand < (probInfect * 0.25f)) {

				if (dot.tag == "cleanDot"){ // first time infected, save month
					Agents [dot.name] ["MonthInfected"] = "\""+infMonth+"\"";
				}

				dot.tag = "infDot";

				if (dot.GetComponent<SpriteRenderer> ().color != Color.gray) {
					dot.GetComponent<SpriteRenderer> ().color = Color.red;
				}
			}
		}

		// save the probabilities for each biosecurity type (MAKE SURE FLOATS (i.e. 0.75f) AGREE WITH ABOVE!!!)
		if (SaveProbs) {
			ProbInfNoneStr = ProbInfNoneStr + probInfect.ToString() + ",";
			ProbInfLowStr = ProbInfLowStr + (probInfect * 0.75f).ToString () + ",";
			ProbInfMedStr = ProbInfMedStr + (probInfect * 0.5f).ToString () + ",";
			ProbInfHighStr = ProbInfHighStr + (probInfect * 0.25f).ToString () + ",";
		}

	
	}


	public void IncreaseBio (GameObject dot)
	{

		bioSquare = dot.gameObject.transform.GetChild (0);

		if (bioSquare.GetComponent<SpriteRenderer> ().color == Color.green) {



		}
		//dark green
		else if (bioSquare.GetComponent<SpriteRenderer> ().color == darkGreen) {

			bioSquare.GetComponent<SpriteRenderer> ().color = Color.green;
			bioSquare.tag = ("green");
			profit = profit - biosecurityCost;
			profit2 = profit2 - biosecurityCost;
			biosecurityText.text = "Protection: High";
			UpdateProfit ();
			biobutton.interactable = false;
			bioSquare.GetComponentInChildren<Text> ().text = "H";

			// save month of adoption
			HighBioAdopted = CurrentMonthText;

		}

		//if brown
		else if (bioSquare.GetComponent<SpriteRenderer> ().color == brown) {
			//dark green
			bioSquare.GetComponent<SpriteRenderer> ().color = darkGreen;
			biosecurityText.text = "Protection: Medium";
			bioSquare.tag = ("darkGreen");
			profit = profit - biosecurityCost;
			profit2 = profit2 - biosecurityCost;
			UpdateProfit ();
			biobutton.interactable = false;
			bioSquare.GetComponentInChildren<Text> ().text = "M";

			// save month of adoption
			MediumBioAdopted = CurrentMonthText;


		} else if (bioSquare.GetComponent<SpriteRenderer> ().color == Color.black) {
			//brown
			bioSquare.GetComponent<SpriteRenderer> ().color = brown;
			biosecurityText.text = "Protection: Low";
			bioSquare.tag = ("brown");
			profit = profit - biosecurityCost;
			profit2 = profit2 - biosecurityCost;
			UpdateProfit ();
			biobutton.interactable = false;
			bioSquare.GetComponentInChildren<Text> ().text = "L";

			// save month of adoption
			LowBioAdopted = CurrentMonthText;

		}

		nextButton.onClick.Invoke ();
	}

	public void GameOver ()
	{
 		
		roundText.text = "";
		//find your dot
		myDot = GameObject.Find ("myDot");
		monthCounter++;
		UpdateMonth ();
		//find biosquare for limiting one bio update per month
		bioSquare = myDot.gameObject.transform.GetChild (0);
		//end game if your dot becomes infected
		if (myDot.tag == ("infDot")) {

			// record infection
			InfectionBool = 1;
			if (MonthInfected == "None") { // only set once
				MonthInfected = CurrentMonthText;
				//MonthInfected = month [monthCounter];
				Debug.Log ("INFECTED!");
				Debug.Log (MonthInfected);
			}


			profit = profit - infectionCost;
			profit2 = profit2 - infectionCost;
			UpdateProfit ();
			if (treatmentArray [sceneCount] != "Z") {
				totalProfit += profit;
				roundText.text = "Infection! Round Over \n You earned $" + profit2 + " this Round\n Total Profit: $" + totalProfit;
			} else {
				GameObject.Find("PracticeText").GetComponent<GUIText>().text = "";
				roundText.text = "Infection! Round Over \n You earned $" + profit2 + " this Round";
			}
			nextRoundText.text = "Press N for next Round";
			GameObject.Find ("Main Camera").GetComponent<Transform> ().position = new Vector3 (0, 0, 0);
			GameObject.Find ("monthText").GetComponent<GUIText> ().text = "";
			GameObject.Find ("biosecurityText").GetComponent<GUIText> ().text = "";
			GameObject.Find ("profitText").GetComponent<GUIText> ().text = "";
			GameObject.Find ("infectionCostText").GetComponent<GUIText> ().text = "";
			GameObject.Find ("ImageCanvas").GetComponentInChildren<Image> ().enabled = false;
			GameObject.Find ("ProbabilityText").GetComponent<Text> ().enabled = false;
			GameObject.Find ("Verbal Messaging").GetComponent<Text> ().text = "";
			GameObject.Find ("HighGauge").GetComponent<Image> ().enabled = false;
			GameObject.Find ("LowGauge").GetComponent<Image> ().enabled = false;
			nextButton.GetComponent<Button>().enabled = false;
			nextButton.GetComponent<Image>().enabled = false;
			nextButton.GetComponentInChildren<Text> ().text = "";
			biobutton.GetComponent<Button>().enabled = false;
			biobutton.GetComponent<Image>().enabled = false;
			biobutton.GetComponentInChildren<Text> ().text = "";
			GameObject.Find ("peekButton").GetComponent<Button> ().enabled = false;
			GameObject.Find ("peekButton").GetComponent<Image> ().enabled = false;
			GameObject.Find ("peekButton").GetComponentInChildren<Text> ().text = "";
			GameObject.Find ("peekButtonBio").GetComponent<Button> ().enabled = false;
			GameObject.Find ("peekButtonBio").GetComponent<Image> ().enabled = false;
			GameObject.Find ("peekButtonBio").GetComponentInChildren<Text> ().text = "";
			nextRound = true;
		}
		// end game if you complete a year
		else if (month [monthCounter] == "Next Year") {
			if (treatmentArray [sceneCount] != "Z") {
				totalProfit += profit;
				roundText.text = "Round Over \n You earned $" + profit2 + " this Round\n Total Profit: $" + totalProfit;
			} else {
				GameObject.Find("PracticeText").GetComponent<GUIText>().text = "";
				roundText.text = "Round Over \n You earned $" + profit2 + " this Round";
			};
			nextRoundText.text = "Press N for next Round";
			GameObject.Find ("Main Camera").GetComponent<Transform> ().position = new Vector3 (0, 0, 0);
			GameObject.Find ("monthText").GetComponent<GUIText> ().text = "";
			GameObject.Find ("biosecurityText").GetComponent<GUIText> ().text = "";
			GameObject.Find ("profitText").GetComponent<GUIText> ().text = "";
			GameObject.Find ("infectionCostText").GetComponent<GUIText> ().text = "";
			GameObject.Find ("ImageCanvas").GetComponentInChildren<Image> ().enabled = false;
			GameObject.Find ("ProbabilityText").GetComponent<Text> ().enabled = false;
			GameObject.Find ("Verbal Messaging").GetComponent<Text> ().text = "";
			GameObject.Find ("HighGauge").GetComponent<Image> ().enabled = false;
			GameObject.Find ("LowGauge").GetComponent<Image> ().enabled = false;
			nextButton.GetComponent<Button>().enabled = false;
			nextButton.GetComponent<Image>().enabled = false;
			nextButton.GetComponentInChildren<Text> ().text = "";
			biobutton.GetComponent<Button>().enabled = false;
			biobutton.GetComponent<Image>().enabled = false;
			biobutton.GetComponentInChildren<Text> ().text = "";
			GameObject.Find ("peekButton").GetComponent<Button> ().enabled = false;
			GameObject.Find ("peekButton").GetComponent<Image> ().enabled = false;
			GameObject.Find ("peekButton").GetComponentInChildren<Text> ().text = "";
			GameObject.Find ("peekButtonBio").GetComponent<Button> ().enabled = false;
			GameObject.Find ("peekButtonBio").GetComponent<Image> ().enabled = false;
			GameObject.Find ("peekButtonBio").GetComponentInChildren<Text> ().text = "";
			nextRound = true;
		} else if (bioSquare.GetComponent<SpriteRenderer> ().color == brown || bioSquare.GetComponent<SpriteRenderer> ().color == darkGreen) {
			biobutton.interactable = true;
		}
	}
	// shuffling function
	public static void ShuffleArray<T> (T[] arr)
	{
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range (0, i + 1);
			T tmp = arr [i];
			arr [i] = arr [r];
			arr [r] = tmp;
		}
	}


}