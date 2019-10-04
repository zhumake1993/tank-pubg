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
		mMsgHandle[Global.mCmd["SC_GAME_START"]] = Handle_SC_GAME_START;
		mMsgHandle[Global.mCmd["SC_SYN_INSTANTIATE"]] = Handle_SC_SYN_INSTANTIATE;
		mMsgHandle[Global.mCmd["SC_SYN_TRANSFORM"]] = Handle_SC_SYN_TRANSFORM;
		mMsgHandle[Global.mCmd["SC_SYN_DESTROY"]] = Handle_SC_SYN_DESTROY;
		mMsgHandle[Global.mCmd["SC_SYN_AUDIO_ENGINE"]] = Handle_SC_SYN_AUDIO_ENGINE;
		mMsgHandle[Global.mCmd["SC_SYN_TANK_HEALTH"]] = Handle_SC_SYN_TANK_HEALTH;
		mMsgHandle[Global.mCmd["SC_SYN_TANK_CHARGE"]] = Handle_SC_SYN_TANK_CHARGE;
		mMsgHandle[Global.mCmd["SC_SYN_TANK_CHARGE_SLIDER"]] = Handle_SC_SYN_TANK_CHARGE_SLIDER;
		mMsgHandle[Global.mCmd["SC_SYN_TANK_FIRE"]] = Handle_SC_SYN_TANK_FIRE;
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
		Debug.Log("clientID: " + clientID);
	}

	void Handle_SC_GAME_START(NetStream reader)
	{
		Global.mGameStatus = 1;
		Debug.Log("game start!");
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

	void Handle_SC_SYN_AUDIO_ENGINE(NetStream reader)
	{
		int entityID = reader.ReadInt32();
		bool engineDriving = reader.ReadInt32() == 1;

		mGame.PlayEngineAudio(entityID, engineDriving);
	}

	void Handle_SC_SYN_TANK_HEALTH(NetStream reader)
	{
		int entityID = reader.ReadInt32();
		float health = reader.ReadFloat();

		mGame.SetTankHealth(entityID, health);
	}

	void Handle_SC_SYN_TANK_CHARGE(NetStream reader)
	{
		int entityID = reader.ReadInt32();

		mGame.PlayChargingAudio(entityID);
	}

	void Handle_SC_SYN_TANK_CHARGE_SLIDER(NetStream reader)
	{
		int entityID = reader.ReadInt32();
		float val = reader.ReadFloat();

		mGame.SetAimSlider(entityID, val);
	}

	void Handle_SC_SYN_TANK_FIRE(NetStream reader)
	{
		int entityID = reader.ReadInt32();

		mGame.PlayFireAudio(entityID);
	}
}
