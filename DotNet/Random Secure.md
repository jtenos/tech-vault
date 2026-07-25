```csharp
using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;

public static class SecureRandomizer
{
	private const int RANDOM_BUFFER_SIZE = 1 << 16;
	private static readonly ConcurrentQueue<byte> _randomBuffer = new ConcurrentQueue<byte>();

	static SecureRandomizer()
	{
		RepopulateRandomBuffer();
	}

	public static byte[] GetRandomBytes(int numBytes)
	{
		if (numBytes < 0)
		{
			throw new ArgumentException("Number of bytes must be non-negative");
		}

		if (numBytes == 0)
		{
			return new byte[0];
		}

		var returnBytes = new byte[numBytes];
		for (int i = 0; i < numBytes; ++i)
		{
			byte b;
			while (!_randomBuffer.TryDequeue(out b))
			{
				RepopulateRandomBuffer();
			}
			returnBytes[i] = b;
		}
		return returnBytes;
	}

	private static void RepopulateRandomBuffer()
	{
		var buffer = new byte[RANDOM_BUFFER_SIZE];
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(buffer);
		}
		lock (_randomBuffer)
		{
			for (int i = 0; i < RANDOM_BUFFER_SIZE; ++i)
			{
				_randomBuffer.Enqueue(buffer[i]);
			}
		}
	}
}
```