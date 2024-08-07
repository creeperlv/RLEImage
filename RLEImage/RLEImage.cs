using RLEImage.Utilities;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RLEImage
{
	public unsafe struct RLEImage : IDisposable
	{
		public uint W;
		public uint H;
		public Pixel* Pixels;
		/// <summary>
		/// Bytes Per Pixel
		/// </summary>
		public byte BPP;
		public RLEImage(uint w, uint h) : this()
		{
			W = w;
			H = h;
			BPP = (byte)sizeof(Pixel);
			Pixels = (Pixel*)Marshal.AllocHGlobal((int)(sizeof(Pixel) * W * H));
		}
		public void Save(Stream s)
		{
			s.Write(Defines.Header);
			s.WriteByte(Defines.CurrentVersion);
			s.WriteData(W);
			s.WriteData(H);
			s.WriteByte(BPP);
			if (W * H == 0) return;
			Pixel p = Pixels[0];
			byte L = 1;
			for (int i = 1; i < W * H; i++)
			{
				if (Pixels[i] != p)
				{
					s.WriteData(p);
					s.WriteData(L);
					p = Pixels[i];
					L = 1;
				}
				else
				{
					L++;
					if (L == byte.MaxValue)
					{
						s.WriteData(p);
						s.WriteData(L);
						L = 0;
					}
				}
			}
			s.WriteData(p);
			s.WriteData(L);
			s.Flush();
		}
		public static bool LoadFromStream(Stream s, out RLEImage img)
		{
			Span<byte> bytes = stackalloc byte[Defines.Header.Length];
			s.Read(bytes);
			for (int i = 0; i < Defines.Header.Length; i++)
			{
				if (bytes[i] != Defines.Header[i])
				{
					img = default;
					return false;
				}
			}
			var Version = s.ReadByte();
			if (Version == -1)
			{
				img = default;
				return false;
			}
			uint W;
			uint H;
			W = s.ReadData<uint>();
			H = s.ReadData<uint>();

			var BBP = s.ReadByte();
			if (BBP == -1)
			{
				img = default;
				return false;
			}
			RLEImage image = new RLEImage(W, H)
			{
				BPP = (byte)BBP
			};
			int Pixels = 0;
			int ToRead = (int)(W * H);
			while (Pixels < ToRead)
			{
				var P = s.ReadData<Pixel>();
				var l = s.ReadByte();
				if (l == -1)
				{
					img = default;
					return false;
				}
				else
				{
					for (int i = 0; i < l; i++)
					{
						image.Pixels[Pixels] = P;
						Pixels++;
					}
				}
			}
			img = image;
			return true;
		}
		public void Dispose() => Marshal.FreeHGlobal((IntPtr)Pixels);
	}
}
