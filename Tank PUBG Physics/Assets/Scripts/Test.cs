using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	NetManager mNetManager;
	Rigidbody mRigidbody;

	void Awake()
    {
		mNetManager = GameObject.FindWithTag("Manager").GetComponent<NetManager>();

		mRigidbody = GetComponent<Rigidbody>();
	}

    void Update()
    {
		NetStream writer = new NetStream();
		writer.WriteInt32(Global.mCmd["PS_SYN_TRANSFORM"]);
		writer.WriteInt32(1);
		writer.WriteFloat(transform.position.x);
		writer.WriteFloat(transform.position.y);
		writer.WriteFloat(transform.position.z);
		writer.WriteFloat(transform.rotation.x);
		writer.WriteFloat(transform.rotation.y);
		writer.WriteFloat(transform.rotation.z);
		writer.WriteFloat(transform.rotation.w);
		mNetManager.AddMsg(writer.GetBuffer());
	}

	public void Move(float v, float h)
	{
		Vector3 movement = transform.forward * v * 10f * Time.deltaTime;
		mRigidbody.MovePosition(mRigidbody.position + movement);
	}
}
