using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public NetManager mNetManager;

    void Update()
    {
		if (Input.GetKeyDown("z"))
		{
			NetStream writer = new NetStream();
			writer.WriteInt32(Global.mCmd["CS_CONTROL_CONNECT"]);
			mNetManager.AddMsg(writer.GetBuffer());
		}

		if (Input.GetKeyDown("x"))
		{
			NetStream writer = new NetStream();
			writer.WriteInt32(Global.mCmd["CS_GAME_START"]);
			mNetManager.AddMsg(writer.GetBuffer());
		}

		//if (Global.mGameStatus == 1)
		//{
		//	NetStream writer = new NetStream();

		//	float v = Input.GetAxis("Vertical");
		//	float h = Input.GetAxis("Horizontal");

		//	writer.WriteInt32(1);
		//	writer.WriteFloat(v);
		//	writer.WriteFloat(h);

		//	mNetManager.AddMsg(writer.GetBuffer());
		//}
	}
}
