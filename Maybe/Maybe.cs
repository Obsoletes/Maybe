using System;
using System.Collections.Generic;
#nullable enable
namespace Observer
{
	public struct Maybe<T> : IEquatable<Maybe<T>> where T : notnull
	{
		public delegate bool TryFunction<TIn, TResult>(TIn a, out TResult b);
		public static Maybe<T> None { get; }
		public static implicit operator Maybe<T>(T obj)
		{
			return new Maybe<T>(obj);
		}
		public Maybe(T value)
		{
			this.value = value;
			InnerException = null;
			HasValue = true;
			if (typeof(T).IsClass)
			{
				HasValue = ((value as object) != null);
			}
		}
		public bool HasValue { get; }
		public bool HasException { get => InnerException != null; }
		public Exception? InnerException { get; private set; }
		public T Value
		{
			get
			{
				if (HasValue)
					return value!;
				else
					throw new InvalidOperationException("empty maybe");
			}
		}
		public Maybe<TResult> Then<TResult>(TryFunction<T, TResult> @try) where TResult : notnull
		{
			if (HasValue && @try(Value, out TResult result))
			{
				return new Maybe<TResult>(result);
			}
			return new Maybe<TResult>();
		}
		public Maybe<TOut> Then<TOut>(Func<T, TOut> func) where TOut : notnull
		{
			if (HasValue)
				return new Maybe<TOut>(func(value!));
			else
				return ValueResult<TOut>();
		}
		public MaybeResult Then<TOut>(Action<T> action)
		{
			if (HasValue)
				action(value!);
			return EmptyResult();
		}
		public Maybe<TOut> ThenNoThrow<TOut>(Func<T, TOut> func) where TOut : notnull
		{
			try
			{
				if (HasValue)
					return new Maybe<TOut>(func(value!));
				else
					return ValueResult<TOut>();
			}
			catch (Exception ex)
			{
				return ValueResult<TOut>(ex);
			}
		}
		public MaybeResult ThenNoThrow(Action<T> action)
		{
			try
			{
				if (HasValue)
					action(value!);
				return EmptyResult();
			}
			catch (Exception ex)
			{
				return EmptyResult(ex);
			}
		}
		public Maybe<T> Or(T obj)
		{
			if (HasValue)
				return this;
			else
				return new Maybe<T>(obj);
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));
			if (obj is Maybe<T> right)
				return this == right;
			else
				return false;
		}
		public bool Equals(Maybe<T> right)
		{
			return !(HasValue ^ right.HasValue) && EqualityComparer<T>.Default.Equals(value, right.value);
		}
		public bool Equals<TRight>(Maybe<TRight> right) where TRight : class
		{
			if (!HasValue && !right.HasValue)
				return true;
			else
				return false;
		}
		public override int GetHashCode()
		{
			if (!HasValue)
				return 0;
			else
				return value!.GetHashCode();
		}
		public static bool operator ==(Maybe<T> left, Maybe<T> right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Maybe<T> left, Maybe<T> right)
		{
			return !left.Equals(right);
		}
		public void Deconstruct(out bool hasValue, out T value)
		{
			hasValue = this.HasValue;
			value = this.Value;
		}
		private readonly T value;
		private Maybe<TOut> ValueResult<TOut>(Exception? ex = null) where TOut : notnull
		{
			return new Maybe<TOut>
			{
				InnerException = ex ?? InnerException
			};
		}
		private MaybeResult EmptyResult(Exception? ex = null)
		{
			return new MaybeResult(ex ?? InnerException);
		}
	}
}