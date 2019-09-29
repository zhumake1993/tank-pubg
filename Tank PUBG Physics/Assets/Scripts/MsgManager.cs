using System.Collections.Generic;
using UnityEngine;

public class MsgManager : MonoBehaviour
{
	delegate void MsgHandle(NetStream reader);

	Game mGame;
	NetManager mNetManager;

	Dictionary<int, MsgHandle> mMsgHandle = new Dictionary<int, MsgHandle>();

	void Start()
	{
		mNetManager = GameObject.FindWithTag("Manager").GetComponent<NetManager>();
		mGame = GameObject.FindWithTag("Game").GetComponent<Game>();

		mMsgHandle[Global.mCmd["SP_CONTROL_CONNECT"]] = Handle_SP_CONTROL_CONNECT;
		mMsgHandle[Global.mCmd["SP_CONTROL_NEW_CLIENT"]] = Handle_SP_CONTROL_NEW_CLIENT;
		mMsgHandle[Global.mCmd["SP_GAME_START"]] = Handle_SP_GAME_START;
		mMsgHandle[Global.mCmd["SP_INPUT"]] = Handle_SP_INPUT;
	}

	public void Handle(byte[] msg)
	{
		NetStream reader = new NetStream(msg);
		int cmd = reader.ReadInt32();
		mMsgHandle[cmd](reader);
	}

	void Handle_SP_CONTROL_CONNECT(NetStream reader)
	{
		NetStream writer = new NetStream();
		writer.WriteInt32(Global.mCmd["PS_CONTROL_CONNECT_SUCCESS"]);
		mNetManager.AddMsg(writer.GetBuffer());
	}

	void Handle_SP_CONTROL_NEW_CLIENT(NetStream reader)
	{
		int clientID = reader.ReadInt32();
		mGame.AddNewClient(clientID);
	}

	void Handle_SP_GAME_START(NetStream reader)
	{
		mGame.StartGame();
	}

	void Handle_SP_INPUT(NetStream reader)
	{
		int playerID = reader.ReadInt32();
		float v = reader.ReadFloat();
		float h = reader.ReadFloat();
		float a = reader.ReadFloat();
		bool j = reader.ReadInt32() != 0;

		mGame.PlayerControl(playerID, v, h, a, j);
	}
}
