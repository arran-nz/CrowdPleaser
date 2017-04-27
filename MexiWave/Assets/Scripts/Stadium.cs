using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stadium : MonoBehaviour {

	private UserInterface _UI;

	[Header("Stadium Size")]
	public int _HEIGHT;
	public int _WIDTH;
	
	[Header("Player Position")]
	public int playerPos_X;
	public int playerPos_Y;

	[Header("Person Position")]
	public ColorPalette playerPalette;
	public GameObject[] _PersonOBJ;
	public Vector3 startPos;
	public Vector3 personSpacing;
	public float waveDelay = 0.1F;

	[Range(2,5)]
	public float waveFreq_MIN = 3F;
	[Range(5,10)]
	public float waveFreq_MAX = 8F;
	private float waveFreq = 5F;

	[Header("Scoring")]
	private float score;
	private bool onStreak;
	private bool lastChance;
	private int streakCount = 0;

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

	[Header("Sound")]
	public AudioClip win;
	public AudioClip lose;
	public AudioClip jump;
	public AudioClip jumpFail;
	private AudioSource _Audio;


	private float time;
	private bool activeWave;

	private Person[,] _PersonArray;
	private Person _Player;

	// Use this for initialization
	void Start () {
		
		AllocateSeating ();

		_Player.AllocatePlayer (playerPalette);
		_UI = GameObject.FindObjectOfType<UserInterface> ();
		_Audio = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {

		//INPUT
		if(Input.GetKeyDown(KeyCode.Space)||(Input.GetMouseButtonDown(0)))
		{
			_Player.Wave ();
			_Audio.PlayOneShot (jump);

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




		time += Time.deltaTime;
		if (time > waveFreq) {
			StartCoroutine (CreateWave(Random.Range(0,4))); // RIGHT
			waveFreq = Random.Range(waveFreq_MIN, waveFreq_MAX);
			time = 0;
		}
	}
	int randPerson;
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
		if ((Y == playerPos_Y) && (X == playerPos_X)) {

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
		float winThres = 0.15F; float goodThres = 0.10f; float greatThres = 0.05F; float perfectThres = 0.025F;
		string descString ="";
		float nScore = 0;

		yield return new WaitForSeconds (perfectTime);

		Debug.Log ((_Player.animTime) + "<- TIME / GOAL ->" + perfectTime);


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
			descString = "SCORE";

			_Audio.PlayOneShot (win);

			lastChance = false;
			if (onStreak) {

				if (_Audio.pitch < 2) {
					_Audio.pitch += 0.025F;
				}


				streakCount++;
			} else {
				onStreak = true;
				streakCount = 1;

			}
		}
 

		else {
			_Audio.pitch = 1;
			_Audio.PlayOneShot (jumpFail);

			if (onStreak) {
				descString = ("Streak Over");
				nScore = 100 * streakCount;

			} else {

				if (lastChance) {
					SpeechFromArray (sBoo, 1, cBoo);
					score = 0;
					_Audio.PlayOneShot (lose);
				} else {
					
					descString += ("Last Chance!");
					lastChance = true;
				}
			}

			onStreak = false; 
			streakCount = 0;

		}
			
		score = _UI.UpdateScore (score, nScore, streakCount, descString);
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
		activeWave = true;

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

		activeWave = false;
	}


	// NOT IN USE
	//CONVERT 1D to 2D ARRAY
	private static Person[,] Make2DArray(Person[] input, int height, int width)
	{
		Person[,] output = new Person[height, width];
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				output[i, j] = input[i * width + j];
			}
		}
		return output;
	}
}
