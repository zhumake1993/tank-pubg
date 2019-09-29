using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute : MonoBehaviour
{
	[SerializeField] int mEntityID = -1;
	[SerializeField] int mClientID = -1;
	[SerializeField] string mName = "";

	public void SetEntityID(int entityID){mEntityID = entityID;}
	public int GetEntityID() { return mEntityID; }

	public void SetClientID(int clientID) { mClientID = clientID; }
	public int GetClientID() { return mClientID; }

	public void SetName(string name){mName = name;}
	public string GetName() { return mName; }
}
