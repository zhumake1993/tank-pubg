using System.Collections.Generic;
using System.Net;

public class Global
{
	public static string mPath = @"F:\Tank PUBG\";
	public static int mMaxRecvSize = 512;
	public static Dictionary<string, int> mCmd = new Dictionary<string, int>();
	public static Dictionary<EndPoint, int> mClients = new Dictionary<EndPoint, int>();

	static int mClientCount = 0;

	public static int GetNewClientID()
	{
		mClientCount += 1;
		return mClientCount;
	}
}
