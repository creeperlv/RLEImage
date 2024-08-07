using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RLEImage.Utilities
{
	public static unsafe class StreamUtilities
	{
		public static void WriteData(this Stream stream, byte[] data)
		{
			stream.WriteData(data);
		}
		public static void WriteData<T>(this Stream s, T data) where T : unmanaged
		{
			byte* ptr = (byte*)&data;
			ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(ptr, sizeof(T));
			s.Write(buffer);
		}
		public static T ReadData<T>(this Stream s) where T : unmanaged
		{
			T t=default(T);
			Span<byte> buffer = new Span<byte>(&t,sizeof(T));
			s.Read(buffer);
			return t;
		}
	}
}
