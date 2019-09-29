using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public GameObject mTankMain;
	public GameObject mTankOther;

	Dictionary<string, GameObject> mPrefabs = new Dictionary<string, GameObject>();
	Dictionary<int, GameObject> mGameObjects = new Dictionary<int, GameObject>();

	// Start is called before the first frame update
	void Start()
    {
		mPrefabs["TankMain"] = mTankMain;
		mPrefabs["TankOther"] = mTankOther;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Instantiate(int entityID, int clientID, string name,Vector3 postion,Quaternion rotation)
	{
		if (name == "Tank")
		{
			if (clientID == Global.mClientID)
			{
				GameObject tank = Instantiate(mPrefabs["TankMain"], postion, rotation) as GameObject;
				mGameObjects[entityID] = tank;
			}
			else
			{
				GameObject tank = Instantiate(mPrefabs["TankOther"], postion, rotation) as GameObject;
				mGameObjects[entityID] = tank;
			}
		}
		else
		{
			GameObject tank = Instantiate(mPrefabs[name], postion, rotation) as GameObject;
			mGameObjects[entityID] = tank;
		}
	}

	public void SetTransform(int entityID, Vector3 postion, Quaternion rotation)
	{
		mGameObjects[entityID].transform.position = postion;
		mGameObjects[entityID].transform.rotation = rotation;
	}

	public void Destroy(int entityID)
	{
		Destroy(mGameObjects[entityID]);
		mGameObjects.Remove(entityID);
	}
}
