using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

	private bool isWaving;

	private SpriteRenderer spriteRenderer;
	private Animator arms;
	public bool enablePalette;
	public ColorPalette[] palettes;

	private Texture2D texture;
	private MaterialPropertyBlock block;

	void Awake(){
		
		if (enablePalette) {
			spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		}
		arms = GetComponentInChildren<Animator> ();
	}
	void Start()
	{
		if (palettes.Length > 0) {
			SwapColors (palettes[Random.Range(0,palettes.Length)]);
		}
	}

	void LateUpdate(){
		
		if (spriteRenderer != null) {
			spriteRenderer.SetPropertyBlock (block);
		}
	}

	public void Wave()
	{
		arms.Play ("LiftArms");
	}

	public void AllocatePlayer()
	{
		Wave ();
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


	void SwapColors(ColorPalette palette){

		if (palette.cachedTexture == null) {

			//Copying Texture
			texture = spriteRenderer.sprite.texture;

			var w = texture.width;
			var h = texture.height;

			var cloneTexture = new Texture2D(w, h);
			cloneTexture.wrapMode = TextureWrapMode.Clamp;
			cloneTexture.filterMode = FilterMode.Point;

			var colors = texture.GetPixels();

			for(int i = 0; i < colors.Length; i++){
				colors[i] = palette.GetColor(colors[i]);
			}

			cloneTexture.SetPixels(colors);
			cloneTexture.Apply();

			palette.cachedTexture = cloneTexture;
		}



		block = new MaterialPropertyBlock();
		block.SetTexture("_MainTex", palette.cachedTexture);
	}
}
