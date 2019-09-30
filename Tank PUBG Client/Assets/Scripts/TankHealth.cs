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
	public GameObject mExplosionPrefab;

	private AudioSource mExplosionAudio;
	private ParticleSystem mExplosionParticles;

	void Start()
    {
		mExplosionParticles = Instantiate(mExplosionPrefab).GetComponent<ParticleSystem>();
		mExplosionAudio = mExplosionParticles.GetComponent<AudioSource>();
		mExplosionParticles.gameObject.SetActive(false);
	}

    void Update()
    {
        
    }
}
