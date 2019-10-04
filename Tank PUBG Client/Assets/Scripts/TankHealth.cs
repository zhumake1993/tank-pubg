using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
	public Slider mSlider;
	public Image mFillImage;
	public Color mFullHealthColor = Color.green;
	public Color mZeroHealthColor = Color.red;
	public ParticleSystem mExplosionParticles;
	public AudioSource mExplosionAudio;

	Camera mCamera;

	void Start()
    {
		mCamera = GetComponentInChildren<Camera>();
	}

	public void SetTankHealth(float health)
	{
		mSlider.value = health;
		mFillImage.color = Color.Lerp(mZeroHealthColor, mFullHealthColor, health);
	}

	public void Destroy()
	{
		if(mCamera)
			mCamera.transform.parent = null;

		mExplosionParticles.transform.parent = null;
		mExplosionParticles.Play();
		mExplosionAudio.Play();
		Destroy(mExplosionParticles.gameObject, mExplosionParticles.main.duration);
	}
}
