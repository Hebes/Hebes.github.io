# CSharp-nullable检查

在C#7(含7)之前 reference 变量可以为null , 但在C#8为了尽量避免 NullReferenceException 异常,引入了breaking change, 默认情况下reference 不可为null.

```C#
//C# 7
private string str=null ;

//C# 8
private string str; //不能为null 

//C# 8, 如果需要为空
private string? str=null ;
```

.Net6 生成的项目, 默认会启用 nullable 检查, 这样会大大避免 NullReferenceException 异常, 但这仅仅是个编译警告,
csproj 文件设置. 如果要上升到报错级别, 需要设置

```C#
<TreatWarningsAsErrors>true</TreatWarningsAsErrors>  
```

```C#
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
```

Nullable 的取值有 enable/disable/warnings/annotations 4种, 4种类型涉及两个维度, 警告上下文和注解上下文的开启,

enable: 意义很明确, 启用最严格的方式, .net 6 默认方式.  即编译器对表达式推断和声明不符合, 将会报编译错误.

disable: 老项目默认方式, 即编译器不对nullable做任何推断, 也不会有任何警告.

warnings: 即关闭注解上下文, 仅仅开启警告上下文, 简单理解: 编译器不对引用类型变量做推断,  仅对值类型做简单推断, 违反nullable规则, 仅仅报 warning.

annotations: 编译器对引用类型变量自行推断, 但不做警告.

## 参考

**[C# 项目的 nullable 检查](<https://www.cnblogs.com/harrychinese/p/nullable.html>)**
