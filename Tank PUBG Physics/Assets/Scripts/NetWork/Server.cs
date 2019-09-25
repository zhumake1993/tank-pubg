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

class Server
{
	string mServerIP = "127.0.0.1";
	int mServerPort = 10081;
	IPEndPoint mIPEndPointServer = null;
	EndPoint mEndPointServer = null;
	Socket mSocketServer = null;
	byte[] mByteReceiveArray = null;

	string mClientIP = "127.0.0.1";
	int mClientPort = 10080;
	IPEndPoint mIPEndPointClient = null;
	EndPoint mEndPointClient = null;

	public Server()
	{
		// 创建服务端
		mIPEndPointServer = new IPEndPoint(IPAddress.Parse(mServerIP), mServerPort);
		mEndPointServer = (EndPoint)mIPEndPointServer;
		mSocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		mByteReceiveArray = new byte[512];
		mSocketServer.Bind(mEndPointServer);

		// 创建客户端
		mIPEndPointClient = new IPEndPoint(IPAddress.Parse(mClientIP), mClientPort);
		mEndPointClient = (EndPoint)mIPEndPointClient;
	}

	~Server()
	{
		mSocketServer.Shutdown(SocketShutdown.Both);
		mSocketServer.Close();
	}

	public void Send(byte[] byteArray)
	{
		mSocketServer.SendTo(byteArray, mEndPointClient);
	}

	public byte[] Receive()
	{
		int intReceiveLenght = mSocketServer.ReceiveFrom(mByteReceiveArray, ref mEndPointClient);
		byte[] res = new byte[intReceiveLenght];
		for (int i = 0; i < intReceiveLenght; i++)
		{
			res[i] = mByteReceiveArray[i];
		}
		return res;
	}
}