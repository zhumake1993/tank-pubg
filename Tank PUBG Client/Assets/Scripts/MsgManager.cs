using System.Collections.Generic;
using UnityEngine;

public class MsgManager : MonoBehaviour
{
	Game mGame;

	delegate void MsgHandle(NetStream reader);

	Dictionary<int, MsgHandle> mMsgHandle = new Dictionary<int, MsgHandle>();

	void Start()
    {
		mGame = GameObject.FindWithTag("Game").GetComponent<Game>();

		mMsgHandle[Global.mCmd["SC_CONTROL_CONNECT_SUCCESS"]] = Handle_SC_CONTROL_CONNECT_SUCCESS;
		mMsgHandle[Global.mCmd["SC_GAME_START_SUCCESS"]] = Handle_SC_GAME_START_SUCCESS;
		mMsgHandle[Global.mCmd["SC_SYN_INSTANTIATE"]] = Handle_SC_SYN_INSTANTIATE;
		mMsgHandle[Global.mCmd["SC_SYN_TRANSFORM"]] = Handle_SC_SYN_TRANSFORM;
		mMsgHandle[Global.mCmd["SC_SYN_DESTROY"]] = Handle_SC_SYN_DESTROY;
	}

	public void Handle(byte[] msg)
	{
		NetStream reader = new NetStream(msg);
		int cmd = reader.ReadInt32();

		mMsgHandle[cmd](reader);
	}

	void Handle_SC_CONTROL_CONNECT_SUCCESS(NetStream reader)
	{
		int clientID = reader.ReadInt32();
		Global.mClientID = clientID;
	}

	void Handle_SC_GAME_START_SUCCESS(NetStream reader)
	{
		Global.mGameStatus = 1;
	}

	void Handle_SC_SYN_INSTANTIATE(NetStream reader)
	{
		int entityID = reader.ReadInt32();
		int clientID = reader.ReadInt32();
		string name = reader.ReadString();
		float px = reader.ReadFloat();
		float py = reader.ReadFloat();
		float pz = reader.ReadFloat();
		float rx = reader.ReadFloat();
		float ry = reader.ReadFloat();
		float rz = reader.ReadFloat();
		float rw = reader.ReadFloat();

		mGame.Instantiate(entityID, clientID, name, new Vector3(px, py, pz), new Quaternion(rx, ry, rz, rw));
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

	void Handle_SC_SYN_DESTROY(NetStream reader)
	{
		int entityID = reader.ReadInt32();

		mGame.Destroy(entityID);
	}
}
