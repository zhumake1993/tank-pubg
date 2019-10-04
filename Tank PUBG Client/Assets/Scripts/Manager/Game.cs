using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public GameObject mTankMain;
	public GameObject mTankOther;
	public GameObject mShell;

	Dictionary<string, GameObject> mPrefabs = new Dictionary<string, GameObject>();
	Dictionary<int, GameObject> mGameObjects = new Dictionary<int, GameObject>();

	void Start()
    {
		mPrefabs["TankMain"] = mTankMain;
		mPrefabs["TankOther"] = mTankOther;
		mPrefabs["Shell"] = mShell;
	}

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
		GameObject entity = mGameObjects[entityID];

		ShellExplosion shellExplosion = entity.GetComponent<ShellExplosion>();
		if (shellExplosion)
		{
			shellExplosion.PlayAudio();
		}

		TankHealth tankHealth = entity.GetComponent<TankHealth>();
		if (tankHealth)
		{
			tankHealth.Destroy();
		}

		Destroy(mGameObjects[entityID]);
		mGameObjects.Remove(entityID);
	}

	public void PlayEngineAudio(int entityID, bool engineDriving)
	{
		GameObject tank = mGameObjects[entityID];
		tank.GetComponent<TankMovement>().PlayEngineAudio(engineDriving);
	}

	public void SetTankHealth(int entityID, float health)
	{
		GameObject tank = mGameObjects[entityID];
		tank.GetComponent<TankHealth>().SetTankHealth(health);
	}

	public void PlayChargingAudio(int entityID)
	{
		GameObject tank = mGameObjects[entityID];
		tank.GetComponent<TankShooting>().PlayChargingClip();
	}

	public void SetAimSlider(int entityID, float val)
	{
		GameObject tank = mGameObjects[entityID];
		tank.GetComponent<TankShooting>().SetAimSlider(val);
	}

	public void PlayFireAudio(int entityID)
	{
		GameObject tank = mGameObjects[entityID];
		tank.GetComponent<TankShooting>().PlayFireClip();
	}
}
