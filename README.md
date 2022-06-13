# FlyTiger

FlyTiger is a dotnet source generator class library. It provide several useful features to make coding on c# more easier.

![build](https://github.com/yscorecore/FlyTiger/workflows/build/badge.svg)
[![codecov](https://codecov.io/gh/yscorecore/FlyTiger/branch/master/graph/badge.svg)](https://codecov.io/gh/yscorecore/FlyTiger) 
[![Nuget](https://img.shields.io/nuget/v/FlyTiger)](https://nuget.org/packages/FlyTiger/) 
[![GitHub](https://img.shields.io/github/license/yscorecore/FlyTiger)](https://github.com/yscorecore/FlyTiger/blob/master/LICENSE)

## How to use
Add `FlyTiger` package in your csharp project.
```bash
dotnet add package FlyTiger 
```


### AutoConstructor

`FlyTiger.AutoConstructorAttribute` will help us generate class constructor. 

- No Use FlyTiger
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
- Use FlyTiger
    ```csharp
    [AutoConstructor(NullCheck = true)]
    [ApiController]
    [Route("[controller]")]
    public partial class MyController : ControllerBase
    {
        private readonly IMyService myservice;
        private readonly ILogger<MyController> logger;

        [HttpPost]
        public async Task Run(string arg)
        {
            logger.LogInformation("run invoked");
            await myservice.Run(arg);
        }
    }
    ```



### AutoNotify

`FlyTiger.AutoNotifyAttribute` will help us generate property with changed event. 

- No Use FlyTiger
    ```csharp
    public class Service1: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _value;

        public int Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if(this._value != value)
                {{
                    this._value = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }}
            }
        }
    }
    ```
- Use FlyTiger
    ```csharp
    public partial class Service1
    {
        [AutoNotify]
        private int _value;
    }
    ```


### SingletonPattern

`FlyTiger.SingletonPatternAttribute` will help us define a singleton pattern.

- No Use FlyTiger
    ```csharp
    [SingletonPattern]
    public class Service1
    {
        private static readonly Lazy<Service1> LazyInstance = new Lazy<Service1>(() => new Service1(), true);

        public static Service1 Instance => LazyInstance.Value;

        private class Service1() 
        {
        }

        public void SayHello(string name)
        {
            Console.WriteLine($"Hello, {name}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Service1.Instance.SayHello("FlyTiger");
        }
    }
    ```
- Use FlyTiger
    ```csharp
    [SingletonPattern]
    public partial class Service1
    {
        public void SayHello(string name)
        {
            Console.WriteLine($"Hello, {name}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Service1.Instance.SayHello("FlyTiger");
        }
    }
    ```
### ConvertTo
`FlyTiger.SingletonPatternAttribute` will help us generate mapping code in different models.

- No Use FlyTiger
    ```csharp
    [AutoConstructor]
    class UserService
    {
        private readonly UserDbContext userContext;
        public List<UserDtos> ListAllUsers()
        {
            return userContexts.Users.Select(p => new UserDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Tel = p.Tel,
                    Roles = p.Roles.Select(r => new RoleDto {Id = r.Id, Name = r.Name}).ToList()
                }).ToList();
        }
    }
    ```
- Use FlyTiger
    ```csharp
    [AutoConstructor]
    class UserService
    {
        private readonly UserDbContext userContext;
        public List<UserDtos> ListAllUsers()
        {
            return userContexts.Users.ToUserDto().ToList();
        }
    }

    [ConvertTo(typeof(User), typeof(UserDto))]
    class Mappings
    {
    }
    ```