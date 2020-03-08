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
				(true, var value) => value,
				(false, _) => "None"
			};
			Assert.Equal("123", result);
		}
	}
}
