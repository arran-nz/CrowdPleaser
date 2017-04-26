using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

	public Text scoreText;
	public Text streakText;

	public Animator screen;
	public Text sc1;
	public Text sc2;

	public float UpdateScore(float score,float value, int streak, string desc)
	{
		float newVal = (value * streak);
		float newScore = score + newVal;
		scoreText.text = ("SCORE " + score);
		streakText.text = ("STREAK - " + streak);

		if (newVal > 0) {
			UpdateScreen (desc + " + " + newVal.ToString ());
		} else {
			UpdateScreen (desc);
		}

		return newScore;
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
