using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeout : MonoBehaviour {


	void OnGUI () {

		// set fontstyle for gui display
		GUIStyle CntStyle = new GUIStyle();
		//smallFont.fontSize = 10;
		CntStyle.fontSize = 50;
		CntStyle.normal.textColor= Color.white;

		GUI.skin.label.fontSize = 30;
		GUI.skin.textField.fontSize = 30;
	
		GUI.Label (new Rect (200, 150, 155, 30), "Network Connection Failure!\nPlease Reload your browser!", CntStyle);

	}

}
