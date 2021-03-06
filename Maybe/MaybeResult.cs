﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Observer
{
	/// <summary>
	/// The no result version of maybe
	/// </summary>
	public readonly struct MaybeResult
	{
		/// <summary>
		/// whether this maybe has exception
		/// </summary>
		public bool HasException { get => InnerException != null; }
		/// <summary>
		/// return the exception if have 
		/// return null if not 
		/// </summary>
		public Exception? InnerException { get; }
		/// <summary>
		/// Construct from exception
		/// </summary>
		/// <param name="ex"></param>
		internal MaybeResult(Exception? ex)
		{
			InnerException = ex;
		}
	}
}
