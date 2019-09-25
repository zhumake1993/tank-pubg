using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public GameObject mCube;

	Dictionary<string, GameObject> mPrefabs = new Dictionary<string, GameObject>();
	Dictionary<int, GameObject> mGameObjects = new Dictionary<int, GameObject>();

	// Start is called before the first frame update
	void Start()
    {
		mPrefabs["Cube"] = mCube;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Instantiate(int entityID, string name,Vector3 postion,Quaternion rotation)
	{
		GameObject cube = Instantiate(mPrefabs[name], postion, rotation) as GameObject;
		mGameObjects[entityID] = cube;
	}

	public void SetTransform(int entityID, Vector3 postion, Quaternion rotation)
	{
		mGameObjects[entityID].transform.position = postion;
		mGameObjects[entityID].transform.rotation = rotation;
	}
}
