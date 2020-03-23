using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
	/// <summary>
	/// Extension for Maybe
	/// </summary>
	public static class MaybeExtension
	{
		/// <summary>
		/// if maybe has exception, throw it
		/// </summary>
		/// <exception cref="System.AggregateException"></exception>
		/// <typeparam name="T">type</typeparam>
		/// <param name="maybe">this</param>
		public static void TryThrow<T>(this Maybe<T> maybe) where T : notnull
		{
			if (maybe.HasException)
				throw new AggregateException(maybe.InnerException!);
		}
		/// <summary>
		/// if this maybe has value,then nothing happen
		/// else return maybe with Maybe(value)
		/// </summary>
		/// <returns>a maybe</returns>
		public static Maybe<T> Or<T>(this Maybe<T> that, Maybe<T> or1) where T : notnull
		{
			if (that.HasValue)
				return that;
			else
				return or1;
		}
		/// <summary>
		/// if this maybe has value,then nothing happen
		/// else return maybe with Maybe(value)
		/// </summary>
		/// <returns>a maybe</returns>
		public static Maybe<T> Or<T>(this Maybe<T> that, Maybe<T> or1, Maybe<T> or2) where T : notnull
		{
			if (that.HasValue)
				return that;
			else if (or1.HasValue)
				return or1;
			else
				return or2;
		}
		/// <summary>
		/// if this maybe has value,then nothing happen
		/// else return maybe with Maybe(value)
		/// </summary>
		/// <returns>a maybe</returns>
		public static Maybe<T> Or<T>(this Maybe<T> that, Maybe<T> or1, Maybe<T> or2, Maybe<T> or3) where T : notnull
		{
			if (that.HasValue)
				return that;
			else if (or1.HasValue)
				return or1;
			else if (or2.HasValue)
				return or2;
			else
				return or3;
		}
		/// <summary>
		/// if this maybe has value,then nothing happen
		/// else return maybe with Maybe(value)
		/// </summary>
		/// <returns>a maybe</returns>
		public static Maybe<T> Or<T>(this Maybe<T> that, Maybe<T> or1, Maybe<T> or2, Maybe<T> or3, Maybe<T> or4) where T : notnull
		{
			if (that.HasValue)
				return that;
			else if (or1.HasValue)
				return or1;
			else if (or2.HasValue)
				return or2;
			else if (or3.HasValue)
				return or3;
			else
				return or4;
		}
		/// <summary>
		/// if this maybe has value,then nothing happen
		/// else return maybe with Maybe(value)
		/// </summary>
		/// <returns>a maybe</returns>
		public static Maybe<T> Or<T>(this Maybe<T> that, Maybe<T> or1, Maybe<T> or2, Maybe<T> or3, Maybe<T> or4, Maybe<T> or5) where T : notnull
		{
			if (that.HasValue)
				return that;
			else if (or1.HasValue)
				return or1;
			else if (or2.HasValue)
				return or2;
			else if (or3.HasValue)
				return or3;
			else if (or4.HasValue)
				return or4;
			else
				return or5;
		}
		/// <summary>
		/// if this maybe has value,then nothing happen
		/// else return maybe with Maybe(value)
		/// </summary>
		/// <returns>a maybe</returns>
		public static Maybe<T> Or<T>(this Maybe<T> that, params Maybe<T>[] ors) where T : notnull
		{
			if (that.HasValue)
				return that;
			foreach (var or in ors)
			{
				if (or.HasValue)
					return or;
			}
			return that;
		}
	}
}
