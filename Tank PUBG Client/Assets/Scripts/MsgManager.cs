using System.Collections.Generic;
using UnityEngine;

public class MsgManager : MonoBehaviour
{
	public Game mGame;

	delegate void MsgHandle(NetStream reader);

	Dictionary<int, MsgHandle> mMsgHandle = new Dictionary<int, MsgHandle>();

	void Awake()
    {
		mMsgHandle[Global.mCmd["SC_CONTROL_CONNECT_SUCCESS"]] = Handle_SC_CONTROL_CONNECT_SUCCESS;
		mMsgHandle[Global.mCmd["SC_INSTANTIATE"]] = Handle_SC_INSTANTIATE;
		mMsgHandle[Global.mCmd["SC_SYN_TRANSFORM"]] = Handle_SC_SYN_TRANSFORM;
	}

	public void Handle(byte[] msg)
	{
		NetStream reader = new NetStream(msg);
		int cmd = reader.ReadInt32();

		mMsgHandle[cmd](reader);
	}

	void Handle_SC_CONTROL_CONNECT_SUCCESS(NetStream reader)
	{
		Debug.Log("Connect Success!");

		//float v = reader.ReadFloat();
		//float h = reader.ReadFloat();
		//Debug.Log(v.ToString() + " " + h.ToString());
	}

	void Handle_SC_INSTANTIATE(NetStream reader)
	{
		int entityID = reader.ReadInt32();
		string name = reader.ReadString();
		float px = reader.ReadFloat();
		float py = reader.ReadFloat();
		float pz = reader.ReadFloat();
		float rx = reader.ReadFloat();
		float ry = reader.ReadFloat();
		float rz = reader.ReadFloat();
		float rw = reader.ReadFloat();

		mGame.Instantiate(entityID, name, new Vector3(px, py, pz), new Quaternion(rx, ry, rz, rw));
	}

	void Handle_SC_SYN_TRANSFORM(NetStream reader)
	{
		int entityID = reader.ReadInt32();
		float px = reader.ReadFloat();
		float py = reader.ReadFloat();
		float pz = reader.ReadFloat();
		float rx = reader.ReadFloat();
		float ry = reader.ReadFloat();
		float rz = reader.ReadFloat();
		float rw = reader.ReadFloat();

		mGame.SetTransform(entityID, new Vector3(px, py, pz), new Quaternion(rx, ry, rz, rw));
	}
}
