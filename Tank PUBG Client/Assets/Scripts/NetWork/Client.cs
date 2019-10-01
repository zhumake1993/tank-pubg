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

class Client
{
	string mServerIP = "127.0.0.1";
	int mServerPort = 10081;
	IPEndPoint mIPEndPointServer = null;
	EndPoint mEndPointServer = null;

	string mClientIP = "127.0.0.1";
	int mClientPort = -1;
	IPEndPoint mIPEndPointClient = null;
	EndPoint mEndPointClient = null;
	Socket mSocketClient = null;
	byte[] mByteReceiveArray = null;

	public Client()
	{
		// 创建服务端
		mIPEndPointServer = new IPEndPoint(IPAddress.Parse(mServerIP), mServerPort);
		mEndPointServer = (EndPoint)mIPEndPointServer;

		// 创建客户端
		mSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		mByteReceiveArray = new byte[Global.mMaxRecvSize];

		while (true)
		{
			try
			{
				mClientPort = GetAvailablePort();
				mIPEndPointClient = new IPEndPoint(IPAddress.Parse(mClientIP), mClientPort);
				mEndPointClient = (EndPoint)mIPEndPointClient;
				mSocketClient.Bind(mEndPointClient);
				break;
			}
			catch (Exception)
			{
			}
		}
	}

	~Client()
	{
		mSocketClient.Shutdown(SocketShutdown.Both);
		mSocketClient.Close();
	}

	public void Send(byte[] byteArray)
	{
		mSocketClient.SendTo(byteArray, mEndPointServer);
	}

	public byte[] Receive()
	{
		int intReceiveLenght = mSocketClient.ReceiveFrom(mByteReceiveArray, ref mEndPointServer);
		byte[] res = new byte[intReceiveLenght];
		for(int i=0;i< intReceiveLenght; i++)
		{
			res[i] = mByteReceiveArray[i];
		}
		return res;
	}

	// 获取一个未被占用的端口号（注意，该端口号可能在函数返回后被占用）
	static int GetAvailablePort()
	{
		int MAX_PORT = 65535;
		int MIN_PORT = 5000;

		for (int i = MIN_PORT; i <= MAX_PORT; i++)
		{
			if (PortIsAvailable(i)) return i;
		}

		return -1;
	}

	// 获取所有被占用的端口号
	static List<int> PortIsUsed()
	{
		IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
		IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();
		IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();
		TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

		List<int> allPorts = new List<int>();
		foreach (IPEndPoint ep in ipsTCP) allPorts.Add(ep.Port);
		foreach (IPEndPoint ep in ipsUDP) allPorts.Add(ep.Port);
		foreach (TcpConnectionInformation conn in tcpConnInfoArray) allPorts.Add(conn.LocalEndPoint.Port);

		return allPorts;
	}

	// 判断该端口是否被占用
	static bool PortIsAvailable(int port)
	{
		bool isAvailable = true;
		List<int> portUsed = PortIsUsed();

		foreach (int p in portUsed)
		{
			if (p == port)
			{
				isAvailable = false;
				break;
			}
		}

		return isAvailable;
	}
}