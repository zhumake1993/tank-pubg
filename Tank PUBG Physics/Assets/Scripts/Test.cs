using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	Rigidbody mRigidbody;

	void Awake()
    {
		mRigidbody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		//Destroy(gameObject, 1.0f);
	}

	void Update()
    {
		
	}

	public void Move(float v, float h)
	{
		Vector3 movement = transform.forward * v * 10f * Time.deltaTime;
		mRigidbody.MovePosition(mRigidbody.position + movement);

		float turn = h * 50f * Time.deltaTime;
		Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
		mRigidbody.MoveRotation(mRigidbody.rotation * turnRotation);
	}
}
