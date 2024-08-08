using System;
using System.Runtime.InteropServices;

namespace RLEImage
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Pixel2
	{
		public byte RG;
		public byte BA;
		public static implicit operator Pixel4(Pixel2 p)
		{
			Pixel4 r = default;
			r.R = (byte)((p.RG >> 4) / 15f * 255f);
			r.G = (byte)((p.RG & 0x0F) / 15f * 255f);
			r.B = (byte)((p.BA >> 4) / 15f * 255f);
			r.A = (byte)((p.BA & 0x0F) / 15f * 255f);
			return r;
		}
		public static implicit operator Pixel2(Pixel4 p)
		{
			Pixel2 p2 = default;
			byte d = 0;
			d = (byte)(p.R / 255f * 15f);
			p2.RG = (byte)(d << 4);
			d = (byte)(p.G / 255f * 15f);
			p2.RG = (byte)(p2.RG | (d & 0x0F));
			d = (byte)(p.B / 255f * 15f);
			p2.BA = (byte)(d << 4);
			d = (byte)(p.A / 255f * 15f);
			p2.BA = (byte)(p2.BA | (d & 0x0F));
			return p2;
		}

		public static bool operator ==(Pixel2 L, Pixel2 R)
		{
			return L.RG == R.RG && L.BA == R.BA;
		}
		public static bool operator !=(Pixel2 L, Pixel2 R)
		{
			return L.RG != R.RG || L.BA != R.BA;
		}

		public override bool Equals(object? obj)
		{
			return obj is Pixel2 pixel &&
				   RG == pixel.RG &&
				   BA == pixel.BA;
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct Pixel1
	{
		public byte RGBA;
		public static implicit operator Pixel4(Pixel1 p)
		{
			Pixel4 r = default;
			r.R = (byte)((p.RGBA >> 6) / 3f * 255f);
			r.G = (byte)((p.RGBA >> 4 & 0b11) / 3f * 255f);
			r.B = (byte)((p.RGBA >> 2 & 0b11) / 3f * 255f);
			r.A = (byte)((p.RGBA & 0b11) / 3f * 255f);
			return r;
		}
		public static implicit operator Pixel1(Pixel4 p)
		{
			Pixel1 p2 = default;
			byte d = 0;
			d = (byte)(p.R / 255f * 3f);
			p2.RGBA = (byte)(d << 6);
			d = (byte)(p.G / 255f * 3f);
			p2.RGBA = (byte)(p2.RGBA | (((d & 0b11) << 4) & 0b110000));
			d = (byte)(p.B / 255f * 3f);
			p2.RGBA = (byte)(p2.RGBA | (((d & 0b11) << 2) & 0b1100));
			d = (byte)(p.A / 255f * 3f);
			p2.RGBA = (byte)(p2.RGBA | (d & 0b11));
			return p2;
		}
		public static bool operator ==(Pixel1 L, Pixel1 R)
		{
			return L.RGBA==R.RGBA;
		}
		public static bool operator !=(Pixel1 L, Pixel1 R)
		{
			return L.RGBA != R.RGBA;
		}

		public override bool Equals(object? obj)
		{
			return obj is Pixel1 pixel &&
				   RGBA == pixel.RGBA;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(RGBA);
		}
	}
}
