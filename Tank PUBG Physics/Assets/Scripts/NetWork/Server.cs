using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using UnityEngine;

public class Msg
{
	public byte[] mMsg;
	public EndPoint mClient;

	public Msg(byte[] msg=null, EndPoint client=null)
	{
		mMsg = msg;
		mClient = client;
	}
}

class Server
{
	string mServerIP = "127.0.0.1";
	int mServerPort = 10081;
	IPEndPoint mIPEndPointServer = null;
	EndPoint mEndPointServer = null;
	Socket mSocketServer = null;
	byte[] mByteReceiveArray = null;

	public Server()
	{
		// 创建服务端
		mIPEndPointServer = new IPEndPoint(IPAddress.Parse(mServerIP), mServerPort);
		mEndPointServer = (EndPoint)mIPEndPointServer;
		mSocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		mByteReceiveArray = new byte[Global.mMaxRecvSize];
		mSocketServer.Bind(mEndPointServer);
	}

	~Server()
	{
		mSocketServer.Shutdown(SocketShutdown.Both);
		mSocketServer.Close();
	}

	public void Send(Msg msg)
	{
		if(msg.mClient != null)
		{
			mSocketServer.SendTo(msg.mMsg, msg.mClient);
		}
		else
		{
			foreach (EndPoint client in Global.mClients.Keys)
			{
				mSocketServer.SendTo(msg.mMsg, client);
			}
		}

	}

	public Msg Receive()
	{
		IPEndPoint IPEndPointClient = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10800);
		EndPoint endPointClient = (EndPoint)IPEndPointClient;

		int intReceiveLenght = mSocketServer.ReceiveFrom(mByteReceiveArray, ref endPointClient);
		byte[] data = new byte[intReceiveLenght];
		for (int i = 0; i < intReceiveLenght; i++)
		{
			data[i] = mByteReceiveArray[i];
		}

		Msg msg = new Msg(data, endPointClient);
		return msg;
	}
}