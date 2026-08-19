```csharp
using BenchmarkDotNet.Running;
using Benchmarks;

// Welcome message
Console.WriteLine("=== BenchmarkDotNet Examples ===");
Console.WriteLine("This project demonstrates various types of benchmarks.\n");

// Run benchmarks based on command line arguments, or show menu
if (args.Length == 0)
{
	ShowMenu();
}
else if (args[0] == "--all")
{
	RunAllBenchmarks();
}
else if (args[0] == "--string")
{
	BenchmarkRunner.Run<StringConcatenationBenchmarks>();
}
else if (args[0] == "--collection")
{
	BenchmarkRunner.Run<CollectionBenchmarks>();
}
else if (args[0] == "--linq")
{
	BenchmarkRunner.Run<LinqVsLoopBenchmarks>();
}
else if (args[0] == "--param")
{
	BenchmarkRunner.Run<ParameterizedBenchmarks>();
}
else if (args[0] == "--memory")
{
	BenchmarkRunner.Run<MemoryAllocationBenchmarks>();
}
else
{
	Console.WriteLine("Unknown argument. Use one of: --all, --string, --collection, --linq, --param, --memory");
}

static void ShowMenu()
{
	Console.WriteLine("Run benchmarks by passing one of these arguments:");
	Console.WriteLine("  --all        Run all benchmarks");
	Console.WriteLine("  --string     String concatenation benchmarks");
	Console.WriteLine("  --collection Collection performance benchmarks");
	Console.WriteLine("  --linq       LINQ vs loops benchmarks");
	Console.WriteLine("  --param      Parameterized benchmarks");
	Console.WriteLine("  --memory     Memory allocation benchmarks");
	Console.WriteLine("\nExample: dotnet run -c Release -- --string");
}

static void RunAllBenchmarks()
{
	Console.WriteLine("Running all benchmarks...\n");
	BenchmarkRunner.Run<StringConcatenationBenchmarks>();
	BenchmarkRunner.Run<CollectionBenchmarks>();
	BenchmarkRunner.Run<LinqVsLoopBenchmarks>();
	BenchmarkRunner.Run<ParameterizedBenchmarks>();
	BenchmarkRunner.Run<MemoryAllocationBenchmarks>();
}
```

## Collection
```csharp
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

/// <summary>
/// Demonstrates performance differences between various collection types.
/// Shows the impact of choosing the right collection for your use case.
/// </summary>
[MemoryDiagnoser]
public class CollectionBenchmarks
{
	private const int Count = 1000;
	private readonly int[] _data = Enumerable.Range(0, Count).ToArray();

	[Benchmark(Baseline = true)]
	public int ListAdd()
	{
		List<int> list = new();
		foreach (int item in _data)
		{
			list.Add(item);
		}
		return list.Count;
	}

	[Benchmark]
	public int ListAddWithCapacity()
	{
		List<int> list = new(Count);
		foreach (int item in _data)
		{
			list.Add(item);
		}
		return list.Count;
	}

	[Benchmark]
	public int ArrayFill()
	{
		int[] array = new int[Count];
		for (int i = 0; i < Count; i++)
		{
			array[i] = _data[i];
		}
		return array.Length;
	}

	[Benchmark]
	public int HashSetAdd()
	{
		HashSet<int> set = new();
		foreach (int item in _data)
		{
			set.Add(item);
		}
		return set.Count;
	}

	[Benchmark]
	public int DictionaryAdd()
	{
		Dictionary<int, int> dict = new();
		foreach (int item in _data)
		{
			dict.Add(item, item);
		}
		return dict.Count;
	}
}
```

## LINQ Vs Loop
```csharp
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

/// <summary>
/// Compares LINQ queries versus traditional for/foreach loops.
/// Shows the performance trade-offs between code readability and execution speed.
/// </summary>
[MemoryDiagnoser]
public class LinqVsLoopBenchmarks
{
	private readonly int[] _numbers = Enumerable.Range(0, 1000).ToArray();

	[Benchmark(Baseline = true)]
	public int ForLoop()
	{
		int sum = 0;
		for (int i = 0; i < _numbers.Length; i++)
		{
			if (_numbers[i] % 2 == 0)
			{
				sum += _numbers[i];
			}
		}
		return sum;
	}

	[Benchmark]
	public int ForeachLoop()
	{
		int sum = 0;
		foreach (int number in _numbers)
		{
			if (number % 2 == 0)
			{
				sum += number;
			}
		}
		return sum;
	}

	[Benchmark]
	public int LinqWhereSum()
	{
		return _numbers.Where(x => x % 2 == 0).Sum();
	}

	[Benchmark]
	public int LinqAggregateWithWhere()
	{
		return _numbers.Where(x => x % 2 == 0).Aggregate(0, (acc, x) => acc + x);
	}

	[Benchmark]
	public int LinqSumWithPredicate()
	{
		return _numbers.Sum(x => x % 2 == 0 ? x : 0);
	}
}
```

## Memory Allocation
```csharp
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

/// <summary>
/// Demonstrates memory allocation patterns and their performance impact.
/// Shows how to use [MemoryDiagnoser] to track memory allocations.
/// </summary>
[MemoryDiagnoser]
public class MemoryAllocationBenchmarks
{
	private const int Iterations = 100;

	[Benchmark(Baseline = true)]
	public List<int> CreateListsInLoop()
	{
		List<int> result = new();
		for (int i = 0; i < Iterations; i++)
		{
			List<int> temp = new() { i };
			result.AddRange(temp);
		}
		return result;
	}

	[Benchmark]
	public List<int> ReuseList()
	{
		List<int> result = new();
		List<int> temp = new();
		for (int i = 0; i < Iterations; i++)
		{
			temp.Clear();
			temp.Add(i);
			result.AddRange(temp);
		}
		return result;
	}

	[Benchmark]
	public List<int> DirectAdd()
	{
		List<int> result = new();
		for (int i = 0; i < Iterations; i++)
		{
			result.Add(i);
		}
		return result;
	}

	[Benchmark]
	public string[] CreateStringsInLoop()
	{
		string[] result = new string[Iterations];
		for (int i = 0; i < Iterations; i++)
		{
			result[i] = i.ToString();
		}
		return result;
	}

	[Benchmark]
	public string[] CreateStringsWithInterpolation()
	{
		string[] result = new string[Iterations];
		for (int i = 0; i < Iterations; i++)
		{
			result[i] = $"{i}";
		}
		return result;
	}
}
```

## Parameterized
```csharp
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

/// <summary>
/// Demonstrates parameterized benchmarks that test the same code with different input values.
/// This is useful for understanding how performance scales with input size.
/// </summary>
[MemoryDiagnoser]
public class ParameterizedBenchmarks
{
	[Params(10, 100, 1000)]
	public int Size { get; set; }

	private int[]? _data;

	[GlobalSetup]
	public void Setup()
	{
		_data = Enumerable.Range(0, Size).ToArray();
	}

	[Benchmark]
	public int SumArray()
	{
		int sum = 0;
		for (int i = 0; i < _data!.Length; i++)
		{
			sum += _data[i];
		}
		return sum;
	}

	[Benchmark]
	public int SumLinq()
	{
		return _data!.Sum();
	}

	[Benchmark]
	public int[] ReverseArray()
	{
		int[] result = new int[_data!.Length];
		for (int i = 0; i < _data.Length; i++)
		{
			result[i] = _data[_data.Length - 1 - i];
		}
		return result;
	}

	[Benchmark]
	public int[] ReverseLinq()
	{
		return _data!.Reverse().ToArray();
	}
}
```

## String Concatenation
```csharp
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

/// <summary>
/// Demonstrates benchmarking different string concatenation approaches.
/// This shows performance differences between string concatenation, StringBuilder, and string.Create.
/// </summary>
[MemoryDiagnoser]
public class StringConcatenationBenchmarks
{
	private const int Iterations = 100;

	[Benchmark(Baseline = true)]
	public string ConcatenationOperator()
	{
		string result = "";
		for (int i = 0; i < Iterations; i++)
		{
			result += i.ToString();
		}
		return result;
	}

	[Benchmark]
	public string StringBuilderAppend()
	{
		StringBuilder sb = new();
		for (int i = 0; i < Iterations; i++)
		{
			sb.Append(i);
		}
		return sb.ToString();
	}

	[Benchmark]
	public string StringBuilderCapacity()
	{
		StringBuilder sb = new(Iterations * 3); // Pre-allocate capacity
		for (int i = 0; i < Iterations; i++)
		{
			sb.Append(i);
		}
		return sb.ToString();
	}

	[Benchmark]
	public string StringJoin()
	{
		return string.Join("", Enumerable.Range(0, Iterations));
	}

	[Benchmark]
	public string StringConcat()
	{
		return string.Concat(Enumerable.Range(0, Iterations));
	}
}
```