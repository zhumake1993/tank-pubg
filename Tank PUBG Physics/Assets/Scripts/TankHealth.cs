using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
	public float mStartingHealth = 1f;

	NetManager mNetManager;
	Attribute mAttribute;
	float mCurrentHealth;

	void Start()
    {
		mNetManager = GameObject.FindWithTag("Manager").GetComponent<NetManager>();
		mAttribute = GetComponent<Attribute>();
		mCurrentHealth = mStartingHealth;
		SynHealth();
	}

    void Update()
    {
        
    }

	public void TakeDamage(float amount)
	{
		mCurrentHealth -= amount;
		SynHealth();

		if (mCurrentHealth <= 0f)
		{
			Destroy(gameObject);
		}
	}

	void SynHealth()
	{
		NetStream writer = new NetStream();
		writer.WriteInt32(Global.mCmd["SC_SYN_TANK_HEALTH"]);
		writer.WriteInt32(mAttribute.GetEntityID());
		writer.WriteFloat(mCurrentHealth);
		mNetManager.AddMsg(new Msg(writer.GetBuffer()));
	}
}
