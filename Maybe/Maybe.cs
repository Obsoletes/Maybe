using System;
using System.Collections.Generic;
#nullable enable
namespace Observer
{
	public struct Maybe<T> : IEquatable<Maybe<T>> 
	{
		public static Maybe<T> None { get; }
		public static implicit operator Maybe<T>(T obj)
		{
			return new Maybe<T>(obj);
		}

		public Maybe(T value)
		{
			this.value = value;
			InnerException = null;
		}
		public bool HasValue { get => value != null; }
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
		public Maybe<TOut> Then<TOut>(Func<T, TOut> func)
		{
			if (HasValue)
				return new Maybe<TOut>(func(value!));
			else
				return Maybe<TOut>.None;
		}
		public void Then<TOut>(Action<T> action)
		{
			if (HasValue)
				action(value!);
		}
		public Maybe<TOut> ThenNoThrow<TOut>(Func<T, TOut> func)
		{
			try
			{
				if (HasValue)
					return new Maybe<TOut>(func(value!));
				else
					return Maybe<TOut>.None;
			}
			catch (Exception ex)
			{
				InnerException = ex;
				return Maybe<TOut>.None;
			}
		}
		public void ThenNoThrow<TOut>(Action<T> action)
		{
			try
			{
				if (HasValue)
					action(value!);
			}
			catch (Exception ex)
			{
				InnerException = ex;
			}
		}
		public Maybe<T> Or(T obj)
		{
			if (HasValue)
				return this;
			else
				return new Maybe<T>(obj);
		}
		private readonly T value;

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
	}
}
