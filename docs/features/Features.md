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

### 入门示例
假如我们有两个User的类，结构相似，我们需要把其中的一个对象转换成另一个对象。
```csharp
public record UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
public record UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
```
只需要在需要转换的类上面定义```[Mapper(typeof(UserEntity), typeof(UserDto))]```，当定义了以上Mapper特性后，```SourceGenerator```会自动给```UserEntity```增加一个```To```的泛型方法。使用这个泛型方法我们可以把```UserEntity```转换为```UserDto``` 对象。
```csharp
[Mapper(typeof(UserEntity), typeof(UserDto))]
internal class Program
{
    static void Main(string[] args)
    {
        var user = new UserEntity { Age = 10, Name = "zhangsan", Id = Guid.NewGuid() };
        var userDto = user.To<UserDto>();
        Console.WriteLine(userDto);
    }
}
```
以上代码生成的核心代码如下
```csharp
private static global::Mapper.Demo.UserDto ToMapper_Demo_UserDto(this global::Mapper.Demo.UserEntity source)
{
    if (source == null) return default;
    return new global::Mapper.Demo.UserDto
    {
        Id = source.Id,
        Name = source.Name,
        Age = source.Age,
    };
}
public static T To<T>(this global::Mapper.Demo.UserEntity source) where T : new()
{
    if (source == null) return default;
    if (typeof(T) == typeof(global::Mapper.Demo.UserDto))
    {
        return (T)(object)ToMapper_Demo_UserDto(source);
    }
    throw new NotSupportedException($"Can not convert '{typeof(global::Mapper.Demo.UserEntity)}' to '{typeof(T)}'.");
}
```


### 单个对象存在嵌套对象

```csharp
[Mapper(typeof(UserEntity), typeof(UserDto))]
internal class Program
{
    static void Main(string[] args)
    {
        var user = new UserEntity { Age = 10, Name = "zhangsan", City = new CityEntity { Name = "Xi'an", Province = "Shannxi" };
        var userDto = user.To<UserDto>();
        Console.WriteLine(userDto);
    }
}
public record UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public CityEntity City { get; set; }
}
public record CityEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Province { get; set; }
}
public record UserDto
{
    public string Name { get; set; }
    public int Age { get; set; }
    public CityDto City { get; set; }
}
public record CityDto
{
    public string Name { get; set; }
    public string Province { get; set; }
}
```
上面的代码运行输出如下，可以看见```City```属性也被正确的填充了，因为我们定义了```[Mapper(typeof(UserEntity), typeof(UserDto))]```，这样会自动尝试帮我们映射```City```属性，即使两个的类型不同。

```
UserDto { Name = zhangsan, Age = 10, City = CityDto { Name = xi'an, Province = shannxi } }
```
生成的核心代码如下
```csharp
private static global::Mapper.Demo.UserDto ToMapper_Demo_UserDto(this global::Mapper.Demo.UserEntity source)
{
    if (source == null) return default;
    return new global::Mapper.Demo.UserDto
    {
        Name = source.Name,
        Age = source.Age,
        City = source.City == null ? default : new global::Mapper.Demo.CityDto
        {
            Name = source.City.Name,
            Province = source.City.Province,
        },
    };
}
```

### 导航属性

有时候，目标对象会展示源对象的一些属性，例如以下的例子
```csharp
[Mapper(typeof(UserEntity), typeof(UserDto))]
internal class Program
{
    static void Main(string[] args)
    {
        var user = new UserEntity { Age = 10, Name = "zhangsan", City = new CityEntity { Name = "xi'an", Province = "shannxi" } };
        var userDto = user.To<UserDto>();
        Console.WriteLine(userDto);
    }
}
public record UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public CityEntity City { get; set; }
}
public record CityEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Province { get; set; }
}
public record UserDto
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Guid CityId { get; set; }
    public string CityName { get; set; }
    public string CityProvince { get; set; }
}
```
生成的核心代码如下
```csharp
 private static global::Mapper.Demo.UserDto ToMapper_Demo_UserDto(this global::Mapper.Demo.UserEntity source)
{
    if (source == null) return default;
    return new global::Mapper.Demo.UserDto
    {
        Name = source.Name,
        Age = source.Age,
        CityId = source.City.Id,
        CityName = source.City.Name,
        CityProvince = source.City.Province,
    };
}
```

### 单个对象存在嵌套集合对象

```csharp
[Mapper(typeof(UserEntity), typeof(UserDto))]
internal class Program
{
    static void Main(string[] args)
    {
        var user = new UserEntity { Age = 10, Name = "zhangsan", Cities =  new List<CityEntity> { new CityEntity { Name = "xi'an", Province = "shannxi" } } };
        var userDto = user.To<UserDto>();
        Console.WriteLine(userDto);
    }
}
public record UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public List<CityEntity> Cities { get; set; }
}
public record CityEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Province { get; set; }
}
public record UserDto
{
    public string Name { get; set; }
    public int Age { get; set; }
    public CityDto[] Cities { get; set; }
}
public record CityDto
{
    public string Name { get; set; }
    public string Province { get; set; }
}

```
生成的核心代码如下
```csharp
private static global::Mapper.Demo.UserDto ToMapper_Demo_UserDto(this global::Mapper.Demo.UserEntity source)
{
    if (source == null) return default;
    return new global::Mapper.Demo.UserDto
    {
        Name = source.Name,
        Age = source.Age,
        Cities = source.Cities == null ? default : source.Cities.Select(p => p == null ? default(global::Mapper.Demo.CityDto) : new global::Mapper.Demo.CityDto
        {
            Name = p.Name,
            Province = p.Province,
        }).ToArray(),
    };
}
```



### 转换集合对象
也可以一次性转换集合对象，例如下面的例子
```csharp
[Mapper(typeof(UserEntity), typeof(UserDto))]
internal class Program2
{
    static void Main(string[] args)
    {
        var userList = new List<UserEntity>
        {
                new UserEntity { Age = 10, Name = "zhangsan", Id = Guid.NewGuid() }
        };
        var userDtoArray = userList.To<UserDto>().ToArray();
        Console.WriteLine(userDtoArray);
    }
}
public record UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
public record UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
```
核心代码如下
```csharp
public static IEnumerable<T> To<T>(this IEnumerable<global::Mapper.Demo.UserEntity> source) where T : new()
{
    if (typeof(T) == typeof(global::Mapper.Demo.UserDto))
    {
        return (IEnumerable<T>)source?.Select(p => p.ToMapper_Demo_UserDto());
    }
    throw new NotSupportedException($"Can not convert '{typeof(global::Mapper.Demo.UserEntity)}' to '{typeof(T)}'.");
}
```

### IQueryable的转换

### 更新对象

### 更新字典
### 更新集合对象，CollectionUpdateMode

### 忽略字段，IgnoreProperties

### 自定义映射，CustomMappings

### 编译检查，CheckType


### 减少生成的代码，合理使用MapperType


## 4. AutoNotify

## 5. CodeException