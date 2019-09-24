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

    // Update is called once per frame
    void Update()
    {
		//NetStream writer = new NetStream();
		//writer.WriteInt32(1);
		//writer.WriteFloat(v);
		//writer.WriteFloat(h);
		//AddMsg(writer.GetBuffer());
	}

	public void Move(float v, float h)
	{
		Vector3 movement = transform.forward * v * 10f * Time.deltaTime;
		mRigidbody.MovePosition(mRigidbody.position + movement);
	}
}
