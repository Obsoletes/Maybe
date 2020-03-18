# Maybe 1.0
<p align="left">
	<img src='https://img.shields.io/github/workflow/status/Obsoletes/Maybe/.NET Core'>
	<img src='https://img.shields.io/nuget/v/Observer.Maybe.svg'>
</p>

>一个 C# 版本的 `Maybe` 

## Feature 

========================= 

- 基于 struct 实现，不存在 null 的可能
- 支持链式调用
- 支持捕获异常
- 支持 C# 8 的 `switch` 模式匹配

## Api

========================

	public bool HasValue { get; }
	public bool HasException { get; }
	public Exception? InnerException { get; }
	public T Value { get; }
	public Maybe<TOut> Then<TOut>(Func<T, TOut> func);
	public MaybeResult Then<TOut>(Action<T> action);
	public Maybe<TOut> ThenNoThrow<TOut>(Func<T, TOut> func) ;
	public MaybeResult ThenNoThrow(Action<T> action);
	public Maybe<T> Or(T obj);

## Install

	dotnet add package Observer.Maybe 
	
or

	Install-Package Observer.Maybe

## Usage

	Maybe<string> maybe = "123456";
	var testObj = maybe.Then(s => s.Substring(0, 3));
	var result = testObj switch
	{
		(true, string value) => value,
		(false, _) => "None"
	};

## ChangeLog

>**1.0** 2020-3-18
>>基本实现

## License

[MIT](LICENSE) © Richard Littauer
