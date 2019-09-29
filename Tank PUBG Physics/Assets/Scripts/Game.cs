using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public GameObject mTank;
	public Transform[] mTankSpawnPoints;

	NetManager mNetManager;

	ArrayList mClientIDs = new ArrayList();
	Dictionary<int, GameObject> mGameObjects = new Dictionary<int, GameObject>();
	Dictionary<int, GameObject> mPlayerGameObjects = new Dictionary<int, GameObject>();
	int mCurrAvailableEntityId = 1;

	void Start()
    {
		mNetManager = GameObject.FindWithTag("Manager").GetComponent<NetManager>();
	}

    void Update()
    {
        
    }

	public void AddNewClient(int clientID)
	{
		mClientIDs.Add(clientID);
	}

	public void StartGame()
	{
		for(int i=0;i< mClientIDs.Count; i++)
		{
			GameObject tank = Instantiate(mTank, mTankSpawnPoints[i].position, mTankSpawnPoints[i].rotation) as GameObject;

			int entityID = GetAvailableEntityID();
			mGameObjects[entityID] = tank;
			mPlayerGameObjects[(int)mClientIDs[i]] = tank;

			Attribute attribute = tank.GetComponent<Attribute>();
			attribute.SetEntityID(entityID);
			attribute.SetClientID((int)mClientIDs[i]);
			attribute.SetName("Tank");
		}
	}

	int GetAvailableEntityID()
	{
		return mCurrAvailableEntityId++;
	}

	public void PlayerControl(int playerID, float v, float h, float a, bool j)
	{
		mPlayerGameObjects[playerID].GetComponent<Test>().Move(v, h);
	}
}
