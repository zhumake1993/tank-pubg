using System.Collections;
using System.Net;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public GameObject mTank;
	public Transform[] mTankSpawnPoints;

	NetManager mNetManager;

	[HideInInspector] public Dictionary<int, GameObject> mGameObjects = new Dictionary<int, GameObject>();
	[HideInInspector] public Dictionary<int, GameObject> mPlayerGameObjects = new Dictionary<int, GameObject>();

	int mCurrAvailableEntityId = 0;

	void Start()
    {
		mNetManager = GameObject.FindWithTag("Manager").GetComponent<NetManager>();
	}

    void Update()
    {
        
    }

	public void AddNewClient(EndPoint client)
	{
		int clientID = Global.GetNewClientID();
		Global.mClients[client] = clientID;
		Global.mClientsR[clientID] = client;

		NetStream writer = new NetStream();
		writer.WriteInt32(Global.mCmd["SC_CONTROL_CONNECT_SUCCESS"]);
		writer.WriteInt32(clientID);
		mNetManager.AddMsg(new Msg(writer.GetBuffer(), client));
	}

	public void StartGame()
	{
		NetStream writer = new NetStream();
		writer.WriteInt32(Global.mCmd["SC_GAME_START"]);
		mNetManager.AddMsg(new Msg(writer.GetBuffer()));

		for (int i=0;i< Global.mClients.Count; i++)
		{
			GameObject tank = Instantiate(mTank, mTankSpawnPoints[i].position, mTankSpawnPoints[i].rotation) as GameObject;

			Attribute attribute = tank.GetComponent<Attribute>();
			attribute.SetEntityID(GetAvailableEntityID());
			attribute.SetClientID(i+1);
			attribute.SetName("Tank");

			mGameObjects[attribute.GetEntityID()] = tank;
			mPlayerGameObjects[i+1] = tank;
		}
	}

	public int GetAvailableEntityID()
	{
		return mCurrAvailableEntityId++;
	}

	public void PlayerControl(int clientID, float v, float h, float a, int j)
	{
		if (mPlayerGameObjects.ContainsKey(clientID))
		{
			mPlayerGameObjects[clientID].GetComponent<TankMovement>().SetMovementInput(v, h);
			mPlayerGameObjects[clientID].GetComponent<TankShooting>().SetFireInput(j);
		}
	}
}
