using RLEImage.Utilities;
namespace RLEImage.Converter
{
	public enum ConvertWay
	{
		P2R,
		R2P,
	}
	internal class Program
	{

		static unsafe void Main(string[] args)
		{
			string? inputFile = null;
			string? outputFile = null;
			ConvertWay way = ConvertWay.P2R;
			for (int i = 0; i < args.Length; i++)
			{
				var item = args[i];
				switch (item)
				{
					case "-s":
						i++;
						inputFile = args[i];
						break;
					case "-o":
						i++;
						outputFile = args[i];
						break;
					case "-p":
						way = ConvertWay.R2P;
						break;
					case "-r":
						way = ConvertWay.P2R;
						break;
					default:
						break;
				}
			}
			if (inputFile == null)
			{
				Console.WriteLine("No input file!");
				return;
			}
			if (outputFile == null) { outputFile = inputFile + ".rli"; }
			switch (way)
			{
				case ConvertWay.P2R:
					{
						var image = SkiaSharp.SKBitmap.Decode(inputFile);
						using RLEImage image1 = new RLEImage((uint)image.Width, (uint)image.Height);
						for (int y = 0; y < image.Height; y++)
						{
							for (int x = 0; x < image.Width; x++)
							{
								var p = image.GetPixel(x, y);
								Pixel _p = new()
								{
									R = p.Red,
									G = p.Green,
									B = p.Blue,
									A = p.Alpha
								};
								image1.Pixels[y * image.Width + x] = _p;
							}
						}
						using var ws = File.OpenWrite(outputFile);
						image1.Save(ws);
					}
					break;
				case ConvertWay.R2P:
					{
						using var fs = File.OpenRead(inputFile);
						if (RLEImage.LoadFromStream(fs, out var image))
						{
							using SkiaSharp.SKBitmap map = new SkiaSharp.SKBitmap((int)image.W, (int)image.H, false);
							for (int x = 0; x < map.Width; x++)
							{
								for (int y = 0; y < map.Height; y++)
								{
									var p = image.Pixels[y * map.Width + x];
									map.SetPixel(x, y, new SkiaSharp.SKColor(p.R, p.G, p.B, p.A));
								}
							}
							using var ofs = File.OpenWrite(outputFile);
							map.NotifyPixelsChanged();
							map.Encode(ofs, SkiaSharp.SKEncodedImageFormat.Png, 100);
							ofs.Flush();
						}
						else
						{
							Console.WriteLine("Load Failed!");
						}
					}
					break;
				default:
					break;
			}
		}
	}
}
