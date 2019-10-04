using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
	public float mSpeed = 12f;
	public float mTurnSpeed = 180f;

	NetManager mNetManager;
	Attribute mAttribute;
	Rigidbody mRigidbody;
	float mVerticalInputValue;
	float mHorizontalInputValue;
	bool mEngineDriving = false;

	void Awake()
	{
		mNetManager = GameObject.FindWithTag("Manager").GetComponent<NetManager>();
		mAttribute = GetComponent<Attribute>();
		mRigidbody = GetComponent<Rigidbody>();
	}

	void Start()
	{

	}

	void Update()
	{
		if (Mathf.Abs(mVerticalInputValue) < 0.1f && Mathf.Abs(mHorizontalInputValue) < 0.1f)
		{
			if (mEngineDriving)
			{
				NetStream writer = new NetStream();
				writer.WriteInt32(Global.mCmd["SC_SYN_AUDIO_ENGINE"]);
				writer.WriteInt32(mAttribute.GetEntityID());
				writer.WriteInt32(0);
				mNetManager.AddMsg(new Msg(writer.GetBuffer()));

				mEngineDriving = false;
			}
		}
		else
		{
			if (!mEngineDriving)
			{
				NetStream writer = new NetStream();
				writer.WriteInt32(Global.mCmd["SC_SYN_AUDIO_ENGINE"]);
				writer.WriteInt32(mAttribute.GetEntityID());
				writer.WriteInt32(1);
				mNetManager.AddMsg(new Msg(writer.GetBuffer()));

				mEngineDriving = true;
			}
		}
	}

	void FixedUpdate()
	{
		Vector3 movement = transform.forward * mVerticalInputValue * mSpeed * Time.deltaTime;
		mRigidbody.MovePosition(mRigidbody.position + movement);

		float turn = mHorizontalInputValue * mTurnSpeed * Time.deltaTime;
		Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
		mRigidbody.MoveRotation(mRigidbody.rotation * turnRotation);
	}

	public void SetMovementInput(float v, float h)
	{
		mVerticalInputValue = v;
		mHorizontalInputValue = h;
	}
}
