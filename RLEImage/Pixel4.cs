using System;
using System.Runtime.InteropServices;

namespace RLEImage
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Pixel4
	{
		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public override bool Equals(object? obj)
		{
			return obj is Pixel4 pixel &&
				   R == pixel.R &&
				   G == pixel.G &&
				   B == pixel.B &&
				   A == pixel.A;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(R, G, B, A);
		}

		public string ToRGBAString()
		{
			return $"{R},{G},{B},{A}";
		}
		public static bool operator ==(Pixel4 L, Pixel4 R)
		{
			return L.R == R.R && L.G == R.G && L.B == R.B && L.A == R.A;
		}
		public static bool operator !=(Pixel4 L, Pixel4 R)
		{
			return L.R != R.R ||
				L.G != R.G ||
				L.B != R.B ||
				L.A != R.A;
		}
	}
}
