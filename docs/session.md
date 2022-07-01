---
presentation:
    theme: night.css
---

<!-- slide -->
#  FlyTiger

<img src="flytiger.jpeg"/>
<!-- slide -->
### FlyTiger是什么

> FlyTiger 是利用SourceGenerator技术，帮助c#的开发者生成常用代码的一个类库。


<!-- slide -->

### FlyTiger的特性

- 自动生成构造函数
- 快速写单例模式的代码
- 快速事件通知属性
- 最高性能的类型转换

<!-- slide -->

### 如何使用

> ```dotnet add package FlyTiger```

<!-- slide -->
### Demo 环节

<!-- slide -->
### Performance

|                     Method |         Mean |      Error |     StdDev |
|--------------------------- |-------------:|-----------:|-----------:|
|       MapSingleUseFlyTiger |     83.00 ns |   2.666 ns |   7.818 ns |
|     MapSingleUseAutoMapper |    133.49 ns |   2.381 ns |   2.111 ns |
|     Map10ObjectUseFlyTiger |    813.38 ns |   4.044 ns |   3.585 ns |
|   Map10ObjectUseAutoMapper |    791.13 ns |   5.720 ns |   5.350 ns |
|    Map100ObjectUseFlyTiger |  7,672.82 ns |  50.805 ns |  47.523 ns |
|  Map100ObjectUseAutoMapper |  5,918.86 ns |  34.354 ns |  30.454 ns |
|   Map1000ObjectUseFlyTiger | 81,942.20 ns | 854.432 ns | 757.432 ns |
| Map1000ObjectUseAutoMapper | 58,910.31 ns | 250.419 ns | 209.111 ns |
<!-- slide -->
### Q&A