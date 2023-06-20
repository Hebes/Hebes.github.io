# VisualStudio-添加自定义代码片段

![1](\../Image/VisualStudio-添加自定义代码片段/1.png)

![2](\../Image/VisualStudio-添加自定义代码片段/2.png)

保存的文件名称XXX.snippet

模板

```XMl
<?xml version="1.0" encoding="utf-8"?>
<CodeSnippets xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">
    <CodeSnippet Format="1.0.0">
        <Header>
            <Title>脚本提示</Title>
            <Shortcut>author</Shortcut>
            <Description>脚本提示</Description>
            <Author>Admin</Author>
            <SnippetTypes>
                <SnippetType>Expansion</SnippetType>
                <SnippetType>SurroundsWith</SnippetType>
            </SnippetTypes>
        </Header>
        <Snippet>
            <Declarations>
                <Literal>
                    <ID>expression</ID>
                    <ToolTip>日期类型</ToolTip>
                    <Function>SimpleTypeName(global::System.DateTime)</Function>
                </Literal>
                <Literal>
                    <ID>author</ID>
                    <ToolTip>作者</ToolTip>
                    <Default>Admin</Default>
                </Literal> 
                <Literal>
                    <ID>dt</ID>
                    <ToolTip>变量名</ToolTip>
                    <Default>dt</Default>
                </Literal> 
            </Declarations>
            <Code Language="csharp">
                <![CDATA[填写的内容$author$
                var $dt$ = $expression$.Now;$end$]]>
            </Code>
        </Snippet>
    </CodeSnippet>
</CodeSnippets>
```

**其他部分:**

Snippet ：指定代码片段的引用、导入、声明以及代码内容
Declarations：声明字面量或对象

**\<Header>部分:**

Header：用于指定有关 IntelliSense 代码段的常规信息

Title：代码段的友好名称

Shortcut：快捷输入文本，这里用的是 author

Description：描述。这会显示在Visual Studio的提示上

Author：作者

**\<Literal>部分:**

Literal：定义可以编辑的代码段的文本（字面量）

ID ：指定字面量的唯一标识符（这个元素是必需的）

Default ：指定插入代码段时字面量的默认值（这个元素是必需的）

Function ：元素 指定当文本在 Visual Studio 中获得焦点时要执行的函数

ToolTip ：元素 用于描述文本的预期值和用法

## 说明

**Code元素**，在这里定义代码片段中的代码内容。

Code元素有两个保留的关键字：$end$ 和 $selected$。

$end$ 标记在插入代码段之后用于放置光标的位置。

$selected$ 表示在文档中选择的要在调用时插入代码段的文本（如果定义了字面量，会直接选择字面量，当按下回车后，会跳到$selected$的位置）

**Code元素支持三种属性:**

Language：用于标识当前的代码片段用于哪种编程语言，可选项（VB、CSharp、CPP、XAML、XML、JavaScrip、TypeScript、SQL、HTML）（这个选项是必需的）

Kind：用于标识 代码片段可以用于哪个位置。（这个属性是可选的）

method body(用于方法内部)、method decl (用于方法定义)、type decl (用于类型定义)、file (完整的代码定义，可用于任何位置)、any(任何位置)

## 使用方法

1. 方法一

    使用代码片段完成方法快速注释
    使用方法：在vs中选择工具->代码片段管理器->导入
    author为快捷键设置

2. 方法二

    写完的XMl保存,直接复制进去,路径请参考开头的两个图片

## 参考网站

**[代码片段功能介绍](<https://www.cnblogs.com/zhaotianff/p/13370909.html>)**

**[VS 编码规范-代码注释设置](<https://www.cnblogs.com/kingkangstudy/p/10665234.html>)**
