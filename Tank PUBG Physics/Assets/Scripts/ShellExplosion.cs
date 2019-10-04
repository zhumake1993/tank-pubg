using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
	public LayerMask mTankMask;
	public float mMaxDamage = 100f;
	public float mExplosionForce = 1000f;
	public float mMaxLifeTime = 2f;
	public float mExplosionRadius = 5f;

	void Start()
    {
		Destroy(gameObject, mMaxLifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, mExplosionRadius, mTankMask);

		for (int i = 0; i < colliders.Length; i++)
		{
			Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

			if (!targetRigidbody)
				continue;

			targetRigidbody.AddExplosionForce(mExplosionForce, transform.position, mExplosionRadius);

			TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

			if (!targetHealth)
				continue;

			float damage = CalculateDamage(targetRigidbody.position);

			targetHealth.TakeDamage(damage);
		}

		Destroy(gameObject);
	}


	float CalculateDamage(Vector3 targetPosition)
	{
		Vector3 explosionToTarget = targetPosition - transform.position;

		float explosionDistance = explosionToTarget.magnitude;

		float relativeDistance = (mExplosionRadius - explosionDistance) / mExplosionRadius;

		float damage = relativeDistance * mMaxDamage;

		damage = Mathf.Max(0f, damage);

		return damage;
	}
}
