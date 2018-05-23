using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SlideShow : MonoBehaviour {
	#region Support class
	[System.Serializable]
	public class Logo{
		public Texture2D image;
		public Color background;
		public float duration = 60.0f;
		public bool skippable = true;
	}
	[System.Serializable]
	public class KeySet{
		public bool useTouches = true;
		public int numberOfTouchesNeeded = 1;
		public KeyCode[] keysToUse;
	}
	#endregion
	#region Public variables
	public Logo[] logos;
	public KeySet exitWith;
	public string levelToLoad;
	public GUITexture logosScreen;
	public Animation fader;
	#endregion
	#region Private variables
	private int currentLogo = -1;
	private float startTime = 0.0f;
	private float logoMaxWidth, logoMaxHeight,
				actualWidth, actualHeight, texAspect;
	private bool transition = false;
	#endregion
	// button value for restarting the intro slide show
	public static bool RestartSlideShowPressed = false;
	public static bool ShowFinishButtons = false;
	public static int InputButtonSlideNumber = 4;
	// button value for continuing to the game
	public static bool FinishSlideShowPressed = false;
	public GUIStyle GuiFont;
    // buttons for replaying info
    public static bool RestartInstructionsPressed = false;
	public static bool ShowInstructionButtons = false;
	public static int RestartInstructionSlideNumber = 5;
	public static bool ContinueToGameButtonPressed = false;

	#region Built-in methods
	void Start(){
		
		// set fontstyle for gui display
		//GuiFont = new GUIStyle();
		//smallFont.fontSize = 10;
		//GuiFont.fontSize = 32;
		//GuiFont.normal.textColor= Color.white;
        //GUI.skin.button.fontSize = 30;
        this.logoMaxWidth = Screen.width * 1.0f;  //.8f
		this.logoMaxHeight = Screen.height * 1.0f; //.8f
		StartCoroutine("SwitchLogo");

    }
	void Update(){
		if(!ShowInstructionButtons && !ShowFinishButtons && !this.transition && (
								Time.time - this.startTime > this.logos[this.currentLogo].duration ||
									(
									this.logos[this.currentLogo].skippable &&
									GetSkipButtonPessed()
									)
								)
			)
			StartCoroutine("SwitchLogo");

		// at end of slide show, check for gui pressed buttons
		if (RestartSlideShowPressed){
			RestartSlideShowPressed = false;
			this.currentLogo = -1;
			StartCoroutine ("SwitchLogo");
		}
		if (FinishSlideShowPressed) {
			FinishSlideShowPressed = false;
			//Application.LoadLevel(this.levelToLoad);
			//this.currentLogo++;
			this.currentLogo = InputButtonSlideNumber+1;
			StartCoroutine ("SwitchLogo");
		}

		if (RestartInstructionsPressed) {
			// restart instructions if chosen
			RestartInstructionsPressed=false;
			this.currentLogo = RestartInstructionSlideNumber;
			StartCoroutine ("SwitchLogo");


		}

		if (ContinueToGameButtonPressed) {
			// load up the game if chosen
			Application.LoadLevel(this.levelToLoad);

		}


			
	}
	#endregion
	#region Private methods
	private IEnumerator SwitchLogo(){
		this.transition = true;
		this.fader.Play("FadeOut");
		yield return new WaitForSeconds(0.5f);
		if (this.currentLogo == InputButtonSlideNumber) {
			ShowFinishButtons = true;

		}
		else if (this.currentLogo + 1 < this.logos.Length) {
			this.currentLogo++;
			this.texAspect = (float)((float)this.logos [this.currentLogo].image.width / (float)this.logos [this.currentLogo].image.height);
			this.actualWidth = this.logoMaxWidth;
			this.actualHeight = this.actualWidth / this.texAspect;
			if (this.actualHeight > this.logoMaxHeight) {
				this.actualHeight = this.logoMaxHeight;
				this.actualWidth = this.actualHeight * this.texAspect;
			}
			this.logosScreen.pixelInset = new Rect (
				-(this.actualWidth * 0.5f),
				-(this.actualHeight * 0.5f),
				this.actualWidth,
				this.actualHeight
			);
			this.logosScreen.texture = this.logos [this.currentLogo].image;
			try {
				Camera.main.backgroundColor = this.logos [this.currentLogo].background;
			} catch {
			}
			// Add Forced Delay here?
			this.fader.Play ("FadeIn");
			yield return new WaitForSeconds (0.5f);
			this.startTime = Time.time;
			this.transition = false;
		} else{
			    // Load the level at end of slideshow...
				//ShowFinishButtons = true;
				// Application.LoadLevel(this.levelToLoad);
		     	// user can decide to play or replay instructions
				ShowInstructionButtons = true;
			}
		}
	private bool GetSkipButtonPessed(){
		if(this.exitWith.useTouches && Input.touches.Length == this.exitWith.numberOfTouchesNeeded)
			return true;
		foreach(KeyCode k in this.exitWith.keysToUse)
			if(Input.GetKeyDown(k))	return true;
		return false;
	}
	#endregion

// display restart button ongui

	void OnGUI () {

		// set fontstyle for gui display
		GuiFont = new GUIStyle();
		//smallFont.fontSize = 10;
		GuiFont.fontSize = 32;
		GuiFont.normal.textColor= Color.white;

        if (SceneManager.GetActiveScene().name == "IntroSlides")
        {
            GUI.skin.button.fontSize = 25;
        }
		// display text box for user to enter name
		if (ShowFinishButtons) {

			GUI.Label (new Rect (200, 50, 155, 30), "Please Choose an Option",GuiFont);
			if (GUI.Button (new Rect (150, 120, 550, 100), "I consent to participate in this research study")) {
				ShowFinishButtons = false;
				FinishSlideShowPressed = true;
			}


			if (GUI.Button (new Rect (150, 270, 550, 100), "I'd like to review the information sheet")) {
				ShowFinishButtons = false;
				RestartSlideShowPressed = true;
			}
				
			
			// Game Instructions
			//GUI.Label (new Rect (200, 150, 350, 30), "Go for the high score.  Keep your pigs alive. Good Luck.");
			// Interactive Label (user input)

		}

		if (ShowInstructionButtons) {
			// display these buttons after instructions (allow replay instructions)
			GUI.Label (new Rect (200, 50, 155, 60), "Instructions Completed. \nPlease Choose an Option",GuiFont);

			if (GUI.Button (new Rect (150, 140, 550, 100), "I'd like to review the instructions.")) {
				ShowInstructionButtons = false;
				RestartInstructionsPressed = true;
			}


			if (GUI.Button (new Rect (150, 280, 550, 100), "I'm ready to play the game!")) {
				ShowInstructionButtons = false;
				ContinueToGameButtonPressed = true;
			}
			


		}


	}



}


