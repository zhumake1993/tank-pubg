using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syn : MonoBehaviour
{
	NetManager mNetManager;
	Attribute mAttribute;

	Vector3 mOldPosition;
	Quaternion mOldRotation;

	void Start()
    {
		mNetManager = GameObject.FindWithTag("Manager").GetComponent<NetManager>();
		mAttribute = GetComponent<Attribute>();

		mOldPosition = new Vector3(0.0f, 0.0f, 0.0f);
		mOldRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

		SynInstantiate();
	}

    void Update()
    {
		SynTransform();
	}

	void OnDestroy()
	{
		SynDestroy();
	}

	void SynInstantiate()
	{
		NetStream writer = new NetStream();
		writer.WriteInt32(Global.mCmd["PS_SYN_INSTANTIATE"]);
		writer.WriteInt32(mAttribute.GetEntityID());
		writer.WriteInt32(mAttribute.GetClientID());
		writer.WriteString(mAttribute.GetName());
		writer.WriteFloat(transform.position.x);
		writer.WriteFloat(transform.position.y);
		writer.WriteFloat(transform.position.z);
		writer.WriteFloat(transform.rotation.x);
		writer.WriteFloat(transform.rotation.y);
		writer.WriteFloat(transform.rotation.z);
		writer.WriteFloat(transform.rotation.w);
		mNetManager.AddMsg(writer.GetBuffer());
	}

	void SynTransform()
	{
		if (transform.position != mOldPosition || transform.rotation != mOldRotation)
		{
			mOldPosition = transform.position;
			mOldRotation = transform.rotation;

			NetStream writer = new NetStream();
			writer.WriteInt32(Global.mCmd["PS_SYN_TRANSFORM"]);
			writer.WriteInt32(mAttribute.GetEntityID());
			writer.WriteFloat(transform.position.x);
			writer.WriteFloat(transform.position.y);
			writer.WriteFloat(transform.position.z);
			writer.WriteFloat(transform.rotation.x);
			writer.WriteFloat(transform.rotation.y);
			writer.WriteFloat(transform.rotation.z);
			writer.WriteFloat(transform.rotation.w);
			mNetManager.AddMsg(writer.GetBuffer());
		}
	}

	void SynDestroy()
	{
		NetStream writer = new NetStream();
		writer.WriteInt32(Global.mCmd["PS_SYN_DESTROY"]);
		writer.WriteInt32(mAttribute.GetEntityID());
		mNetManager.AddMsg(writer.GetBuffer());
	}
}
