using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

	public GameObject _SpeechOBJ;
	public Canvas speechArea;

	public Text scoreText;
	public Text streakText;

	public Animator screen;
	public Text sc1;
	public Text sc2;

	public float UpdateScore(float score,float value, int streak, string desc)
	{
		float newVal = (value);
		float newScore = score + newVal;
		scoreText.text = ("SCORE " + newScore);
		streakText.text = ("STREAK - " + streak);

		if (newVal > 0) {
			UpdateScreen (desc + " + " + newVal.ToString ());
		} else {
			UpdateScreen (desc);
		}

		return newScore;
	}

	public void CreateSpeechBubble(string text, int type, Color color)
	{
		Vector3 pos = speechArea.transform.position + new Vector3 (Random.Range(-400,100), Random.Range(-500,50), 0);
		GameObject clone = Instantiate (_SpeechOBJ, pos, new Quaternion (0, 0, 0, 0), speechArea.transform);
		clone.GetComponent<SpeechBubble> ().Setup (text, type, color);

	}

	private void UpdateScreen(string text)
	{
		if (screen.GetBool ("screen2")) { //IF ON SCREEN 2

			sc1.text = text;
			screen.SetBool ("screen2", false);
		} else { // IF ON SCREEN 1
			sc2.text = text;
			screen.SetBool ("screen2", true);

		}
	}
}
