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
		public Pixel4* Pixels;
		/// <summary>
		/// Bytes Per Pixel
		/// </summary>
		public byte BPP;
		public RLEImage(uint w, uint h) : this()
		{
			W = w;
			H = h;
			BPP = (byte)sizeof(Pixel4);
			Pixels = (Pixel4*)Marshal.AllocHGlobal((int)(sizeof(Pixel4) * W * H));
		}
		public void Save(Stream s)
		{
			s.Write(Defines.Header);
			s.WriteByte(Defines.CurrentVersion);
			s.WriteData(W);
			s.WriteData(H);
			s.WriteByte(BPP);
			if (W * H == 0) return;
			Pixel4 p = Pixels[0];
			byte L = 1;
			for (int i = 1; i < W * H; i++)
			{
				switch (this.BPP)
				{
					case 4:
					default:
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
						break;
					case 3:
						{
							var item = (Pixel3)Pixels[i];
							var RP= (Pixel3)p;
							if (item != RP)
							{
								s.WriteData(RP);
								s.WriteData(L);
								p = Pixels[i];
								L = 1;
							}
							else
							{
								L++;
								if (L == byte.MaxValue)
								{
									s.WriteData(RP);
									s.WriteData(L);
									L = 0;
								}
							}
						}
						break;
					case 2:
						{

							var item = (Pixel2)Pixels[i];
							var RP= (Pixel2)p;
							if (item != RP)
							{
								s.WriteData(RP);
								s.WriteData(L);
								p = Pixels[i];
								L = 1;
							}
							else
							{
								L++;
								if (L == byte.MaxValue)
								{
									s.WriteData(RP);
									s.WriteData(L);
									L = 0;
								}
							}
						}
						break;
					case 1:
						{

							var item = (Pixel1)Pixels[i];
							Pixel1 RP = (Pixel1)p;
							if (item != RP)
							{
								s.WriteData(RP);
								s.WriteData(L);
								p = Pixels[i];
								L = 1;
							}
							else
							{
								L++;
								if (L == byte.MaxValue)
								{
									s.WriteData(RP);
									s.WriteData(L);
									L = 0;
								}
							}
						}
						break;
				}
			}
			{
				switch (this.BPP)
				{
					default:
					case 4:
						s.WriteData(p);
						break;
					case 3:
						s.WriteData((Pixel3)p);
						break;
					case 2:
						s.WriteData((Pixel2)p);
						break;
					case 1:
						s.WriteData((Pixel1)p);
						break;
				}
			}
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

			var BPP = s.ReadByte();
			if (BPP == -1)
			{
				img = default;
				return false;
			}
			RLEImage image = new RLEImage(W, H)
			{
				BPP = (byte)BPP
			};
			int Pixels = 0;
			int ToRead = (int)(W * H);
			while (Pixels < ToRead)
			{
				Pixel4 P;
				switch (BPP)
				{
					case 4:
					default:
						P = s.ReadData<Pixel4>();
						break;
					case 3:
						P = (Pixel4)s.ReadData<Pixel3>();
						break;
					case 2:
						P = (Pixel4)s.ReadData<Pixel2>();
						break;
					case 1:
						P = (Pixel4)s.ReadData<Pixel1>();
						break;
				}
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
