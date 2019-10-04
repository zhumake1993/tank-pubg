using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
	public ParticleSystem mExplosionParticles;
	public AudioSource mExplosionAudio;

	public void PlayAudio()
	{
		mExplosionParticles.transform.parent = null;
		mExplosionParticles.Play();
		mExplosionAudio.Play();
		Destroy(mExplosionParticles.gameObject, mExplosionParticles.main.duration);
	}
}
