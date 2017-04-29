using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	//Game
	private Stadium _Game;

	[Header("Main Menu")]
	public Animator _A_Main;

	[Header("Settings")]
	public Animator _A_Settings;

	[Header("End Game")]
	public Animator _A_End;
	public Text T_Waves;
	public Text T_Score;

	[Header("Pause Menu")]
	public Animator _A_Pause;
	public Button _B_Pause;


	// Use this for initialization
	void Start () {
		_Game = GameObject.FindObjectOfType<Stadium> ();
		MainMenu (true);
		_B_Pause.gameObject.SetActive (false);
	}

	public void PlayGame()
	{
		MainMenu (false);
		_Game.StartGame ();
		_B_Pause.gameObject.SetActive (true);
	}

	public void MuteAudio()
	{
		AudioSource[] sounds = GameObject.FindObjectsOfType<AudioSource> ();
		for (int i = 0; i < sounds.Length; i++) {
			sounds [i].mute = !sounds [i].mute;
		}
	}


	// SET UP END GAME MENU ============================
	public void SetupEndGame(float score, int waves)
	{
		_B_Pause.gameObject.SetActive (false);
		Animate (_A_End, true);
		T_Score.text = score.ToString ();;
		T_Waves.text = (waves.ToString() + "\nWaves!");
	}



	// ANIMATIONS ==========================
	public void MainMenu(bool open)
	{
		Animate (_A_Settings, false);
		Animate (_A_End, false);

		Animate (_A_Main, open);
	}
	public void SettingsMenu(bool open)
	{
		Animate (_A_Main, !open);

		Animate (_A_Settings, open);
	}
	public void EndGameMenu(bool open)
	{
		Animate (_A_End, open);
	}
	public void PauseMenu(bool open)
	{
		_B_Pause.gameObject.SetActive(!open);
		_Game.PauseGame (open);
		Animate (_A_Pause, open);
	}


	void Animate(Animator controller, bool open)
	{
		if (open) {
			controller.Play ("open");
			controller.SetBool ("open", true);
		}else if (!open) {

			if (controller.GetBool ("open")) {
				controller.Play ("close");
				controller.SetBool ("open", false);
			}
		}
	}
}
