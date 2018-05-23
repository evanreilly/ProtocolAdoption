using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;

// push data to silk server for storage in webdb-mysql database
// Author: @eclark66

public class DataHandler : MonoBehaviour {

	// current random seed
	//public static int randomSeed;
	// random number generator
	//public static System.Random getRandom = new System.Random();

	// Use this for initialization
	void Start () {

		// set random seed
		//randomSeed = getRandom.Next(1, 10000);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	// call this from gamecontroller at the end of each round
	public static void PushEndRoundStats(){

		string ScriptURL = "https://segs.w3.uvm.edu/gamescripts/pa_DB_gamestats.php";

		// dont save practice rounds
		if  (gameController.treatmentArray [gameController.sceneCount] == "Z"){ return;} 

		// retool this to push end level stats
		WWWForm form = new WWWForm();

		// Session ID
		form.AddField ("sessionId", PlayerLoginName.SessionID);

		form.AddField ("AMT_flag", (string)PlayerLoginName.AMT_flag.ToString());

		// Total Time Played
		var CurrentTime = Convert.ToInt64 ((DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalMilliseconds);
		var TimeElapsed = CurrentTime - PlayerLoginName.StartTime;
		form.AddField ("time", TimeElapsed.ToString());

		// Game Variables
		//form.AddField("treatment",gameController.sceneCount.ToString());
		form.AddField("treatment",(gameController.sceneCount-1).ToString()); // subtract the practice round

		form.AddField("treatmentType", gameController.treatmentArray [gameController.sceneCount]);
		form.AddField("sessionScore", gameController.totalProfit.ToString ());
		form.AddField("score",gameController.profit2.ToString ());
		form.AddField("infectionBool",gameController.InfectionBool.ToString ());

		if (gameController.InfectionBool == 1) {
			form.AddField ("monthInfected",gameController.MonthInfected );
		} else {
			form.AddField ("monthInfected" , "None" );
		}
			
		form.AddField ("lowBioAdopted", gameController.LowBioAdopted);
		form.AddField("mediumBioAdopted" , gameController.MediumBioAdopted);
		form.AddField ("highBioAdopted" , gameController.HighBioAdopted);
		form.AddField ("probInfNoneStr" , gameController.ProbInfNoneStr);
		form.AddField("probInfLowStr" , gameController.ProbInfLowStr);
		form.AddField("probInfMedStr" , gameController.ProbInfMedStr);
		form.AddField("probInfHighStr" , gameController.ProbInfHighStr);
		form.AddField("totalFarms" , gameController.TotalFarms.ToString());
		form.AddField("numFarmsBioVisible" , gameController.NumFarmsBioVisible.ToString());
		form.AddField("numVisibleNoBio" , gameController.NumVisibleNoBio.ToString());
		form.AddField("numVisibleLowBio" , gameController.NumVisibleLowBio.ToString());
		form.AddField("numVisibleMedBio" , gameController.NumVisibleMedBio.ToString());
		form.AddField("numVisibleHighBio" , gameController.NumVisibleHighBio.ToString());
		form.AddField("messageType" , gameController.MessageType);
		form.AddField("contagionRate" , gameController.ContagionRate.ToString());
		form.AddField("infectionCost" , gameController.infectionCost.ToString ());
		form.AddField("informationTreatment" , gameController.InformationTreatment);
		form.AddField("diseaseInfoTreatment" , gameController.DiseaseInfoTreatment);
		form.AddField("bioInfoTreatment" , gameController.BiosecurityInfoTreatment);
		form.AddField("peekButtonMonthClicked" , gameController.peekButtonMonthClicked);
		form.AddField("bioPeekButtonMonthClicked" , gameController.biopeekButtonMonthClicked.ToString());
		form.AddField("peekButtonCost" , gameController.peekCost.ToString());
		form.AddField ("lowBioScalar", gameController.LowBioScalar.ToString ());
		form.AddField ("medBioScalar", gameController.MedBioScalar.ToString ());
		form.AddField ("highBioScalar", gameController.HighBioScalar.ToString ());

		// Push to Server
		WWW www = new WWW(ScriptURL, form);

		// dev
		PrintEndRoundStats();

		// Finally post agent stats
		PushAgentData();
	
	}

	// pushes the gamecontroller.Agent Dictionary every round 
	public static void PushAgentData(){

		string ScriptURL = "https://segs.w3.uvm.edu/gamescripts/pa_DB_PushAgents.php";

		// retool this to push end level stats
		WWWForm form = new WWWForm();

		// Session ID
		form.AddField ("sessionId", PlayerLoginName.SessionID);

		// Game Variables
		//form.AddField("treatment",gameController.sceneCount.ToString());
		form.AddField("treatment",(gameController.sceneCount-1).ToString()); // subtract the practice round

		form.AddField ("AMT_flag", (string)PlayerLoginName.AMT_flag.ToString());

		// Parse Agent data nested Dictionary (from gameController)
		StringBuilder AgentPostStr = new StringBuilder();
		foreach (string agentName in gameController.Agents.Keys) {
			// add the agentname
			AgentPostStr.Append("AgentName:"+"\""+agentName+"\"");
			foreach (string datakey in gameController.Agents[agentName].Keys) {
				AgentPostStr.Append (","+datakey + ":" + gameController.Agents [agentName] [datakey]);
			}
			AgentPostStr.Append(";");
		}

		//Debug.Log (AgentPostStr.ToString().TrimEnd(';'));
		// Game Variables
		form.AddField("agents",AgentPostStr.ToString().TrimEnd(';')); // also snip trailing semicolon
		//form.AddField("agents",AgentPostStr.ToString()); // also snip trailing semicolon


		// Push to Server
		WWW www = new WWW(ScriptURL, form);

	}



	// for dev, print out these stats every round
	public static void PrintEndRoundStats(){

		// TODO: Test each of these data params, build php script to push to db

		// Total Time Played
		var CurrentTime = Convert.ToInt64 ((DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalMilliseconds);
		var TimeElapsed = CurrentTime - PlayerLoginName.StartTime;

		// print to console (dev)
		//Debug.Log("Treatment: "+gameController.sceneCount);
		//Debug.Log ("Treatment Type: " + gameController.treatmentArray [gameController.sceneCount]);
		//Debug.Log ("timeElapsed: " + TimeElapsed.ToString ());
		Debug.Log ("Session Profit: " + gameController.totalProfit.ToString ());
		Debug.Log ("Round Profit: " + gameController.profit2.ToString ());
		//Debug.Log ("Infection Bool: " + gameController.InfectionBool.ToString ());

		if (gameController.InfectionBool == 1) {
			//Debug.Log ("Month Infected: " + gameController.CurrentMonthText );
		} else {
			//Debug.Log ("Month Infected: " + "None" );
		}

		//Debug.Log ("LowBioAdopted: " + gameController.LowBioAdopted);
		//Debug.Log ("MedBioAdopted: " + gameController.MediumBioAdopted);
		//Debug.Log ("HighBioAdopted: " + gameController.HighBioAdopted);
		//Debug.Log ("ProbInfNoneStr: " + gameController.ProbInfNoneStr);
		//Debug.Log ("ProbInfLowStr: " + gameController.ProbInfLowStr);
		//Debug.Log ("ProbInfMedStr: " + gameController.ProbInfMedStr);
		//Debug.Log ("ProbInfHighStr: " + gameController.ProbInfHighStr);
		//Debug.Log ("Total Farms: " + gameController.TotalFarms);
		//Debug.Log ("Num Farms Low Bio Visible: " + gameController.NumVisibleLowBio);
		//Debug.Log ("Num Farms Med Bio Visible: " + gameController.NumVisibleMedBio);
		//Debug.Log ("Num Farms High Bio Visible: " + gameController.NumVisibleHighBio);
		//Debug.Log ("Message Type: " + gameController.MessageType);
		Debug.Log ("Contagion Rate: " + gameController.ContagionRate.ToString());
		//Debug.Log ("Infection Cost: " + gameController.infectionCost.ToString ());
		//Debug.Log ("Disease Info Treatment: " + gameController.DiseaseInfoTreatment);
		//Debug.Log ("Biosecurity Info Treatment: " + gameController.BiosecurityInfoTreatment);
		//Debug.Log ("Peek Button Month Picked: " + gameController.peekButtonMonthClicked);
		//Debug.Log ("BioPeek Button Month Picked: " + gameController.biopeekButtonMonthClicked);
		//Debug.Log ("Peek Button Cost: " + gameController.peekCost);
	
	}



}
