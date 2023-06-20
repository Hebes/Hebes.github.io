<!-- <h1 style="text-align:center">MD文档</h1> -->
# MD文档

**参考来源:**

**[Markdown 交叉引用](<https://blog.csdn.net/qq_20515461/article/details/106809249>)**

**[markdown文字居中以及尺寸颜色设置（二）](<https://blog.csdn.net/abcdef314159/article/details/117886276>)**

**[在 VSCode 下用 Markdown Preview Enhanced 愉快地写文档](<https://zhuanlan.zhihu.com/p/56699805>)**

**[VScode写Markdown能否做到像Typora一样的即时渲染？](<https://www.zhihu.com/question/423855665>)**

## 快捷键加粗

Ctrl + B
斜体 Ctrl + I
引用 Ctrl + Q

## MD文档样式

### 引用

>这段就是引用。

### 列表

1. 加上数字编号就能做出有序列表
2. 有序列表也不需要前面的数字完全按照顺序排列
3. 这样更方便我们随便改变顺序对不对

* 无序列表的符号除了\*之外
* 用+/-也是可以的
* 但混用不太好吧，还是统一比较好
  * 我是习惯于用\*的
  * 嗯，手动的空格缩进就能带来列表缩进

### 折叠

<!-- #region -->
折叠的内容
<!-- #endregion -->

### 代码或者纯文本

---
    这段会被认为是代码，或者纯文本。反正是不用担
    心会被格式处理的。
    print 3*5*2
---

### 一个表格的例子

| 左对齐 | 居中  | 右对齐 |
| :----- | :---: | -----: |
| 2      |   3   |      5 |
| 10     |  100  |   1000 |

### 其次是批注

MPE 支持用 ==批注== 来高亮一段文字

预计这个项目将由一个由1名策划
{>>他非常睿智，但是我们不要告诉
他! <<}、{~~6~>3~~
}名程序、2名
美术{++、1名测试++ }组成的小组，
{==耗时1年==}{>>我们知道这不可
能但甲方说了! <<}完成。

### 单元格合并

| aa  | bb  | cc  |
| --- | --- | --- |
| 1   |     | 2   |
| 3   | >   |
| 4   |
| 5   | 6   |     |
| ^   | 7   | 8   |

### Emoji

L ^A^T~E~X :smile:

### LaTeX 写数学公式

$$\int_ {-\infty}^{\infty} e^
{-x^2} =
\sqrt{ \pi}$$

### 在文档里跑代码

被三个单撇号（键盘左上角那个键）包裹的文本块会被认为是代码，这和 Github 的习惯是一致的。（注意！这和 Markdown 原始的规则是不同的。）在单撇号后面加上语言的名字就可以享受到对应语言的语法高亮。

### 引用其他文档

@import "../Docsify搭建/Docsify搭建.md"

## 文字居中展示

``` Text {.line-numbers}
可以使用center标签，或者使用div标签，或者使用p标签，或者h标签都是可以的

例如
<center>数据结构和算法是居中展示，使用center标签</center>
<div align=center>数据结构和算法是居中展示，使用div标签</div>
<p align="center">数据结构和算法是居中展示，使用p标签</p>
<h5 style="text-align:center">数据结构和算法是居中展示，使用h标签</h5>
```

**样式展示**
<center>数据结构和算法是居中展示，使用center标签</center>
<div align=center>数据结构和算法是居中展示，使用div标签</div>
<p align="center">数据结构和算法是居中展示，使用p标签</p>
<h6 style="text-align:center">数据结构和算法是居中展示，使用h标签</h6>

## 文字居右展示

``` markdown {.line-numbers}
可以使用center标签，或者使用div标签，或者使用p标签，或者h标签都是可以的

例如
可以使用div标签，或者使用p标签，或者h标签都是可以的

例如
<div align=right>数据结构和算法是居右展示，使用div标签</div>
<p align="right">数据结构和算法是居右展示，使用p标签</p>
<h5 style="text-align:right">数据结构和算法是居右展示，使用h标签</h5>
```

**样式展示**
<div align=right>数据结构和算法是居右展示，使用div标签</div>
<p align="right">数据结构和算法是居右展示，使用p标签</p>
<h6 style="text-align:right">数据结构和算法是居右展示，使用h标签</h6>

## 文字字体，颜色和尺寸

``` markdown {.line-numbers}
使用font标签，字体使用face，颜色使用color，尺寸使用size。
颜色可以使用字母比如red，black，blue，yellow等，也可以是十六进制表示比如#0000ff或者#F025AB等等
size 是从1到7，数字越小字体越小，浏览器默认是3
这几个属性可以都设置，也可以只设置其中的1到2个

例如
<font face="黑体">我是黑体字体</font>
<font face="微软雅黑">我是微软雅黑字体</font>
<font face="STCAIYUN">我是华文彩字体云</font>
<font color=red size=3 face="黑体">我是红色，黑色字体，大小是3</font>
<font color=#F025AB size=5>我的颜色是#F025AB，大小是5</font>
```

**样式展示**
<font face="黑体">我是黑体字体</font>
<font face="微软雅黑">我是微软雅黑字体</font>
<font face="STCAIYUN">我是华文彩字体云</font>
<font color=red size=3 face="黑体">我是红色，黑色字体，大小是3</font>
<font color=#F025AB size=5>我的颜色是#F025AB，大小是5</font>

## 文字支持的字体

``` markdown {.line-numbers}
<font face="等线">我是等线</font>
<font face="方正粗黑宋简体">我是方正粗黑宋简体</font>
<font face="方正舒体">我是方正舒体</font>
<font face="方正姚体">我是方正姚体</font>
<font face="仿宋">我是仿宋</font>
<font face="黑体">我是黑体</font>
<font face="华文彩云">我是华文彩云</font>
<font face="华文仿宋">我是华文仿宋</font>
<font face="华文琥珀">我是华文琥珀</font>
<font face="华文楷体">我是华文楷体</font>
<font face="华文隶书">我是华文隶书</font>
<font face="华文宋体">我是华文宋体</font>
<font face="华文细黑">我是华文细黑</font>
<font face="华文新魏">我是华文新魏</font>
<font face="华文行楷">我是华文行楷</font>
<font face="华文中宋">我是华文中宋</font>
<font face="楷体">我是楷体</font>
<font face="隶书">我是隶书</font>
<font face="宋体">我是宋体</font>
<font face="微软雅黑">我是微软雅黑</font>
<font face="小米兰亭">我是小米兰亭</font>
<font face="新宋体">我是新宋体</font>
<font face="幼圆">我是幼圆</font>
```

**样式展示**
<font face="等线">我是等线</font>
<font face="方正粗黑宋简体">我是方正粗黑宋简体</font>
<font face="方正舒体">我是方正舒体</font>
<font face="方正姚体">我是方正姚体</font>
<font face="仿宋">我是仿宋</font>
<font face="黑体">我是黑体</font>
<font face="华文彩云">我是华文彩云</font>
<font face="华文仿宋">我是华文仿宋</font>
<font face="华文琥珀">我是华文琥珀</font>
<font face="华文楷体">我是华文楷体</font>
<font face="华文隶书">我是华文隶书</font>
<font face="华文宋体">我是华文宋体</font>
<font face="华文细黑">我是华文细黑</font>
<font face="华文新魏">我是华文新魏</font>
<font face="华文行楷">我是华文行楷</font>
<font face="华文中宋">我是华文中宋</font>
<font face="楷体">我是楷体</font>
<font face="隶书">我是隶书</font>
<font face="宋体">我是宋体</font>
<font face="微软雅黑">我是微软雅黑</font>
<font face="小米兰亭">我是小米兰亭</font>
<font face="新宋体">我是新宋体</font>
<font face="幼圆">我是幼圆</font>

## 文字常用的颜色

<h6 style="text-align:center"><table><tbody><tr><th>颜色名</th><th>十六进制颜色值</th><th>rgb颜色值</th></tr><tr><td bgcolor="#F0F8FF">AliceBlue</td><td>#F0F8FF</td><td>rgb(240, 248, 255)</td></tr><tr><td bgcolor="#FAEBD7">AntiqueWhite</td><td>#FAEBD7</td><td>rgb(250, 235, 215)</td></tr><tr><td bgcolor="#00FFFF">Aqua</td><td>#00FFFF</td><td>rgb(0, 255, 255)</td></tr><tr><td bgcolor="#7FFFD4">Aquamarine</td><td>#7FFFD4</td><td>rgb(127, 255, 212)</td></tr><tr><td bgcolor="#F0FFFF">Azure</td><td>#F0FFFF</td><td>rgb(240, 255, 255)</td></tr><tr><td bgcolor="#F5F5DC">Beige</td><td>#F5F5DC</td><td>rgb(245, 245, 220)</td></tr><tr><td bgcolor="#FFE4C4">Bisque</td><td>#FFE4C4</td><td>rgb(255, 228, 196)</td></tr><tr><td bgcolor="#000000">Black</td><td>#000000</td><td>rgb(0, 0, 0)</td></tr><tr><td bgcolor="#FFEBCD">BlanchedAlmond</td><td>#FFEBCD</td><td>rgb(255, 235, 205)</td></tr><tr><td bgcolor="#0000FF">Blue</td><td>#0000FF</td><td>rgb(0, 0, 255)</td></tr><tr><td bgcolor="#8A2BE2">BlueViolet</td><td>#8A2BE2</td><td>rgb(138, 43, 226)</td></tr><tr><td bgcolor="#A52A2A">Brown</td><td>#A52A2A</td><td>rgb(165, 42, 42)</td></tr><tr><td bgcolor="#DEB887">BurlyWood</td><td>#DEB887</td><td>rgb(222, 184, 135)</td></tr><tr><td bgcolor="#5F9EA0">CadetBlue</td><td>#5F9EA0</td><td>rgb(95, 158, 160)</td></tr><tr><td bgcolor="#7FFF00">Chartreuse</td><td>#7FFF00</td><td>rgb(127, 255, 0)</td></tr><tr><td bgcolor="#D2691E">Chocolate</td><td>#D2691E</td><td>rgb(210, 105, 30)</td></tr><tr><td bgcolor="#FF7F50">Coral</td><td>#FF7F50</td><td>rgb(255, 127, 80)</td></tr><tr><td bgcolor="#6495ED">CornflowerBlue</td><td>#6495ED</td><td>rgb(100, 149, 237)</td></tr><tr><td bgcolor="#FFF8DC">Cornsilk</td><td>#FFF8DC</td><td>rgb(255, 248, 220)</td></tr><tr><td bgcolor="#DC143C">Crimson</td><td>#DC143C</td><td>rgb(220, 20, 60)</td></tr><tr><td bgcolor="#00FFFF">Cyan</td><td>#00FFFF</td><td>rgb(0, 255, 255)</td></tr><tr><td bgcolor="#00008B">DarkBlue</td><td>#00008B</td><td>rgb(0, 0, 139)</td></tr><tr><td bgcolor="#008B8B">DarkCyan</td><td>#008B8B</td><td>rgb(0, 139, 139)</td></tr><tr><td bgcolor="#B8860B">DarkGoldenRod</td><td>#B8860B</td><td>rgb(184, 134, 11)</td></tr><tr><td bgcolor="#A9A9A9">DarkGray</td><td>#A9A9A9</td><td>rgb(169, 169, 169)</td></tr><tr><td bgcolor="#006400">DarkGreen</td><td>#006400</td><td>rgb(0, 100, 0)</td></tr><tr><td bgcolor="#BDB76B">DarkKhaki</td><td>#BDB76B</td><td>rgb(189, 183, 107)</td></tr><tr><td bgcolor="#8B008B">DarkMagenta</td><td>#8B008B</td><td>rgb(139, 0, 139)</td></tr><tr><td bgcolor="#556B2F">DarkOliveGreen</td><td>#556B2F</td><td>rgb(85, 107, 47)</td></tr><tr><td bgcolor="#FF8C00">Darkorange</td><td>#FF8C00</td><td>rgb(255, 140, 0)</td></tr><tr><td bgcolor="#9932CC">DarkOrchid</td><td>#9932CC</td><td>rgb(153, 50, 204)</td></tr><tr><td bgcolor="#8B0000">DarkRed</td><td>#8B0000</td><td>rgb(139, 0, 0)</td></tr><tr><td bgcolor="#E9967A">DarkSalmon</td><td>#E9967A</td><td>rgb(233, 150, 122)</td></tr><tr><td bgcolor="#8FBC8F">DarkSeaGreen</td><td>#8FBC8F</td><td>rgb(143, 188, 143)</td></tr><tr><td bgcolor="#483D8B">DarkSlateBlue</td><td>#483D8B</td><td>rgb(72, 61, 139)</td></tr><tr><td bgcolor="#2F4F4F">DarkSlateGray</td><td>#2F4F4F</td><td>rgb(47, 79, 79)</td></tr><tr><td bgcolor="#00CED1">DarkTurquoise</td><td>#00CED1</td><td>rgb(0, 206, 209)</td></tr><tr><td bgcolor="#9400D3">DarkViolet</td><td>#9400D3</td><td>rgb(148, 0, 211)</td></tr><tr><td bgcolor="#FF1493">DeepPink</td><td>#FF1493</td><td>rgb(255, 20, 147)</td></tr><tr><td bgcolor="#00BFFF">DeepSkyBlue</td><td>#00BFFF</td><td>rgb(0, 191, 255)</td></tr><tr><td bgcolor="#696969">DimGray</td><td>#696969</td><td>rgb(105, 105, 105)</td></tr><tr><td bgcolor="#1E90FF">DodgerBlue</td><td>#1E90FF</td><td>rgb(30, 144, 255)</td></tr><tr><td bgcolor="#D19275">Feldspar</td><td>#D19275</td><td>rgb(209, 146, 117)</td></tr><tr><td bgcolor="#B22222">FireBrick</td><td>#B22222</td><td>rgb(178, 34, 34)</td></tr><tr><td bgcolor="#FFFAF0">FloralWhite</td><td>#FFFAF0</td><td>rgb(255, 250, 240)</td></tr><tr><td bgcolor="#228B22">ForestGreen</td><td>#228B22</td><td>rgb(34, 139, 34)</td></tr><tr><td bgcolor="#FF00FF">Fuchsia</td><td>#FF00FF</td><td>rgb(255, 0, 255)</td></tr><tr><td bgcolor="#DCDCDC">Gainsboro</td><td>#DCDCDC</td><td>rgb(220, 220, 220)</td></tr><tr><td bgcolor="#F8F8FF">GhostWhite</td><td>#F8F8FF</td><td>rgb(248, 248, 255)</td></tr><tr><td bgcolor="#FFD700">Gold</td><td>#FFD700</td><td>rgb(255, 215, 0)</td></tr><tr><td bgcolor="#DAA520">GoldenRod</td><td>#DAA520</td><td>rgb(218, 165, 32)</td></tr><tr><td bgcolor="#808080">Gray</td><td>#808080</td><td>rgb(128, 128, 128)</td></tr><tr><td bgcolor="#008000">Green</td><td>#008000</td><td>rgb(0, 128, 0)</td></tr><tr><td bgcolor="#ADFF2F">GreenYellow</td><td>#ADFF2F</td><td>rgb(173, 255, 47)</td></tr><tr><td bgcolor="#F0FFF0">HoneyDew</td><td>#F0FFF0</td><td>rgb(240, 255, 240)</td></tr><tr><td bgcolor="#FF69B4">HotPink</td><td>#FF69B4</td><td>rgb(255, 105, 180)</td></tr><tr><td bgcolor="#CD5C5C">IndianRed</td><td>#CD5C5C</td><td>rgb(205, 92, 92)</td></tr><tr><td bgcolor="#4B0082">Indigo</td><td>#4B0082</td><td>rgb(75, 0, 130)</td></tr><tr><td bgcolor="#FFFFF0">Ivory</td><td>#FFFFF0</td><td>rgb(255, 255, 240)</td></tr><tr><td bgcolor="#F0E68C">Khaki</td><td>#F0E68C</td><td>rgb(240, 230, 140)</td></tr><tr><td bgcolor="#E6E6FA">Lavender</td><td>#E6E6FA</td><td>rgb(230, 230, 250)</td></tr><tr><td bgcolor="#FFF0F5">LavenderBlush</td><td>#FFF0F5</td><td>rgb(255, 240, 245)</td></tr><tr><td bgcolor="#7CFC00">LawnGreen</td><td>#7CFC00</td><td>rgb(124, 252, 0)</td></tr><tr><td bgcolor="#FFFACD">LemonChiffon</td><td>#FFFACD</td><td>rgb(255, 250, 205)</td></tr><tr><td bgcolor="#ADD8E6">LightBlue</td><td>#ADD8E6</td><td>rgb(173, 216, 230)</td></tr><tr><td bgcolor="#F08080">LightCoral</td><td>#F08080</td><td>rgb(240, 128, 128)</td></tr><tr><td bgcolor="#E0FFFF">LightCyan</td><td>#E0FFFF</td><td>rgb(224, 255, 255)</td></tr><tr><td bgcolor="#FAFAD2">LightGoldenRodYellow</td><td>#FAFAD2</td><td>rgb(250, 250, 210)</td></tr><tr><td bgcolor="#D3D3D3">LightGrey</td><td>#D3D3D3</td><td>rgb(211, 211, 211)</td></tr><tr><td bgcolor="#90EE90">LightGreen</td><td>#90EE90</td><td>rgb(144, 238, 144)</td></tr><tr><td bgcolor="#FFB6C1">LightPink</td><td>#FFB6C1</td><td>rgb(255, 182, 193)</td></tr><tr><td bgcolor="#FFA07A">LightSalmon</td><td>#FFA07A</td><td>rgb(255, 160, 122)</td></tr><tr><td bgcolor="#20B2AA">LightSeaGreen</td><td>#20B2AA</td><td>rgb(32, 178, 170)</td></tr><tr><td bgcolor="#87CEFA">LightSkyBlue</td><td>#87CEFA</td><td>rgb(135, 206, 250)</td></tr><tr><td bgcolor="#8470FF">LightSlateBlue</td><td>#8470FF</td><td>rgb(132, 112, 255)</td></tr><tr><td bgcolor="#778899">LightSlateGray</td><td>#778899</td><td>rgb(119, 136, 153)</td></tr><tr><td bgcolor="#B0C4DE">LightSteelBlue</td><td>#B0C4DE</td><td>rgb(176, 196, 222)</td></tr><tr><td bgcolor="#FFFFE0">LightYellow</td><td>#FFFFE0</td><td>rgb(255, 255, 224)</td></tr><tr><td bgcolor="#00FF00">Lime</td><td>#00FF00</td><td>rgb(0, 255, 0)</td></tr><tr><td bgcolor="#32CD32">LimeGreen</td><td>#32CD32</td><td>rgb(50, 205, 50)</td></tr><tr><td bgcolor="#FAF0E6">Linen</td><td>#FAF0E6</td><td>rgb(250, 240, 230)</td></tr><tr><td bgcolor="#FF00FF">Magenta</td><td>#FF00FF</td><td>rgb(255, 0, 255)</td></tr><tr><td bgcolor="#800000">Maroon</td><td>#800000</td><td>rgb(128, 0, 0)</td></tr><tr><td bgcolor="#66CDAA">MediumAquaMarine</td><td>#66CDAA</td><td>rgb(102, 205, 170)</td></tr><tr><td bgcolor="#0000CD">MediumBlue</td><td>#0000CD</td><td>rgb(0, 0, 205)</td></tr><tr><td bgcolor="#BA55D3">MediumOrchid</td><td>#BA55D3</td><td>rgb(186, 85, 211)</td></tr><tr><td bgcolor="#9370D8">MediumPurple</td><td>#9370D8</td><td>rgb(147, 112, 216)</td></tr><tr><td bgcolor="#3CB371">MediumSeaGreen</td><td>#3CB371</td><td>rgb(60, 179, 113)</td></tr><tr><td bgcolor="#7B68EE">MediumSlateBlue</td><td>#7B68EE</td><td>rgb(123, 104, 238)</td></tr><tr><td bgcolor="#00FA9A">MediumSpringGreen</td><td>#00FA9A</td><td>rgb(0, 250, 154)</td></tr><tr><td bgcolor="#48D1CC">MediumTurquoise</td><td>#48D1CC</td><td>rgb(72, 209, 204)</td></tr><tr><td bgcolor="#C71585">MediumVioletRed</td><td>#C71585</td><td>rgb(199, 21, 133)</td></tr><tr><td bgcolor="#191970">MidnightBlue</td><td>#191970</td><td>rgb(25, 25, 112)</td></tr><tr><td bgcolor="#F5FFFA">MintCream</td><td>#F5FFFA</td><td>rgb(245, 255, 250)</td></tr><tr><td bgcolor="#FFE4E1">MistyRose</td><td>#FFE4E1</td><td>rgb(255, 228, 225)</td></tr><tr><td bgcolor="#FFE4B5">Moccasin</td><td>#FFE4B5</td><td>rgb(255, 228, 181)</td></tr><tr><td bgcolor="#FFDEAD">NavajoWhite</td><td>#FFDEAD</td><td>rgb(255, 222, 173)</td></tr><tr><td bgcolor="#000080">Navy</td><td>#000080</td><td>rgb(0, 0, 128)</td></tr><tr><td bgcolor="#FDF5E6">OldLace</td><td>#FDF5E6</td><td>rgb(253, 245, 230)</td></tr><tr><td bgcolor="#808000">Olive</td><td>#808000</td><td>rgb(128, 128, 0)</td></tr><tr><td bgcolor="#6B8E23">OliveDrab</td><td>#6B8E23</td><td>rgb(107, 142, 35)</td></tr><tr><td bgcolor="#FFA500">Orange</td><td>#FFA500</td><td>rgb(255, 165, 0)</td></tr><tr><td bgcolor="#FF4500">OrangeRed</td><td>#FF4500</td><td>rgb(255, 69, 0)</td></tr><tr><td bgcolor="#DA70D6">Orchid</td><td>#DA70D6</td><td>rgb(218, 112, 214)</td></tr><tr><td bgcolor="#EEE8AA">PaleGoldenRod</td><td>#EEE8AA</td><td>rgb(238, 232, 170)</td></tr><tr><td bgcolor="#98FB98">PaleGreen</td><td>#98FB98</td><td>rgb(152, 251, 152)</td></tr><tr><td bgcolor="#AFEEEE">PaleTurquoise</td><td>#AFEEEE</td><td>rgb(175, 238, 238)</td></tr><tr><td bgcolor="#D87093">PaleVioletRed</td><td>#D87093</td><td>rgb(216, 112, 147)</td></tr><tr><td bgcolor="#FFEFD5">PapayaWhip</td><td>#FFEFD5</td><td>rgb(255, 239, 213)</td></tr><tr><td bgcolor="#FFDAB9">PeachPuff</td><td>#FFDAB9</td><td>rgb(255, 218, 185)</td></tr><tr><td bgcolor="#CD853F">Peru</td><td>#CD853F</td><td>rgb(205, 133, 63)</td></tr><tr><td bgcolor="#FFC0CB">Pink</td><td>#FFC0CB</td><td>rgb(255, 192, 203)</td></tr><tr><td bgcolor="#DDA0DD">Plum</td><td>#DDA0DD</td><td>rgb(221, 160, 221)</td></tr><tr><td bgcolor="#B0E0E6">PowderBlue</td><td>#B0E0E6</td><td>rgb(176, 224, 230)</td></tr><tr><td bgcolor="#800080">Purple</td><td>#800080</td><td>rgb(128, 0, 128)</td></tr><tr><td bgcolor="#FF0000">Red</td><td>#FF0000</td><td>rgb(255, 0, 0)</td></tr><tr><td bgcolor="#BC8F8F">RosyBrown</td><td>#BC8F8F</td><td>rgb(188, 143, 143)</td></tr><tr><td bgcolor="#4169E1">RoyalBlue</td><td>#4169E1</td><td>rgb(65, 105, 225)</td></tr><tr><td bgcolor="#8B4513">SaddleBrown</td><td>#8B4513</td><td>rgb(139, 69, 19)</td></tr><tr><td bgcolor="#FA8072">Salmon</td><td>#FA8072</td><td>rgb(250, 128, 114)</td></tr><tr><td bgcolor="#F4A460">SandyBrown</td><td>#F4A460</td><td>rgb(244, 164, 96)</td></tr><tr><td bgcolor="#2E8B57">SeaGreen</td><td>#2E8B57</td><td>rgb(46, 139, 87)</td></tr><tr><td bgcolor="#FFF5EE">SeaShell</td><td>#FFF5EE</td><td>rgb(255, 245, 238)</td></tr><tr><td bgcolor="#A0522D">Sienna</td><td>#A0522D</td><td>rgb(160, 82, 45)</td></tr><tr><td bgcolor="#C0C0C0">Silver</td><td>#C0C0C0</td><td>rgb(192, 192, 192)</td></tr><tr><td bgcolor="#87CEEB">SkyBlue</td><td>#87CEEB</td><td>rgb(135, 206, 235)</td></tr><tr><td bgcolor="#6A5ACD">SlateBlue</td><td>#6A5ACD</td><td>rgb(106, 90, 205)</td></tr><tr><td bgcolor="#708090">SlateGray</td><td>#708090</td><td>rgb(112, 128, 144)</td></tr><tr><td bgcolor="#FFFAFA">Snow</td><td>#FFFAFA</td><td>rgb(255, 250, 250)</td></tr><tr><td bgcolor="#00FF7F">SpringGreen</td><td>#00FF7F</td><td>rgb(0, 255, 127)</td></tr><tr><td bgcolor="#4682B4">SteelBlue</td><td>#4682B4</td><td>rgb(70, 130, 180)</td></tr><tr><td bgcolor="#D2B48C">Tan</td><td>#D2B48C</td><td>rgb(210, 180, 140)</td></tr><tr><td bgcolor="#008080">Teal</td><td>#008080</td><td>rgb(0, 128, 128)</td></tr><tr><td bgcolor="#D8BFD8">Thistle</td><td>#D8BFD8</td><td>rgb(216, 191, 216)</td></tr><tr><td bgcolor="#FF6347">Tomato</td><td>#FF6347</td><td>rgb(255, 99, 71)</td></tr><tr><td bgcolor="#40E0D0">Turquoise</td><td>#40E0D0</td><td>rgb(64, 224, 208)</td></tr><tr><td bgcolor="#EE82EE">Violet</td><td>#EE82EE</td><td>rgb(238, 130, 238)</td></tr><tr><td bgcolor="#D02090">VioletRed</td><td>#D02090</td><td>rgb(208, 32, 144)</td></tr><tr><td bgcolor="#F5DEB3">Wheat</td><td>#F5DEB3</td><td>rgb(245, 222, 179)</td></tr><tr><td bgcolor="#FFFFFF">White</td><td>#FFFFFF</td><td>rgb(255, 255, 255)</td></tr><tr><td bgcolor="#F5F5F5">WhiteSmoke</td><td>#F5F5F5</td><td>rgb(245, 245, 245)</td></tr><tr><td bgcolor="#FFFF00">Yellow</td><td>#FFFF00</td><td>rgb(255, 255, 0)</td></tr><tr><td bgcolor="#9ACD32">YellowGreen</td><td>#9ACD32</td><td>rgb(154, 205, 50)</td></tr></tbody></table></h6>

## 任务列表

- [x] 已经完成的事 1
- [x] 已经完成的事 2
- [x] 已经完成的事 3
- [ ] 仍未完成的事 4
- [ ] 仍未完成的事 5

## 分割线

---