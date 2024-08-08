using System;
using System.Runtime.InteropServices;

namespace RLEImage
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Pixel3
	{
		public byte RG;
		public byte GB;
		public byte BA;
		public static implicit operator Pixel4(Pixel3 p)
		{
			Pixel4 r = default;
			r.R = (byte)((p.RG >> 2) / 63f * 255f);
			r.G = (byte)((((p.RG << 4) & 0b00110000) | (p.GB >> 4)) / 63f * 255f);
			r.B = (byte)((((p.GB << 2) & 0b111100) | (p.BA >> 6)) / 63f * 255f);
			r.A = (byte)((p.BA & 0b111111) / 63f * 255f);
			return r;
		}
		public static implicit operator Pixel3(Pixel4 p)
		{
			Pixel3 r = default;
			//
			//RRRRRRGG_GGGGBBBB_BBAAAAAA
			byte R = (byte)(p.R / 255f * 63f);
			var temp = R << 2;
			r.RG = (byte)(temp & 0b11111100);

			var G = (byte)(p.G / 255f * 63f);
			temp = G >> 4;

			r.RG = (byte)(r.RG | (temp & 0b11));
			temp = G << 4;

			r.GB = (byte)(temp & 0b11110000);

			byte B = (byte)(p.B / 255f * 63f);
			temp = B >> 2;
			r.GB = (byte)(r.GB | (temp & 0b00001111));
			temp = B << 6;
			r.BA = (byte)(temp & 0b11000000);

			byte A = (byte)(p.A / 255f * 63f);
			r.BA = (byte)(r.BA | (A & 0b00111111));
			return r;
		}
		public static bool operator ==(Pixel3 L, Pixel3 R)
		{
			return L.RG == R.RG && L.GB == R.GB && L.BA == R.BA;
		}
		public static bool operator !=(Pixel3 L, Pixel3 R)
		{
			return L.RG != R.RG && L.GB != R.GB && L.BA != R.BA;
		}

		public override bool Equals(object? obj)
		{
			return obj is Pixel3 pixel &&
				   RG == pixel.RG &&
				   GB == pixel.GB &&
				   BA == pixel.BA;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(RG, GB, BA);
		}
	}
}
