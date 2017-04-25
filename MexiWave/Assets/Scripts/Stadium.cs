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


	private Person[,] _PersonArray;

	// Use this for initialization
	void Start () {
		
		AllocateSeating ();
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space))
		{
			_PersonArray [playerPos_Y, playerPos_X].Wave ();
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			StartCoroutine (CreateWave(0)); // RIGHT
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			StartCoroutine (CreateWave(2)); // LEFT
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			StartCoroutine (CreateWave(3)); // UP
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			StartCoroutine (CreateWave(1)); // DOWN
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


			switch (dir) {

			case 0: 
				// CHECK LEFT SEAT
				_PersonArray[playerPos_Y, playerPos_X-1].Check(false);
				// CHECK PLAYER
				_PersonArray[playerPos_Y,playerPos_X].Check(true);
				break;

			case 1:
				// CHECK TOP SEAT
				_PersonArray[playerPos_Y-1, playerPos_X].Check(false);
				// CHECK PLAYER
				_PersonArray[playerPos_Y,playerPos_X].Check(true);
				break;

			case 2:
				// CHECK RIGHT SEAT
				_PersonArray[playerPos_Y, playerPos_X+1].Check(false);
				// CHECK PLAYER
				_PersonArray[playerPos_Y,playerPos_X].Check(true);
				break;

			case 3:
				// CHECK BOTTOM SEAT
				_PersonArray[playerPos_Y+1, playerPos_X].Check(false);
				// CHECK PLAYER
				_PersonArray[playerPos_Y,playerPos_X].Check(true);
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
