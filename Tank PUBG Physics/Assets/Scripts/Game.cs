using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public NetManager mNetManager;

	public GameObject mCube;

	Dictionary<int, GameObject> mGameObjects = new Dictionary<int, GameObject>();

	void Start()
    {
        
    }

    void Update()
    {
        
    }

	public void StartGame()
	{
		GameObject cube = Instantiate(mCube, mCube.transform.position, mCube.transform.rotation) as GameObject;
		mGameObjects[1] = cube;

		// id
		// mGameObjects



		NetStream writer = new NetStream();
		writer.WriteInt32(Global.mCmd["PS_INSTANTIATE"]);
		writer.WriteInt32(1);
		writer.WriteString("Cube");
		writer.WriteFloat(mCube.transform.position.x);
		writer.WriteFloat(mCube.transform.position.y);
		writer.WriteFloat(mCube.transform.position.z);
		writer.WriteFloat(mCube.transform.rotation.x);
		writer.WriteFloat(mCube.transform.rotation.y);
		writer.WriteFloat(mCube.transform.rotation.z);
		writer.WriteFloat(mCube.transform.rotation.w);
		mNetManager.AddMsg(writer.GetBuffer());
	}
}
