using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UserInterface : MonoBehaviour, IPointerDownHandler {

	public GameObject _SpeechOBJ;
	public GameObject _TouchEffect;
	public Canvas speechArea;

	public Text scoreText;
	public Text streakText;
	public Text newValueText;

	private Animator newScore_A;
	public Animator screen_A;
	public Text sc1;
	public Text sc2;

	private Stadium game;
	public string defaultScreen = "Crowd Pleaser";
	void Start()
	{
		game = GameObject.FindObjectOfType<Stadium> ();
		newScore_A = newValueText.GetComponent<Animator> ();
	}

	public float UpdateScore(float score,float value, int streak)
	{
		float newVal = (value);
		float newScore = score + newVal;
		scoreText.text = (newScore.ToString());
		streakText.text = (streak.ToString());

		if (newVal > 0) {
			newValueText.text = newVal.ToString ();
			newScore_A.Play ("score");
		}

		return newScore;
	}

	public void CreateSpeechBubble(string text, int type, Color color)
	{
		Vector3 pos = speechArea.transform.position + new Vector3 (Random.Range(-400,100), Random.Range(-300,150), 0);
		GameObject clone = Instantiate (_SpeechOBJ, pos, new Quaternion (0, 0, 0, 0), speechArea.transform);
		clone.GetComponent<SpeechBubble> ().Setup (text, type, color);

	}

	public void UpdateScreen(string text)
	{
		if (screen_A.GetBool ("screen2")) { //IF ON SCREEN 2

			sc1.text = text;
			screen_A.SetBool ("screen2", false);
		} else { // IF ON SCREEN 1
			sc2.text = text;
			screen_A.SetBool ("screen2", true);

		}
	}

	public void ReturnScreen()
	{
		UpdateScreen (defaultScreen);
	}
	public IEnumerator ReturnScreenDelay()
	{
		yield return new WaitForSeconds (3F);
		ReturnScreen ();
	}

	public void OnPointerDown(PointerEventData ped)
	{
		game.PlayerJump ();
		GameObject clone = Instantiate (_TouchEffect, ped.position, new Quaternion (0, 0, 0, 0), this.transform);
		Destroy (clone, 0.7F);
	}

}
