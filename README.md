# Smocks

Smocks is an experimental framework for "static mocking" for .NET 4 and .NET 4.5. It is not a full-featured mocking framework, but rather a supplement to existing frameworks such as [moq](http://www.moqthis.com/). These frameworks typically do not support mocking of static or non-virtual methods and properties. Smocks fills the gap.

## Usage

Smocks uses some magic under the hood to mock the normally unmockable. This magic has its (technical) limitations though. Therefore it's very important that you [play by the rules](https://github.com/vanderkleij/Smocks/wiki/Technical-limitations). Most importantly: avoid using variables defined outside the scope of `Smocks.Run` unless you know what you're doing. These are some *valid* scenarios:

### Static properties
```C#
Smock.Run(context =>
{
    context.Setup(() => DateTime.Now).Returns(new DateTime(2000, 1, 1));

    // Outputs "2000"
    Console.WriteLine(DateTime.Now.Year);
});
```

### Static methods
```C#
Smock.Run(context =>
{
    int fortytwo = 42;
    context.Setup(() => int.TryParse("forty-two", out fortytwo)).Returns(true);

    int outResult;
    bool result = int.TryParse("forty-two", out outResult);

    // Outputs "True, 42"
    Console.WriteLine("{0}, {1}", result, outResult);
});
```

### Non-virtual methods
```C#
Smock.Run(context =>
{
    context.Setup(() => It.IsAny<string>().Equals("Bar")).Returns(true);

    // Outputs "True"
    Console.WriteLine("Foo".Equals("Bar"));
});
```

### Non-virtual properties
```C#
Smock.Run(context =>
{
    context.Setup(() => It.IsAny<string>().Length).Returns(42);

    // Outputs "42"
    Console.WriteLine("Four".Length);
});
```

## Installation

Available on [NuGet](https://www.nuget.org/packages/Smocks/):

```
PM> Install-Package Smocks
```

## Roadmap

- ~~Strategies for deciding which assemblies to rewrite~~
- ~~`.Returns((arg1, arg2) => ...)`~~
- ~~`.Callback((arg1, arg2) => ...)`~~
- ~~Matching `It.Is<T>(x => ...)`~~
- Strong-named assemblies
- `.SetupSet(() => ...)`
- Support for mocking events

## Disclaimer
This library is currently in alpha status. I expect plenty of bugs to still be present. Should you encounter any oddities, please submit an issue or pull request. Any feedback will be greatly appreciated.

## Credits

- [mono.cecil](http://cecil.pe/) (MIT license): the mindblowing CIL reading/writing library that powers Smocks.
- [moq](http://www.moqthis.com/): the awesome mocking framework that heavily inspired the syntax and functionality of Smocks.
- [AppDomainToolkit](https://github.com/jduv/AppDomainToolkit): a great source of knowledge on the intricacies of `AppDomain`.