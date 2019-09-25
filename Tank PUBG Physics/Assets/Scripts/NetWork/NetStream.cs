using System;
using System.IO;
using System.Net;
using UnityEngine;

class NetStream
{
	MemoryStream mStream;
	BinaryReader mReader;
	BinaryWriter mWriter;

	public NetStream(byte[] buffer = null)
	{
		if (buffer == null)
		{
			mStream = new MemoryStream();
		}
		else
		{
			mStream = new MemoryStream(buffer);
		}

		mReader = new BinaryReader(mStream);
		mWriter = new BinaryWriter(mStream);
	}

	public void Close()
	{
		mStream.Close();
		mReader.Close();
		mWriter.Close();
	}

	//-------------------------------------------------------------------------------

	public long ReadInt64()
	{
		return IPAddress.HostToNetworkOrder(mReader.ReadInt64());
	}

	public int ReadInt32()
	{
		return IPAddress.HostToNetworkOrder(mReader.ReadInt32());
	}

	public short ReadInt16()
	{
		return IPAddress.HostToNetworkOrder(mReader.ReadInt16());
	}

	public byte ReadByte()
	{
		return mReader.ReadByte();
	}

	public float ReadFloat()
	{
		byte[] temp = mReader.ReadBytes(4);
		Array.Reverse(temp);
		return BitConverter.ToSingle(temp, 0);
	}

	public double ReadDouble()
	{
		byte[] temp = mReader.ReadBytes(8);
		Array.Reverse(temp);
		return BitConverter.ToDouble(temp, 0);
	}

	public string ReadString()
	{
		return System.Text.Encoding.UTF8.GetString(mReader.ReadBytes(ReadInt32()));
	}

	public long Seek(long offset)
	{
		return mStream.Seek(offset, SeekOrigin.Begin);
	}

	//-------------------------------------------------------------------------------

	public void WriteByte(byte value)
	{
		mWriter.Write(value);
	}

	public void WriteInt16(short value)
	{
		mWriter.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
	}

	public void WriteInt32(int value)
	{
		mWriter.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
	}

	public void WriteInt64(long value)
	{
		mWriter.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
	}

	public void WriteFloat(float value)
	{
		byte[] temp = BitConverter.GetBytes(value);
		Array.Reverse(temp);
		mWriter.Write(temp);

	}

	public void WriteDouble(double value)
	{
		byte[] temp = BitConverter.GetBytes(value);
		Array.Reverse(temp);
		mWriter.Write(temp);
	}

	public void WriteString(string value)
	{
		byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(value);
		WriteInt32(byteArray.Length);
		mWriter.Write(byteArray);
	}

	public byte[] GetBuffer()
	{
		mStream.Flush();
		return mStream.ToArray();
	}

	public int GetLength()
	{
		return (int)mStream.Length;
	}
}
