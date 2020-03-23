using System;
using System.Collections.Generic;
#nullable enable
namespace Observer
{
	/// <summary>
	/// no sense
	/// </summary>
	public static class Maybe
	{
		/// <summary>
		/// no sense
		/// </summary>
		public delegate bool TryFunction<TIn, TResult>(TIn @in, out TResult @result);
	}
	/// <summary>
	/// Maybe
	/// </summary>
	/// <typeparam name="T">The type of value,should not be System.Nullable&lt;T&gt;</typeparam>
	public readonly struct Maybe<T> : IEquatable<Maybe<T>> where T : notnull
	{
		/// <summary>
		/// Empty Maybe&lt;T&gt;
		/// </summary>
		public static Maybe<T> None { get; }
		/// <summary>
		/// Construct from value
		/// </summary>
		/// <param name="value">value</param>
		public static implicit operator Maybe<T>(T value)
		{
			return new Maybe<T>(value);
		}
		/// <summary>
		/// Construct from value
		/// </summary>
		/// <param name="value"></param>
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
		private Maybe(Exception? exception)
		{
			value = default!;
			InnerException = exception;
			HasValue = false;
		}
		/// <summary>
		/// whether this maybe has value
		/// </summary>
		public bool HasValue { get; }
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
		/// return value if have
		/// </summary>
		/// <exception cref="System.InvalidOperationException">Throw if maybe is empty</exception>
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
		/// <summary>
		/// use function like <c>bool Try(TIn @in, out TResult @result)</c>
		/// The function return value indicates whether the conversion succeeded.
		/// the parameter @result of function is the result of function
		/// won't catch exception throwed by funciton
		/// </summary>
		/// <remarks>
		/// if <paramref name="try"/> return null,the maybe has no value
		/// </remarks>
		/// <typeparam name="TResult">return of <paramref name="try"/></typeparam>
		/// <param name="try">the function</param>
		/// <example>
		/// <code>
		/// Maybe&lt;string&gt; maybe = "123456";
		/// var result = maybe.Then(int.TryParse);
		/// </code>
		/// </example>
		/// <returns>
		/// return maybe with result if maybe has value and function return true
		/// if not, return maybe with nothing
		/// </returns>
		public Maybe<TResult> Then<TResult>(Maybe.TryFunction<T, TResult> @try) where TResult : notnull
		{
			if (HasValue && @try(Value, out TResult result))
			{
				return new Maybe<TResult>(result);
			}
			return new Maybe<TResult>();
		}
		/// <summary>
		/// use function
		/// won't catch exception throwed by funciton
		/// </summary>
		/// <remarks>
		/// if <paramref name="func"/> return null,the maybe has no value
		/// </remarks>
		/// <typeparam name="TResult">return of <paramref name="func"/></typeparam>
		/// <param name="func">function</param>
		/// <returns>
		/// return maybe with result if maybe has value
		/// if not, return maybe with nothing
		/// </returns>
		public Maybe<TResult> Then<TResult>(Func<T, TResult> func) where TResult : notnull
		{
			if (HasValue)
				return new Maybe<TResult>(func(value!));
			else
				return ValueResult<TResult>();
		}
		/// <summary>
		/// use function
		/// won't catch exception throwed by funciton
		/// </summary>
		/// <param name="action">function</param>
		/// <returns>
		/// return useless MaybeResult
		/// </returns>
		public MaybeResult Then(Action<T> action)
		{
			if (HasValue)
				action(value!);
			return EmptyResult();
		}
		/// <summary>
		/// use function like <c>bool Try(TIn @in, out TResult @result)</c>
		/// The function return value indicates whether the conversion succeeded.
		/// the parameter @result of function is the result of function
		/// will catch exception throwed by funciton
		/// </summary>
		/// <remarks>
		/// if <paramref name="try"/> return null,the maybe has no value
		/// </remarks>
		/// <typeparam name="TResult">return of <paramref name="try"/></typeparam>
		/// <param name="try">the function</param>
		/// <example>
		/// <code>
		/// Maybe&lt;string&gt; maybe = "123456";
		/// var result = maybe.Then(int.TryParse);
		/// </code>
		/// </example>
		/// <returns>
		/// return maybe with result if maybe has value and function return true
		/// if not, return maybe with nothing
		/// </returns>
		public Maybe<TResult> ThenNoThrow<TResult>(Maybe.TryFunction<T, TResult> @try) where TResult : notnull
		{
			try
			{
				if (HasValue && @try(Value, out TResult result))
				{
					return new Maybe<TResult>(result);
				}
				return new Maybe<TResult>();
			}
			catch (Exception ex)
			{
				return ValueResult<TResult>(ex);
			}
			
		}
		/// <summary>
		/// use function
		/// will catch exception throwed by funciton and put it in the returned maybe
		/// </summary>
		/// <remarks>
		/// if <paramref name="func"/> return null,the maybe has no value
		/// </remarks>
		/// <typeparam name="TResult">return of <paramref name="func"/></typeparam>
		/// <param name="func">function</param>
		/// <returns>
		/// return maybe with result if maybe has value
		/// if not, return maybe with nothing
		/// if exception throwed, return maybe with exception
		/// </returns>
		public Maybe<TResult> ThenNoThrow<TResult>(Func<T, TResult> func) where TResult : notnull
		{
			try
			{
				if (HasValue)
					return new Maybe<TResult>(func(value!));
				else
					return ValueResult<TResult>();
			}
			catch (Exception ex)
			{
				return ValueResult<TResult>(ex);
			}
		}
		/// <summary>
		/// use function
		/// will catch exception throwed by funciton and put it in the returned MaybeResult
		/// </summary>
		/// <param name="action">function</param>
		/// <returns>
		/// if exception throwed, return MaybeResult with exception
		/// else return empty MaybeResult
		/// </returns>
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
		/// <summary>
		/// if this maybe has value,then nothing happen
		/// else return maybe with Maybe(value)
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>a maybe</returns>
		public Maybe<T> Or(T value)
		{
			if (HasValue)
				return this;
			else
				return new Maybe<T>(value);
		}
		/// <summary>
		/// Indicates whether this is equal to obj
		/// </summary>
		/// <exception cref="System.ArgumentNullException">throw if obj is null</exception>
		/// <param name="obj">other object</param>
		/// <returns>whether this is equal to obj</returns>
		public override bool Equals(object? obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));
			if (obj is Maybe<T> right)
				return this == right;
			else
				return false;
		}
		/// <summary>
		/// Indicates whether this is equal to other Maybe
		/// </summary>
		/// <param name="other">other maybe</param>
		/// <returns>whether this is equal to obj</returns>
		public bool Equals(Maybe<T> other)
		{
			return !(HasValue ^ other.HasValue) && EqualityComparer<T>.Default.Equals(value, other.value);
		}
		/// <summary>
		/// Indicates whether this is equal to other Maybe
		/// </summary>
		/// <typeparam name="TRight">other</typeparam>
		/// <param name="other">other</param>
		/// <returns>only return true when both this and other are empty</returns>
		public bool Equals<TRight>(Maybe<TRight> other) where TRight : class
		{
			if (!HasValue && !other.HasValue)
				return true;
			else
				return false;
		}
		/// <summary>
		/// get hash code
		/// </summary>
		/// <returns>
		/// return value's hash code if this is not empty
		/// else return 0
		/// </returns>
		public override int GetHashCode()
		{
			if (!HasValue)
				return 0;
			else
				return value!.GetHashCode();
		}
		/// <summary>
		/// Indicates whether this is equal to other Maybe
		/// </summary>
		/// <param name="left">a maybe</param>
		/// <param name="right">a maybe</param>
		/// <returns>whether this is equal to other Maybe</returns>
		public static bool operator ==(Maybe<T> left, Maybe<T> right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Indicates whether this is not equal to other Maybe
		/// </summary>
		/// <param name="left">a maybe</param>
		/// <param name="right">a maybe</param>
		/// <returns>whether this is not equal to other Maybe</returns>
		public static bool operator !=(Maybe<T> left, Maybe<T> right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// helper function for pattern match
		/// </summary>
		/// <param name="hasValue"></param>
		/// <param name="value"></param>
		public void Deconstruct(out bool hasValue, out T value)
		{
			hasValue = this.HasValue;
			value = this.Value;
		}
		private readonly T value;
		private Maybe<TOut> ValueResult<TOut>(Exception? ex = null) where TOut : notnull
		{
			return new Maybe<TOut>(ex);
		}
		private MaybeResult EmptyResult(Exception? ex = null)
		{
			return new MaybeResult(ex ?? InnerException);
		}
	}
}