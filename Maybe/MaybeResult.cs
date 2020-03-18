using System;
using System.Collections.Generic;
using System.Text;

namespace Observer
{
	public struct MaybeResult
	{
		public bool HasException { get => InnerException != null; }
		public Exception? InnerException { get; private set; } 
		public MaybeResult(Exception? ex)
		{
			InnerException = ex;
		}
	}
}
