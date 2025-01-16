---
layout: default
title: FEATURES
nav_order: 2
description: "getting started"
has_children: true
section_id: features
---

## 1. SingletonPattern

### 入门示例
定义单例模式，在需要定义的类上加上```[SingletonPattern]```特性，并保证类加上```partial```修饰符
```csharp
[SingletonPattern]
public partial class Service1
{
    public void SayHello(string name)
    {
        Console.WriteLine($"Hello,{name}");
    }
}
```
上面的代码，因为定义了```[SingletonPattern]```特性，```SourceGenerator```会自动帮我们生成以下代码

```csharp
partial class Service1
{
    private static readonly Lazy<Service1> LazyInstance = new Lazy<Service1>(() => new Service1(), true);

    private Service1() { }

    public static Service1 Instance => LazyInstance.Value;
}
```

使用的代码
```csharp
Service1.Instance.SayHello("Zhang");
```
### 自定义单例属性名称
可以在```[SingletonPattern]```里面加上```InstancePropertyName```，用于指定单例属性的名称。
```csharp
[SingletonPattern(InstancePropertyName = "Default")]
public partial class Service2
{
    public void SayHello(string name)
    {
        Console.WriteLine($"Hello,{name}");
    }
}
```
使用的代码
```csharp
Service2.Default.SayHello("Zhang");
```



## 2. AutoConstructor


### 入门示例

需要在自动生成构造函数的类上面加上```[AutoConstructor]```, 并确保类加上```partial```修饰符，这样就会生成一个包含类内部所有字段和属性的构造函数。

```csharp
[AutoConstructor]
public partial class Demo1
{
    private string strValue;
    private int intValue;
}
```

应用了```[AutoConstructor]```后，```SourceGenerator```会自动生成以下代码片段
```csharp
partial class Demo1
{
    public Demo1(string strValue, int intValue)
    {
        this.strValue = strValue;
        this.intValue = intValue;
    }
}
```
可以看到生成的代码相当于我们手写的代码，并且自动把字段赋值成构造函数传入的值了。

使用的代码如下

```csharp
var demo = new Demo1("val1", 1);
```

### 使用属性

对于自动实现的属性，```AutoConstructor```同样适用。这里要注意的是必须是自动实现的属性，如果属性包含了代码定义，则会忽略改属性。

```csharp
[AutoConstructor]
public partial class Demo2
{
    public string StrValue { get; set; }
    public int IntValue { get; set; }
}
```

使用的代码

```csharp
var demo = new Demo2("val2", 2);
```
### NullCheck

```AutoConstructor```可以使用```NullCheck```的属性，默认值为```false```，当```NullCheck```设置为```true```的时候，对于引用类型，生成的代码就会做```null```值的检测。

```csharp
[AutoConstructor(NullCheck = true)]
public partial class Demo3
{
    private string strValue;
    private int intValue;
}
```
上述代码对应生成的代码如下

```csharp
partial class Demo3
{
    public Demo3(string strValue, int intValue)
    {
        this.strValue = strValue ?? throw new System.ArgumentNullException(nameof(strValue));
        this.intValue = intValue;
    }
}
```

当使用以下代码调用时，会抛出```System.ArgumentNullException```的异常
```csharp
var demo = new Demo3(null, 1);
```
### 忽略字段

如果想要指定某个字段或者属性不通过构造函数赋值，那么可以在该字段或属性上面使用```AutoConstructorIgnore```特性
```csharp
[AutoConstructor]
public partial class Demo4
{
    private string strValue;
    [AutoConstructorIgnore]
    private int intValue;
}
```
上述代码生成的构造函数就会忽略```intValue```字段的赋值。
```csharp
partial class Demo4
{
    public Demo4(string strValue)
    {
        this.strValue = strValue;
    }
}
```

### 初始化函数

允许我们为每个类定义一个无参的初始化函数，我们需要在这个函数上加上```[AutoConstructorInitialize]```特性，那么生成的构造函数会自动帮我们去调用这个初始化函数。
```csharp
例如以下代码
[AutoConstructor]
public partial class Demo5
{
    private string strValue;
    private int intValue;

    [AutoConstructorInitialize]
    private void MyInitLogic()
    {
        Console.WriteLine("instance is creating.");
    }
}
```
生成的构造函数如下
```csharp
partial class Demo5
{
    public Demo5(string strValue, int intValue)
    {
        this.strValue = strValue;
        this.intValue = intValue;
        this.MyInitLogic();
    }
}
```
一般情况下，```[AutoConstructorInitialize]```都会结合```[AutoConstructorIgnore]```，完成自定义的初始化逻辑。

### 依赖注入

在使用依赖注入的场景，使用```AutoConstructor```可以帮助我们减少写构造函数的麻烦。例如以下的代码，如果可以从```Dependency Injection```容器中获取到```IService1```和```IService2```的实例的话，那么从容器中获取```Demo6Service```则可以正常工作。

```csharp
public interface IService1
{
    void Action1();
}
public interface IService2
{
    void Action2();
}
[AutoConstructor]
public partial class Demo6Service
{ 
    private readonly IService1 service1;
    private readonly IService2 service2;
    public void Action()
    {
        service1.Action1();
        service2.Action2();
    }
}
```


### 继承

```[AutoConstructor]```可以继承父类的构造函数，例如下面的代码

```csharp
[AutoConstructor]
public partial class Demo7Parent
{
    private string strValue;
}
[AutoConstructor]
public partial class Demo7Child: Demo7Parent
{
    private int intValue;
}
```
自动生成的代码如下
```csharp
partial class Demo7Parent
{
    public Demo7Parent(string strValue)
    {
        this.strValue = strValue;
    }
}
partial class Demo7Child
{
    public Demo7Child(string strValue, int intValue)
        : base(strValue: strValue)
    {
        this.intValue = intValue;
    }
}
```

## 3. Mapper

### Convert

### Update

### Query

## 4. AutoNotify

## 5. CodeException