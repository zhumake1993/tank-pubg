using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class NetManager : MonoBehaviour
{
	Server mServer;
	LinkedList<byte[]> mSendMsgList = new LinkedList<byte[]>();
	LinkedList<byte[]> mRecvMsgList = new LinkedList<byte[]>();

	int mWaitTime = 100;

	void Awake()
	{
		mServer = new Server();
	}

	void Update()
	{
		RecvMsg();
		SendMsg();
	}

	public void AddMsg(byte[] msg)
	{
		mSendMsgList.AddLast(msg);
	}

	void SendMsg()
	{
		if (mSendMsgList.Count == 0) return;

		NetStream writer = new NetStream();
		writer.WriteInt32(0);
		writer.WriteInt32(mSendMsgList.Count);
		mServer.Send(writer.GetBuffer());

		foreach (byte[] ba in mSendMsgList)
		{
			mServer.Send(ba);
		}

		mSendMsgList.Clear();
	}

	void RecvMsg()
	{
		Thread recvMsgThread = new Thread(RecvMsgThread);
		recvMsgThread.Start();

		bool status = recvMsgThread.Join(mWaitTime);
		recvMsgThread.Abort();

		if (status)
		{
			foreach (byte[] ba in mRecvMsgList)
			{
				NetStream reader = new NetStream(ba);
				int cmd = reader.ReadInt32();

				if (cmd == 0)
				{
					int len = reader.ReadInt32();
					//Debug.Log(cmd.ToString() + " " + len.ToString());
				}
				else
				{
					float v = reader.ReadFloat();
					float h = reader.ReadFloat();
					//Debug.Log(v.ToString() + " " + h.ToString());

					GameObject test = GameObject.Find("Cube");
					test.GetComponent<Test>().Move(v, h);

					
				}
			}
		}
		else
		{
			Debug.Log("fail");
		}

		mRecvMsgList.Clear();
	}

	void RecvMsgThread()
	{
		byte[] msg = mServer.Receive();
		mRecvMsgList.AddLast(msg);

		msg = mServer.Receive();
		mRecvMsgList.AddLast(msg);
	}
}
