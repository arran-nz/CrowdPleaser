using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stadium : MonoBehaviour {

	private UserInterface _UI;

	[Header("Stadium Size")]
	public int _HEIGHT;
	public int _WIDTH;
	
	[Header("Player Position")]
	public bool player_RandPos;
	public int playerPos_X;
	public int playerPos_Y;
	private bool playerJumping;

	[Header("Person Position")]
	public ColorPalette playerPalette;
	public GameObject[] _PersonOBJ;
	public Vector3 startPos;
	public Vector3 personSpacing;

	private int randPerson;

	[Header("Waves")]
	[Range(2F,5F)]
	public float waveFreq_MIN = 3F;
	private float sWF_Mn;
	[Range(2.5F,7F)]
	public float waveFreq_MAX = 8F;	private float sWF_Mx;
	private float waveFreq = 5F;
	public float waveDelay = 0.1F;	private float s_WD;


	private float score;
	private bool onStreak;
	private bool lastChance;
	private int streakCount = 0;

	[Header("Scoring")]
	public int diffWaveStep = 10;
	public int diffThresStep = 25;
	public float diffMinWD = 0.07F;
	public float diffMinWF = 2F;

	[Header("Thresholds")]
	public float winThres = 0.15F; private float s_WT;
	public float goodThres = 0.10f; private float s_GT;
	public float greatThres = 0.05F; private float s_GRT;
	public float perfectThres = 0.025F; private float s_PT;

	[Header("Speech")]
	public Color cPerfect;
	public string[] sPerfect;
	public Color cGreat;
	public string[] sGreat;
	public Color cGood;
	public string[] sGood;
	public Color cAlright;
	public string[] sAlright;
	public Color cBoo;
	public string[] sBoo;
	public string[] sWellDone;

	[Header("Sound")]
	public AudioClip win;
	public AudioClip lose;
	public AudioClip jump;
	public AudioClip jumpFail;
	private AudioSource[] _Audio;

	// MENU 
	private MenuController _MenuController;

	private float time;

	private Person[,] _PersonArray;
	private Person _Player;


	public bool _Waves = true;
	private bool _GameOn;

	// Use this for initialization
	void Start () {

		if (player_RandPos) {
			playerPos_X = Random.Range (3, 5);
			playerPos_Y = Random.Range (4, 6);
		}

		//SET WAVES DEFAULTS
		sWF_Mn = waveFreq_MIN;
		sWF_Mx = waveFreq_MAX;
		s_WD = waveDelay;
		// SET THRES DEFAULTS
		s_WT = winThres;
		s_GT = goodThres;
		s_GRT = greatThres;
		s_PT = perfectThres;

		AllocateSeating ();
		_Player.AllocatePlayer (playerPalette);
		_UI = GameObject.FindObjectOfType<UserInterface> ();
		_Audio = GetComponents<AudioSource> ();
		_MenuController = GameObject.FindObjectOfType<MenuController> ();
	}

	// Update is called once per frame
	void Update () {

		//INPUT FOR PC
		if(Input.GetKeyDown(KeyCode.Space))
		{
			PlayerJump ();
		}

		// GAME
		if (_Waves) {
			MakeWaves ();
		}

		// DEBUG
		if(Input.GetKeyDown(KeyCode.X))
		{
			Time.timeScale = 0.2f;
		}
		if(Input.GetKeyDown(KeyCode.C))
		{
			Time.timeScale = 1f;
		}

	}
	public void PauseGame(bool pause)
	{
		if (pause) {
			Time.timeScale = 0F;

		} else {
			Time.timeScale = 1F;
		}
	}

	public void StartGame() //START NEW GAME
	{
		_GameOn = true; _Waves = true;
		waveFreq = 5F;

		//CHANGE VALUE TO ORIGINAL VALUES - WAVES
		waveFreq_MIN = sWF_Mn;
		waveFreq_MAX = sWF_Mx;
		waveDelay = s_WD;

		// THRESHOLDS
		winThres = s_WT;
		goodThres = s_GT;
		greatThres = s_GRT;
		perfectThres = s_PT;

	}

	IEnumerator EndGame (float p_Score,int p_Streak)
	{
		_GameOn = false; _Waves = false;
		SpeechFromArray (sWellDone, 1, Color.white);
		yield return new WaitForSeconds (3F);
		score = 0; streakCount = 0;
		_MenuController.SetupEndGame (p_Score, p_Streak);
	}

	public void PlayerJump()
	{
		if (!playerJumping && _GameOn) {
			playerJumping = true;
			_Player.Wave ();
			_Audio[0].PlayOneShot (jump);
			StartCoroutine (NotJump ());
		}
	}
	IEnumerator NotJump()
	{
		yield return new WaitForSeconds (1F);
		playerJumping = false;
	}

	void MakeWaves()
	{
		time += Time.deltaTime;
		if (time > waveFreq) {
			StartCoroutine (CreateWave(Random.Range(0,4))); // RANDOM DIR
			waveFreq = Random.Range(waveFreq_MIN, waveFreq_MAX);
			time = 0;
		}
	}


	void InreaseWave()
	{			
		_UI.UpdateScreen ("Speed Increased!");

		if (waveFreq_MIN > diffMinWF) {
			waveFreq_MIN  = waveFreq_MIN - 0.25F; 
		}
		if (waveDelay > diffMinWD) {
			waveDelay = waveDelay - 0.01F;
		}
	}

	void DecreaseThres()
	{



	}

	void AllocateSeating()
	{
		_PersonArray = new Person[_HEIGHT, _WIDTH];

		Quaternion rot = new Quaternion (0, 0, 0, 0);
		Vector3 pos = startPos * this.transform.localScale.x; 		// SCALE CORRECT RELATIVE TO PARENT
		pos = new Vector3 (pos.x, pos.y, 1);						//SET Z SO SORTS CORRECTLY
		for (int j = 0; j < _HEIGHT; j++) {


			for (int i = 0; i < _WIDTH; i++) {
				
				randPerson = Random.Range (0, _PersonOBJ.Length);

				GameObject clone = Instantiate (_PersonOBJ[randPerson], pos, rot, this.gameObject.transform);
				_PersonArray [j, i] =	clone.GetComponent<Person> ();

				//ASSIGN PLAYER
				if (j == playerPos_Y && i == playerPos_X) {
					_Player = _PersonArray [j, i];
					_Player.AllocatePlayer (playerPalette);
				} else {
					_PersonArray [j, i].AllocateSupporter ();
				}
					


				pos = new Vector3 (pos.x + (personSpacing.x * this.transform.localScale.x/2), pos.y, pos.z);

			}	

			pos = new Vector3 (startPos.x * this.transform.localScale.x , pos.y - personSpacing.y * this.transform.localScale.y, -(j));
		}
			
	}

	void CheckIfPlayer (int Y, int X, int dir)
	{
		// PLAYER
		if ((Y == playerPos_Y) && (X == playerPos_X)&&(_GameOn)) {

			switch (dir) {

			case 0: 
				// CHECK LEFT SEAT
				StartCoroutine(GenerateScore (Y, X - 1));
				break;

			case 1:
				// CHECK TOP SEAT
				StartCoroutine(GenerateScore(Y-1, X));
				break;

			case 2:
				// CHECK RIGHT SEAT
				StartCoroutine(GenerateScore(Y, X + 1));
				break;

			case 3:
				// CHECK BOTTOM SEAT
				StartCoroutine(GenerateScore(Y+1, X));
				break;

			default:
				break;
			}
		} 

		// NOT PLAYER
		else {
			
			_PersonArray [Y, X].Wave ();
		}

	}
	IEnumerator GenerateScore(int Y, int X)
	{
		float perfectTime = 0.5F; // HALFWAY THOUGH ANIM
		float nScore = 0;

		yield return new WaitForSeconds (perfectTime);

		//Debug.Log ((_Player.animTime) + "<- TIME / GOAL ->" + perfectTime);


		if ((WithinThreshold (perfectTime, winThres))&&((_Player.animTime != 0))) {

			if (WithinThreshold (perfectTime, perfectThres)) {
				//PERFECT
				nScore = 200;
				SpeechFromArray (sPerfect, 0, cPerfect);

			} else if (WithinThreshold (perfectTime, greatThres)) {
				// GREAT			
				nScore = 100;
				SpeechFromArray (sGreat, 0, cGreat);
			} else if (WithinThreshold (perfectTime, goodThres)) {
				// GOOD			
				nScore = 50;
				SpeechFromArray (sGood, 0, cGood);
			} else {
				// ALRIGHT			
				nScore = 20;
				SpeechFromArray (sAlright, 0, cAlright);
			}

			_Audio[1].PlayOneShot (win);

			if (onStreak) {

				if (_Audio[1].pitch < 2) {
					_Audio[1].pitch += 0.01F;
				}
				streakCount++;

				//INCREASE WAVES
				if (streakCount % diffWaveStep == 0) {
					InreaseWave ();
					StartCoroutine( _UI.ReturnScreenDelay () );
				}
				//DECREASE THRESHOLDS
				if (streakCount % diffThresStep == 0) {
					DecreaseThres ();
					StartCoroutine( _UI.ReturnScreenDelay () );
				}

			} 
			else 
			{
				
				onStreak = true;
				streakCount = 1;
				_UI.ReturnScreen ();

			}
		}
 

		else { // PLAYER FAILED
			
			_Audio[1].pitch = 1;
			_Audio[1].PlayOneShot (jumpFail);

			if (streakCount < 5) { // X amount of chances before actually displying end game screen


				if (onStreak) {
					if (streakCount > 1) {
						_UI.UpdateScreen (streakCount + " Waves!");
					} else {
						_UI.UpdateScreen (streakCount + " Wave!");
					}
					score = 0;
					_Audio [1].PlayOneShot (lose);
					SpeechFromArray (sBoo, 1, cBoo);
				} else {

					_UI.UpdateScreen ("Try Again");
				}
				streakCount = 0;
			} 
			else {
				StartCoroutine( EndGame (score, streakCount) );
			}
			onStreak = false;

		}



		score = _UI.UpdateScore (score, nScore, streakCount);

			

	}

	private void SpeechFromArray(string[] speechArray, int type, Color color)
	{
		if (speechArray.Length > 0) {
			_UI.CreateSpeechBubble (speechArray [Random.Range (0, speechArray.Length)], type, color);
		} else {
			Debug.LogWarning (speechArray.ToString () + "Missing Values");
		}
	}

	private bool WithinThreshold(float pTime, float _value)
	{
		if (((_Player.animTime) > pTime - _value) && ((_Player.animTime) < pTime + _value)) {
			return true;
		} else {
			return false;
		}

	}
	IEnumerator CreateWave(int dir)
	{

		switch (dir) {
		case 0:
			
			// RIGHT ===========================
			for (int i = 0; i < _WIDTH; i++) {

				for (int j = 0; j < _HEIGHT; j++) {

					CheckIfPlayer (j, i, dir);
				}
				yield return new WaitForSeconds (waveDelay);
			}
			break;
			// ========================================

		case 1:

			// DOWN ======================================
			for (int j = 0; j < _HEIGHT; j++) {

				for (int i = 0; i < _WIDTH; i++) {

					CheckIfPlayer (j, i, dir);
				}
				yield return new WaitForSeconds (waveDelay);
			}
			break;
			// ==========================================

		case 2:
			
			// LEFT ===========================
			for (int i = _WIDTH-1; i >= 0; i--) {

				for (int j = 0; j < _HEIGHT; j++) {

					CheckIfPlayer (j, i, dir);
				}
				yield return new WaitForSeconds (waveDelay);
			}
			break;
			// ========================================
		
		case 3:

			// UP ======================================
			for (int j = _HEIGHT-1; j >=0; j--) {

				for (int i = 0; i < _WIDTH; i++) {

					CheckIfPlayer (j, i, dir);
				}
				yield return new WaitForSeconds (waveDelay);
			}
			break;
			// ==========================================

		default:
			break;
		}
			
	}
}
