using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.IO;

public class NetManager : MonoBehaviour
{
	MsgManager mMsgManager;

	Server mServer;
	LinkedList<byte[]> mSendMsgList = new LinkedList<byte[]>();
	LinkedList<byte[]> mRecvMsgList = new LinkedList<byte[]>();
	object mLockSend = new object();
	object mLockRecv = new object();

	void Awake()
	{
		LoadCmd();
	}

	void Start()
	{
		mMsgManager = GameObject.FindWithTag("Manager").GetComponent<MsgManager>();

		mServer = new Server();

		Thread recvMsgThread = new Thread(RecvMsgThread);
		recvMsgThread.IsBackground = true;
		recvMsgThread.Start();
	}

	void Update()
	{
		SendMsg();
		HandleMsg();
	}

	public void AddMsg(byte[] msg)
	{
		lock (mLockSend)
		{
			mSendMsgList.AddLast(msg);
		}
	}

	void SendMsg()
	{
		lock (mLockSend)
		{
			foreach (byte[] ba in mSendMsgList)
			{
				mServer.Send(ba);
			}
			mSendMsgList.Clear();
		}
	}

	void RecvMsgThread()
	{
		while (true)
		{
			byte[] msg = mServer.Receive();
			lock (mLockRecv)
			{
				mRecvMsgList.AddLast(msg);
			}
		}
	}

	void HandleMsg()
	{
		lock (mLockRecv)
		{
			foreach (byte[] msg in mRecvMsgList)
			{
				mMsgManager.Handle(msg);
			}
			mRecvMsgList.Clear();
		}
	}

	void LoadCmd()
	{
		try
		{
			// 创建一个 StreamReader 的实例来读取文件 
			// using 语句也能关闭 StreamReader
			using (StreamReader sr = new StreamReader(Global.mPath + "cmd.txt"))
			{
				string line;

				// 跳过第一行
				line = sr.ReadLine();

				int index = 0;

				// 从文件读取行，直到文件的末尾 
				while ((line = sr.ReadLine()) != null)
				{
					line = line.Trim();
					if (line == "") continue;

					string[] sub = line.Split();
					Global.mCmd[sub[0]] = index++;
				}
			}
		}
		catch (Exception e)
		{
			// 显示出错消息
			Debug.Log("The file could not be read:");
			Debug.Log(e.Message);
		}
	}
}
