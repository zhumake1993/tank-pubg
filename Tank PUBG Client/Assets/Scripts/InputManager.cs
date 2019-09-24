using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public NetManager mNetManager;

    void Update()
    {
		NetStream writer = new NetStream();

		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

		writer.WriteInt32(1);
		writer.WriteFloat(v);
		writer.WriteFloat(h);

		mNetManager.AddMsg(writer.GetBuffer());
	}
}
