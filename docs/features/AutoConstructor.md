---
layout: default
title: AutoConstructor
nav_order: 2
parent: FEATURES
---

## AutoConstructor

> Automatically generate constructor for class

### 快速入门
常见的N层架构中，层与层之间往往有强烈的依赖关系，我们通常使用依赖注入，通过构造函数，将下一层的依赖对象注入到当前层的实现中。但是大部分情况下，这些构造函数都长的很相似，因为它们都做了很类似的事情：赋值和为空检查，例如下面的代码片段。

```csharp
[ApiController]
[Route("[controller]")]
public class MyController : ControllerBase
{
    public MyController(IMyService myservice, ILogger<MyController> logger)
    {
        _myservice = myservice ?? throw new ArgumentNullException(nameof(myservice));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private readonly IMyService _myservice;
    private readonly ILogger<MyController> _logger;

    [HttpPost]
    public async Task Run(string arg)
    {
        _logger.LogInformation("run invoked");
        await _myservice.Run(arg);
    }
}
```

大多数情况下，书写这些构造函数都是重复性的工作。比如我们如果想在当前的`MyController`中再增加一个依赖的服务`IMyService2`，就必须额外定义一个字段，然后在构造函数中也定义一个相同类型的局部参数，并且需要在构造函数体中将此字段赋值并做为空检查。

使用了`FlyTiger`后，上面的示例代码就可以被简化。

```csharp
[AutoConstructor(NullCheck = true)]
[ApiController]
[Route("[controller]")]
public partial class MyController : ControllerBase
{
    private readonly IMyService myService;
    private readonly ILogger<MyController> logger;

    [HttpPost]
    public async Task Run(string arg)
    {
        logger.LogInformation("run invoked");
        await myservice.Run(arg);
    }
}
```

简化后的代码片段和之前的代码片段功能上是一模一样的，仅仅在类的定义上加了`[AutoConstructor(NullCheck = true)]`特性， 这样`FlyTiger`会在编译期间自动生成以下代码

```csharp
partial class MyController
{
    public MyController(IMyService myService, ILogger<MyController> logger)
    {
        this.myService = myService ?? throw new System.ArgumentNullException(nameof(myService));
        this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
    }
}
```
因为用了*partial*关键字，所以dotnet在编译期间会将生成的代码和原有的代码片段合并到一起编译，这样就和手写的构造函数起到了相同的作用。如果在此情况下我们想给`MyController`中再增加一个依赖的服务`IMyService2`，那么只需要在`MyController`类中额外再定义一个额外的字段即可。

```csharp
[AutoConstructor(NullCheck = true)]
[ApiController]
[Route("[controller]")]
public partial class MyController : ControllerBase
{
    private readonly IMyService myService;
    private readonly IMyService2 myService2;
    private readonly ILogger<MyController> logger;

    [HttpPost]
    public async Task Run(string arg)
    {
        logger.LogInformation("run invoked");
        await myservice.Run(arg);
    }
}
```

### 注意事项
- 确保加了`AutoConstructor`特性的类加了**partial**关键字的修饰。
- 确保没有再定义其它的构造函数，如果你也手动定义了相同参数的构造函数，这个构造函数将会和生成的构造函数冲突导致编译失败。


### 控制为空检查的行为

`AutoConstructor`提供了一个属性`NullCheck`，用于控制对于引用类型的对象是否要做为空检查，默认为`false`表示不做为空检查。上面示例的代码，如果不加`NullCheck`，则会生成以下代码片段。


```csharp
partial class MyController
{
    public MyController(IMyService myService, ILogger<MyController> logger)
    {
        this.myService = myService;
        this.logger = logger;
    }
}
```
### 忽略的字段或属性

默认情况下 `AutoConstructor`会遍历当前类中所有的字段和可写的属性，生成构造函数。在某些情况下，你可能不需要将某些字段或属性通过构造函数赋值。那么你只需要给忽略的字段或属性加上`AutoConstructorIgnore`特性即可。

```csharp
[AutoConstructor(NullCheck = true)]
[ApiController]
[Route("[controller]")]
public partial class MyController : ControllerBase
{
    [AutoConstructorIgnore]
    private readonly int myValue = 10;
    private readonly IMyService myService;
    private readonly ILogger<MyController> logger;
    
    [HttpPost]
    public async Task Run(string arg)
    {
        logger.LogInformation("run invoked");
        await myservice.Run(arg);
    }
}
```
上面的代码片段仍旧只会生成两个参数的构造函数，因为`myValue`的字段上面加了`AutoConstructorIgnore`特性。如果去掉`AutoConstructorIgnore`，则会生成三个参数的构造函数。

### 内部嵌套类

如果你需要在内部嵌套类上面使用`AutoConstructor`，那么也需要确保外层的类上面加了**partial**关键字，例如下面的代码段。
```csharp
using FlyTiger;
namespace ClassLibrary1
{
    public partial class Class1
    {

        [AutoConstructor]
        public partial class User
        {
            private readonly string name;
            private readonly int age;
        }
    }
}
```
对应生成的构造函数代码片段如下
```csharp
namespace ClassLibrary1
{
    partial class Class1
    {
        partial class User
        {
            public User(string name, int age)
            {
                this.name = name;
                this.age = age;
            }
        }
    }
```

### 有继承关系的构造函数
如果定义了`AutoConstructor`特性的类有继承的父类，默认会继承父类中最少参数的构造函数，例如下面的代码片段。
```csharp
using FlyTiger;
namespace ClassLibrary1
{
    public partial class Person
    {
        public Person(Guid id)
        {
            this.Id = id;
        }
        public Guid Id { get; set; }
    }

    [AutoConstructor]
    public partial class User : Person
    {
        private readonly string name;
        private readonly int age;
    }
}
```

对应生成的构造函数代码片段如下
```csharp
namespace ClassLibrary1
{
    partial class User
    {
        public User(global::System.Guid id, string name, int age)
            : base(id: id)
        {
            this.name = name;
            this.age = age;
        }
    }
}
```


