using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
	public AudioSource mShootingAudio;
	public AudioClip mChargingClip;
	public AudioClip mFireClip;

	Slider mAimSlider;

	void Start()
    {
		mAimSlider = GameObject.FindWithTag("AimSlider").GetComponent<Slider>();
		mAimSlider.value = 0;
	}

	public void PlayChargingClip()
	{
		mShootingAudio.clip = mChargingClip;
		mShootingAudio.Play();
	}

	public void SetAimSlider(float val)
	{
		mAimSlider.value = val;
	}

	public void PlayFireClip()
	{
		mAimSlider.value = 0;
		mShootingAudio.clip = mFireClip;
		mShootingAudio.Play();
	}
}
