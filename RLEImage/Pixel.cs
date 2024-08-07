using System;
using System.Runtime.InteropServices;

namespace RLEImage
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Pixel
	{
		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public override bool Equals(object? obj)
		{
			return obj is Pixel pixel &&
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
		public static bool operator ==(Pixel L, Pixel R)
		{
			return L.R == R.R && L.G == R.G && L.B == R.B && L.A == R.A;
		}
		public static bool operator !=(Pixel L, Pixel R)
		{
			return L.R != R.R ||
				L.G != R.G ||
				L.B != R.B ||
				L.A != R.A;
		}
	}
}
