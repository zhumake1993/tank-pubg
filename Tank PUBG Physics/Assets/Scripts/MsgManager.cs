using System.Collections.Generic;
using UnityEngine;

public class MsgManager : MonoBehaviour
{
	public NetManager mNetManager;
	public Game mGame;

	delegate void MsgHandle(NetStream reader);

	Dictionary<int, MsgHandle> mMsgHandle = new Dictionary<int, MsgHandle>();

	void Start()
	{
		mMsgHandle[Global.mCmd["SP_CONTROL_CONNECT"]] = Handle_SP_CONTROL_CONNECT;
		mMsgHandle[Global.mCmd["SP_GAME_START"]] = Handle_SP_GAME_START;
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

	void Handle_SP_GAME_START(NetStream reader)
	{
		mGame.StartGame();
	}
}
