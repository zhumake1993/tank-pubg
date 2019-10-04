using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MsgManager : MonoBehaviour
{
	Game mGame;

	delegate void MsgHandle(NetStream reader, EndPoint client);
	Dictionary<int, MsgHandle> mMsgHandle = new Dictionary<int, MsgHandle>();

	void Start()
	{
		mGame = GameObject.FindWithTag("Game").GetComponent<Game>();

		mMsgHandle[Global.mCmd["CS_CONTROL_CONNECT"]] = Handle_CS_CONTROL_CONNECT;
		mMsgHandle[Global.mCmd["CS_GAME_START"]] = Handle_CS_GAME_START;
		mMsgHandle[Global.mCmd["CS_INPUT"]] = Handle_CS_INPUT;
	}

	public void Handle(Msg msg)
	{
		NetStream reader = new NetStream(msg.mMsg);
		int cmd = reader.ReadInt32();
		mMsgHandle[cmd](reader, msg.mClient);
	}

	void Handle_CS_CONTROL_CONNECT(NetStream reader, EndPoint client)
	{
		mGame.AddNewClient(client);
	}

	void Handle_CS_GAME_START(NetStream reader, EndPoint client)
	{
		mGame.StartGame();
	}

	void Handle_CS_INPUT(NetStream reader, EndPoint client)
	{
		int clientID = Global.mClients[client];
		float v = reader.ReadFloat();
		float h = reader.ReadFloat();
		float a = reader.ReadFloat();
		int j = reader.ReadInt32();

		mGame.PlayerControl(clientID, v, h, a, j);
	}
}
