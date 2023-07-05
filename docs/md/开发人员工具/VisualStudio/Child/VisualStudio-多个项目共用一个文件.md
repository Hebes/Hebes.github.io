# VisualStudio-多个项目共用一个文件

选择添加现有项时，那个添加按钮有个三角箭，下拉下去有一个 “添加为链接” 的按钮

在Visual Studio中，我们可以“添加为链接”以添加指向解决方案中另一个项目中的文件的链接 .

![1](https://img2022.cnblogs.com/blog/1573471/202203/1573471-20220330144722751-1369966531.png)

有没有办法对整个文件夹执行此操作，以便项目A中的整个文件夹在项目B中可见，而无需手动链接到该文件夹中的新项目？

下面就是解决办法：

在项目文件里添加如下代码，

Include后是共享代码的相对路径，\*代表获取所有的

Link后第一个参数是共享进来后创建一个共享文件夹

```c#
<ItemGroup\>
    <Compile Include\="..\\RoslynPad.Editor.Shared\\\*\*\\\*.cs"\>
      <Link\>Shared\\%(RecursivePath)%(Filename)%(Extension)</Link\>
    </Compile\>
  </ItemGroup\>
```

最终效果

![2](https://img2022.cnblogs.com/blog/1573471/202203/1573471-20220330144554953-1926413557.png)