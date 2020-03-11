using System;
using System.Diagnostics;

internal class CustomStopwatch : Stopwatch, IDisposable
{
	public CustomStopwatch() : base()
	{
//		
		base.Start();
	}

	public void Dispose()
	{
		base.Stop();
		Console.WriteLine("Elapsed: {0} ms", base.ElapsedMilliseconds);
	}
}
