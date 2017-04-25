using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

	private Animator arms;

	private bool isWaving;
	// Use this for initialization
	void Start () {
		arms = GetComponentInChildren<Animator> ();
	}

	public void Wave()
	{
		arms.Play ("wave");
	}
	public float Check(bool isPlayer)
	{
		if (!isPlayer) {
			Debug.Log ("Beside Player: " + AnimatorTime ());
		} else {
			Debug.Log ("Player: " + AnimatorTime ());
		}

		return AnimatorTime ();
	}
	private float AnimatorTime()
	{
		AnimatorStateInfo animationState = arms.GetCurrentAnimatorStateInfo(0);
		AnimatorClipInfo[] myAnimatorClip = arms.GetCurrentAnimatorClipInfo(0);
		if (animationState.IsName ("wave")) {
			return myAnimatorClip [0].clip.length * animationState.normalizedTime;
		} else {
			return 0;
		}
	}
}
