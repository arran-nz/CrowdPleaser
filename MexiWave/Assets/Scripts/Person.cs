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
		arms.Play ("LiftArms");
	}

	public void AllocatePlayer()
	{
		GetComponentInChildren<SpriteRenderer> ().color = Color.grey;
	}
		
	public float[] AnimatorInfo()
	{
		float[] _animArray = new float[2];

		AnimatorStateInfo animationState = arms.GetCurrentAnimatorStateInfo(0);
		AnimatorClipInfo[] myAnimatorClip = arms.GetCurrentAnimatorClipInfo(0);
		_animArray[1] =  (myAnimatorClip [0].clip.length * animationState.normalizedTime) * 100;

		if (animationState.IsName ("Idle")) {
			_animArray [0] = 1;
		}
		if (animationState.IsName ("LiftArms")) {
			_animArray [0] = 2;
		}
		if (animationState.IsName ("Up")) {
			_animArray [0] = 3;
		}
		if (animationState.IsName ("DropArms")) {
			_animArray [0] = 4;
		}


		return _animArray;
	}
}
