using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
	public AudioSource mMovementAudio;
	public AudioClip mEngineIdling;
	public AudioClip mEngineDriving;

	public void PlayEngineAudio(bool engineDriving)
	{
		if (engineDriving)
		{
			mMovementAudio.clip = mEngineDriving;
			mMovementAudio.Play();
		}
		else
		{
			mMovementAudio.clip = mEngineIdling;
			mMovementAudio.Play();
		}
	}
}
