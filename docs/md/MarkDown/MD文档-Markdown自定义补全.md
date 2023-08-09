# MD文档-Markdown自定义补全

VSCode 首选项->用户代码片段->MarkDown

![1](\../Image/MD文档-Markdown自定义补全/1.gif)

设置补全内容，如cpp代码块

```c#
"cpp": {
   "prefix": "cpp",  // 触发词
   "body": [  // 补全内容
      "```c++",
      "$1",  // 光标停留位置
      "```"
   ],
   "description": "Add C++ code block"  // 注释
}
```

文件->首选项->设置->(右上角)打开设置，将下面这段设置写入setting.json文件

```c#
"[markdown]":  {
      "editor.quickSuggestions": true
   }
```

![2](\../Image/MD文档-Markdown自定义补全/2.gif)

![3](\../Image/MD文档-Markdown自定义补全/3.gif)
