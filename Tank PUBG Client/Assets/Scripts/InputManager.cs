using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	NetManager mNetManager;

	void Start()
	{
		mNetManager = GameObject.FindWithTag("Manager").GetComponent<NetManager>();
	}

    void Update()
    {
		if(Global.mGameStatus == 0)
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
		}else if(Global.mGameStatus == 1)
		{
			float v = Input.GetAxis("Vertical");
			float h = Input.GetAxis("Horizontal");

			if(v!=0 || h != 0)
			{
				NetStream writer = new NetStream();
				writer.WriteInt32(Global.mCmd["CS_INPUT"]);
				writer.WriteFloat(v);
				writer.WriteFloat(h);
				writer.WriteFloat(3.14f);
				writer.WriteInt32(0);
				mNetManager.AddMsg(writer.GetBuffer());
			}
		}
	}
}
