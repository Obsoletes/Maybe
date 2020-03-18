using System;
using Xunit;
using Observer;
namespace Observer.Test
{
	public class UnitTest1
	{
		[Fact]
		public void TestEqual()
		{
			string str = "123";
			Assert.True(((Maybe<string>)str).Equals(str));
			Assert.True(Maybe<string>.None.Equals(Maybe<string>.None));
			Assert.True(Maybe<string>.None.Equals(Maybe<object>.None));
		}
		[Fact]
		public void TestFunction()
		{
			Maybe<string> maybe = "123456";
			Assert.True(maybe.Then(s => s.Substring(0, 5)).Then(s => s.Length) == 5);
		}
		[Fact]
		public void TestMatch()
		{
			Maybe<string> maybe = "123456";
			var testObj = maybe.Then(s => s.Substring(0, 3));
			var result = testObj switch
			{
				(true, string value) => value,
				(false, _) => "None"
			};
			Assert.Equal("123", result);
		}
		[Fact]
		public void TestCatch()
		{
			Maybe<int> maybe = 123;
			var result = maybe.ThenNoThrow(i => i / 0).ThenNoThrow(i => Console.WriteLine(i));
			Assert.True(result.InnerException is DivideByZeroException);
		}
		[Fact]
		public void TestConstruct()
		{
			Assert.False(new Maybe<object>(null).HasValue);
			Maybe<object> t1 = new Maybe<object>();
			Maybe<object> t2 = null;
			Assert.False(t1.HasValue);
			Assert.False(t2.HasValue);
			Maybe<int> t3 = new Maybe<int>();
			Maybe<int> t4 = 0;
			Assert.False(t3.HasValue);
			Assert.True(t4.HasValue);
		}
	}
}
