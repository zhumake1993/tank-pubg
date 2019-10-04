using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{
	public GameObject mShell;
	public Transform mFireTransform;
	public float mMinLaunchForce = 15f;
	public float mMaxLaunchForce = 30f;
	public float mMaxChargeTime = 0.75f;

	Game mGame;
	NetManager mNetManager;
	Attribute mAttribute;
	float mCurrentLaunchForce;
	float mChargeSpeed;
	bool mFired;
	int mFirePress = 0;

	void Start()
    {
		mGame = GameObject.FindWithTag("Game").GetComponent<Game>();
		mNetManager = GameObject.FindWithTag("Manager").GetComponent<NetManager>();
		mAttribute = GetComponent<Attribute>();
		mCurrentLaunchForce = mMinLaunchForce;
		mChargeSpeed = (mMaxLaunchForce - mMinLaunchForce) / mMaxChargeTime;
	}

    void Update()
    {
		if (mCurrentLaunchForce >= mMaxLaunchForce && !mFired)
		{
			mCurrentLaunchForce = mMaxLaunchForce;
			Fire();
		}
		else if (mFirePress == 1)
		{
			mFired = false;
			mCurrentLaunchForce = mMinLaunchForce;

			NetStream writer = new NetStream();
			writer.WriteInt32(Global.mCmd["SC_SYN_TANK_CHARGE"]);
			writer.WriteInt32(mAttribute.GetEntityID());
			mNetManager.AddMsg(new Msg(writer.GetBuffer(), Global.mClientsR[mAttribute.GetClientID()]));
		}
		else if (mFirePress == 2 && !mFired)
		{
			mCurrentLaunchForce += mChargeSpeed * Time.deltaTime;

			NetStream writer = new NetStream();
			writer.WriteInt32(Global.mCmd["SC_SYN_TANK_CHARGE_SLIDER"]);
			writer.WriteInt32(mAttribute.GetEntityID());
			writer.WriteFloat((mCurrentLaunchForce - mMinLaunchForce) / (mMaxLaunchForce - mMinLaunchForce));
			mNetManager.AddMsg(new Msg(writer.GetBuffer(), Global.mClientsR[mAttribute.GetClientID()]));
		}
		else if (mFirePress == 3 && !mFired)
		{
			Fire();
		}
	}

	void Fire()
	{
		mFired = true;

		GameObject shell = Instantiate(mShell, mFireTransform.position, mFireTransform.rotation) as GameObject;

		Attribute attribute = shell.GetComponent<Attribute>();
		attribute.SetEntityID(mGame.GetAvailableEntityID());
		attribute.SetClientID(0);
		attribute.SetName("Shell");

		mGame.mGameObjects[attribute.GetEntityID()] = shell;

		shell.GetComponent<Rigidbody>().velocity = mCurrentLaunchForce * mFireTransform.forward;

		NetStream writer = new NetStream();
		writer.WriteInt32(Global.mCmd["SC_SYN_TANK_FIRE"]);
		writer.WriteInt32(mAttribute.GetEntityID());
		mNetManager.AddMsg(new Msg(writer.GetBuffer()));

		mCurrentLaunchForce = mMinLaunchForce;
	}

	public void SetFireInput(int firePress)
	{
		mFirePress = firePress;
	}
}
