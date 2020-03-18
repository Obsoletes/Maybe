using System;
using System.Collections.Generic;
using System.Text;

namespace Observer
{
	public static class MaybeExtension
	{
		public static void TryThrow<T>(this Maybe<T> maybe) where T : notnull
		{
			if (maybe.HasException)
				throw new AggregateException(maybe.InnerException!);
		}
		public static Maybe<T> Or<T>(this Maybe<T> that, Maybe<T> or1) where T : notnull
		{
			if (that.HasValue)
				return that;
			else
				return or1;
		}
		public static Maybe<T> Or<T>(this Maybe<T> that, Maybe<T> or1, Maybe<T> or2) where T : notnull
		{
			if (that.HasValue)
				return that;
			else if (or1.HasValue)
				return or1;
			else
				return or2;
		}
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
