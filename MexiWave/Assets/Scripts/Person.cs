using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {



	private SpriteRenderer spriteRenderer;
	private Animator arms;
	public bool enablePalette;
	public ColorPalette[] palettes;

	private Texture2D texture;
	private MaterialPropertyBlock block;

	public float animTime;

	void Awake(){
		
		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		arms = GetComponentInChildren<Animator> ();
	}
	void LateUpdate(){

		AnimatorInfo ();
		if (spriteRenderer != null && enablePalette) {
			spriteRenderer.SetPropertyBlock (block);
		}
	}



	public void Wave()
	{
		arms.Play ("wave");
	}

	public void AllocatePlayer(ColorPalette palette)
	{
		SwapColors (palette);
	}
	public void AllocateSupporter()
	{
		if (palettes.Length > 0 && enablePalette) {
			SwapColors (palettes[Random.Range(0,palettes.Length)]);
		}
	}

	private void AnimatorInfo()
	{
		AnimatorStateInfo animationState = arms.GetCurrentAnimatorStateInfo(0);
	
		animTime =  animationState.normalizedTime;
	}


	private void SwapColors(ColorPalette palette){

		if ((palette.cachedTexture == null)) {

			//Copying Texture
			texture = spriteRenderer.sprite.texture;

			int w = texture.width;
			int h = texture.height;

			Texture2D cloneTexture = new Texture2D(w, h);
			cloneTexture.wrapMode = TextureWrapMode.Clamp;
			cloneTexture.filterMode = FilterMode.Point;

			Color[] colors = texture.GetPixels();

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
