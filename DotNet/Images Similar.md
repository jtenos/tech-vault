Look at two photos, see if they have the same dimensions, then check to see if they are similar photos. 

It reduces both photos to 16×16 (regardless of original dimensions), 
then compares the RGB values of each pixel. 

If the photos are different enough, that number will be high. 

If the difference is close to zero, then they are likely the same photo, or at least very similar.

```csharp
static bool AreSimilar(string file1, string file2)
{
	Console.WriteLine("Comparing:\r\n  {0}\r\n  {1}", file1, file2);
	using (var image1 = (Bitmap)Image.FromFile(file1))
	{
		using (var image2 = (Bitmap)Image.FromFile(file2))
		{
			var proportion1 = (decimal)image1.Width / image1.Height;
			var proportion2 = (decimal)image2.Width / image2.Height;
			if (Math.Abs(proportion1 - proportion2) > 0.01m)
			{
				return false;
			}


			using (var image1_resize = new Bitmap(image1, new Size(16, 16)))
			{
				using (var image2_resize = new Bitmap(image2, new Size(16, 16)))
				{
					image1_resize.Clone(new Rectangle(0, 0, image1_resize.Width, image1_resize.Height), PixelFormat.Format8bppIndexed);
					image2_resize.Clone(new Rectangle(0, 0, image2_resize.Width, image2_resize.Height), PixelFormat.Format8bppIndexed);


					int totalDiff = 0;
					for (int i = 0; i < 16; ++i)
					{
						for (int j = 0; j < 16; ++j)
						{
							var c1 = image1_resize.GetPixel(i, j);
							var c2 = image2_resize.GetPixel(i, j);
							int diff = Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
							totalDiff += diff;
						}
					}
					Console.WriteLine("Total diff: {0}", totalDiff);
					return totalDiff < 2560;
				}
			}
		}
	}
}
```