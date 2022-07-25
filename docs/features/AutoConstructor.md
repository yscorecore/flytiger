---
layout: default
title: AutoConstructor
nav_order: 2
parent: FEATURES
---

## AutoConstructor

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