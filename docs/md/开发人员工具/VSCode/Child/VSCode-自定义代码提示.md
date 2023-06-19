# VSCode-自定义代码提示

## Markedwon启动智能提示

 输入setting

 选择 首选项：打开用户设置（json）

```text
"[markdown]": {
    "editor.wordWrap": "on",
    "editor.quickSuggestions": true
},
```

## 配置入口

点击 vscode 左下角 设置⚙️ 图标，点击用户代码片段

## 选择代码片段文件

点击 **用户代码片段** 后会展示所有语言的列表，博客使用 markdown 语法，这里选择 markdown.json ，点击进入 json 文件

## 正文

进入配置文件后，官方提供了一个模板 ，去掉注释后，可以看到代码长这样：

```c#
"Print to console": {
    "prefix": "log",
    "body": [
        "console.log('$1');",
        "$2"
    ],
    "description": "Log output to console"
}
```

案例图片

```c#
"image": {
  "prefix": "image",
  "body": [
   "![$1]($2)"
  ],
  "description": "图片的快捷格式"
 }
```

- *Print to console* 是代码段的名称，当没有设置 *description* 时，会作为代码提示的标题显示
- *prefix* 顾名思义就是触发代码提示的词头，在代码区输入设置好的词头，就会有代码提示
- *body* 里的就是代码段，支持多行显示和单行显示，字符串数组形式，每个元素代表一行，也可以用 \n 换行
- *description* 是代码的说明，配置后会替代代码段的名称显示

> body 里面可以使用特殊的结构来控制插入光标和文字：

- 使用 $ 加数字可以通过制表符控制光标移动，例如 $1 、 $2 、 $3...，数字越大越靠后，生成代码块后，光标会在数字最小的位置如 $1 开始，最小是 $1，$0 控制的是光标最后所在位置，官方给的例子中，光标会在 $1 位置，按下 tab 后，光标会跳到 $2 位置
- 可以在光标处添加预设的文本，例：${1:摘要}，生成代码块后，光标会生成并选中 摘要，可直接输入新的文本代替或者直接按 tab 保留并进入下一个位置，并且可以嵌套：${1:一级标题 ${2:二级标题}}
- 可以在光标处添加可选择的文本，语法是用逗号分隔的值枚举，例：${1|使用上下键选择分类,📒笔记,🔧工具|}

- 可以设置以下变量:

  - TM_SELECTED_TEXT 当前选择的文本或空字符串
  - TM_CURRENT_LINE 当前行的内容
  - TM_CURRENT_WORD 光标下的单词内容或空字符串
  - TM_LINE_INDEX 基于零索引的行号
  - TM_LINE_NUMBER 基于一索引的行号
  - TM_FILENAME_BASE 当前文档的文件名，不带扩展名
  - TM_FILENAME 当前文档的文件名
  - TM_DIRECTORY 当前文档的目录
  - TM_FILEPATH 当前文档的完整文件路径
  - CLIPBOARD 剪贴板中的内容
  - WORKSPACE_NAME 打开的工作空间或文件夹的名称

- 当前日期和时间

  - CURRENT_YEAR 本年度
  - CURRENT_YEAR_SHORT 本年度的最后两位数字
  - CURRENT_MONTH 以两位数字表示的月份（例如“02”）
  - CURRENT_MONTH_NAME 月的全名（例如“十月”）
  - CURRENT_MONTH_NAME_SHORT 该月的简称（例如“10月”）
  - CURRENT_DATE 一个月中的某天，今天
  - CURRENT_DAY_NAME 一天的名称（例如“周一”）
  - CURRENT_DAY_NAME_SHORT 当前小时（24小时制）
  - CURRENT_MINUTE 当前分钟
  - CURRENT_SECOND 当前秒
  - CURRENT_SECONDS_UNIX 自Unix时代以来的秒数

- 插入行或块注释

  - BLOCK_COMMENT_START 输出示例：用PHP /* 或 HTML <!--
  - BLOCK_COMMENT_END 输出示例：用PHP */ 或 HTML -->
  - LINE_COMMENT 输出示例：用PHP //

## 图例配置

```c#
markdown.json
"博客配置": {
    "prefix": "setting",
    "body": [
        "---",
        "title: ${1:标题}",
        "date: $CURRENT_YEAR-$CURRENT_MONTH-$CURRENT_DATE",
        "categories:",
        " - ${2|使用上下键选择分类,📒笔记,🔧工具|}",
        "tags:",
        " - ${3|使用上下键选择标签,vscode,JS,css,html,Vue,uniapp,微信小程序,React,TypeScript|}",
        "---",
        "",
        ":::tip",
        "${4:输入摘要}",
        ":::",
        "",
        "<!-- more -->",
        "",
        "$5"
    ],
    "description": "博客配置"
}
```

win 下遇到的问题
markdown文件在 windows 系统下貌似不会有代码提示，需要通过快捷键(默认 ctrl+i )触发

**[官方文档传送门👈](<https://code.visualstudio.com/docs/editor/userdefinedsnippets#_create-your-own-snippets>)**

## 参考网站

**[vscode自定义代码片段（代码提示）](<https://blog.csdn.net/cainiaoyihao_/article/details/115492570>)**

**[[vscode自定义代码片段（代码提示）](https://blog.csdn.net/Silvester123/article/details/90376904)**

**[[VSCode中markdown开启智能提示](https://blog.csdn.net/weixin_38680881/article/details/118165041)**
