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
			int d = 0b00111111;
			float v = d;
			byte R = (byte)((p.R / 255f) * v);
			byte G = (byte)((p.G / 255f) * v);
			byte B = (byte)((p.B / 255f) * v);
			byte A = (byte)((p.A / 255f) * v);

			var tempR0 = R << 2;
			var tempG0 = G >> 4;
			var tempG1 = G << 4;
			var tempB0 = B >> 2;
			var tempB1 = B << 6;

			r.RG = (byte)(tempR0 | (tempG0 & 0b11));

			r.GB = (byte)(tempG1 | (tempB0 & 0b00001111));

			r.BA = (byte)(tempB1 | (A & 0b00111111));
			return r;
		}
		public static bool operator ==(Pixel3 L, Pixel3 R)
		{
			return L.RG == R.RG && L.GB == R.GB && L.BA == R.BA;
		}
		public static bool operator !=(Pixel3 L, Pixel3 R)
		{
			return L.RG != R.RG || L.GB != R.GB || L.BA != R.BA;
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
