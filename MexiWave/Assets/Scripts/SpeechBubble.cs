using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

	public Sprite[] type;

	private Animator _Anim;
	private Image _Image;
	private Text _Text;
	// Use this for initialization
	void Awake () {
		
		_Anim = GetComponentInChildren<Animator> ();
		_Image = GetComponentInChildren<Image> ();
		_Text = GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	public void Setup (string speech, int typeValue, Color color) {

		_Image.color = color;
		_Image.sprite = type [typeValue];
		_Text.text = speech;
		_Anim.Play ("open");

		StartCoroutine (Destroy ());
	}

	IEnumerator Destroy()
	{
		yield return new WaitForSeconds (3f);
		_Anim.Play ("close");
		Destroy(this.gameObject, 0.5F);
	}


}
