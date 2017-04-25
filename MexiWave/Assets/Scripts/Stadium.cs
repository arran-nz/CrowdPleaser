using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stadium : MonoBehaviour {

	[Header("Stadium Size")]
	public int _HEIGHT;
	public int _WIDTH;
	
	[Header("Player Position")]
	public int playerPos_X;
	public int playerPos_Y;

	[Header("Person Position")]
	public GameObject _PersonOBJ;
	public Vector3 startPos;
	public Vector3 personSpacing;
	public float waveDelay = 0.1F;

	[Range(2,5)]
	public float waveFreq_MIN = 3F;
	[Range(5,10)]
	public float waveFreq_MAX = 8F;

	private float waveFreq = 5F;

	private float time;
	private Person[,] _PersonArray;

	// Use this for initialization
	void Start () {
		
		AllocateSeating ();
		_PersonArray [playerPos_Y, playerPos_X].AllocatePlayer ();
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space))
		{
			_PersonArray [playerPos_Y, playerPos_X].Wave ();
		}

		time += Time.deltaTime;

		if (time > waveFreq) {
			StartCoroutine (CreateWave(Random.Range(0,4))); // RIGHT
			waveFreq = Random.Range(waveFreq_MIN, waveFreq_MAX);
			time = 0;
		}
	}
	void AllocateSeating()
	{
		_PersonArray = new Person[_HEIGHT, _WIDTH];

		Quaternion rot = new Quaternion (0, 0, 0, 0);
		Vector3 pos = startPos * this.transform.localScale.x;
		for (int j = 0; j < _HEIGHT; j++) {

			for (int i = 0; i < _WIDTH; i++) {

				GameObject clone = Instantiate (_PersonOBJ, pos, rot, this.gameObject.transform);
				_PersonArray [j, i] =	clone.GetComponent<Person> ();

				pos += new Vector3 (personSpacing.x * this.transform.localScale.x, 0);

			}	

			pos = new Vector3 (startPos.x * this.transform.localScale.x , pos.y - personSpacing.y * this.transform.localScale.y);
		}
			
	}

	void CheckIfPlayer (int Y, int X, int dir)
	{
		// PLAYER
		if ((Y == playerPos_Y) && (X == playerPos_X)) {
			
			float[] playerInfo = _PersonArray[playerPos_Y, playerPos_X].AnimatorInfo();

			switch (dir) {

			case 0: 
				// CHECK LEFT SEAT
				GenerateScore (playerInfo, Y, X - 1);
				break;

			case 1:
				// CHECK TOP SEAT
				GenerateScore(playerInfo, Y-1, X);
				break;

			case 2:
				// CHECK RIGHT SEAT
				GenerateScore(playerInfo,Y, X + 1);
				break;

			case 3:
				// CHECK BOTTOM SEAT
				GenerateScore(playerInfo, Y+1, X);
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
	void GenerateScore(float[] playerInfo, int Y, int X)
	{
		// CHECK LEFT SEAT
		float[] seat = _PersonArray [Y, X].AnimatorInfo ();
		string debugString;

		if (playerInfo [0] == 2) {
			debugString = "GOT IT -";
			//Debug.Log (seat [1].ToString ());
			if (playerInfo [1] == seat [1]) {
				debugString += " Perfect!";
			} else if (playerInfo [1] < seat [1]) {
				debugString += " Little Late..";
			} else if (playerInfo [1] > seat [1]) {
				debugString += " Little Early..";
			}
		} else {
			debugString = "FAIL -";
			if (playerInfo [0] == 3) {
				debugString += " Close, But Early";
			} else if (playerInfo [0] == 4) {
				debugString += " Far too Early!";
			} else {
				debugString += " Are you paying attention?";
			}
		}

		Debug.Log (debugString);
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
