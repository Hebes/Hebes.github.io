# Markdownlint格式

## MD001

Heading levels should only increment by one level at a time
标题级数只能每次扩大一个，也就是说不能隔级创建标题，必须h1-h2-h3…这样

## MD002

First heading should be a top level heading
文档的第一个标题必须是最高级的标题，也就是h1

## MD003

Heading style
整篇文档的标题格式要统一

## MD004

Unordered list style
整篇文档的无序列表的格式要一致

## MD005

Inconsistent indentation for list items at the same level
同一个等级的列表的缩进要一致

## MD006

Consider starting bulleted lists at the beginning of the line
一级标题不能够缩进

## MD007

Unordered list indentation
无序列表嵌套的时候默认采取两个空格的缩进方式

## MD009

Trailing spaces
行尾最多可以添加两个空格，超出之后会有警告，最好每次都是两个空格因为两个空格刚好可以用来换行

## MD010

Hard tabs
不能使用tab来进行缩进，要使用空格

## MD011

Reversed link syntax
内联形式的链接和创建方式是否错误，中括号和圆括号是否使用正确

## MD012

Multiple consecutive blank lines
文档中不能有连续的空行（文档末可以有一个空行），在代码块中这个规则不会生效

## MD013

Line length
默认行的最大长度是80，对表格代码块标题都起效果

## MD014

Dollar signs used before commands without showing output
在代码块中，终端命令前面不需要有美元符号，如果如果代码块中既有终端命令，也有命令的输出，则终端命令前可以有美元符号($)

## MD018

No space after hash on atx style heading
标题格式如果是"atx"的话，#号和文字之间需要一个空格隔开

## MD019

Multiple spaces after hash on atx style heading
标题格式如果是"atx"的话，#号和文字之间只需要一个空格隔开，不需要多个

## MD020

No space inside hashes on closed atx style heading
在closed_atx格式的标题中，文字和前后的#号之间都需要一个空格隔开

## MD021

Multiple spaces inside hashes on closed atx style heading
在closed_atx格式的标题中，文字和前后的#号之间只需要一个空格隔开，不能有多余的

## MD022

Headings should be surrounded by blank lines
标题的上下行必须都是空格

## MD023

Headings must start at the beginning of the line
标题行不能缩进

## MD024

Multiple headings with the same content
在文档中不能有重复性的标题

## MD025

Multiple top level headings in the same document
同一个文档中，只能有一个最高级的标题，默认也只能有一个一级标题

## MD026

Trailing punctuation in heading
标题的末尾不能有". , ; : ! ? "这些符号

## MD027

Multiple spaces after blockquote symbol
在创建引用块的时候，右尖号与文字之间必须有且只有一个空格

## MD028

Blank line inside blockquote
两个引用区块间不能仅用一个空行隔开或者同一引用区块中不能有空行，如果一行中没有内容，则这一行要用>开头

## MD029

Ordered list item prefix
有序列表的前缀序号格式必须只用1或者从1开始的加1递增数字

## MD030

Spaces after list markers
列表（有序、无序）的前缀符号和文字之间用1个空格隔开，在列表嵌套或者同一列表项中有多个段落时，无序列表缩进两个空格，有序列表缩进3个空格

## MD031

Fenced code blocks should be surrounded by blank lines
单独的代码块前后需要用空行隔开（除非是在文档开头或末尾），否则有些解释器不会解释为代码块

## MD032

Lists should be surrounded by blank lines
列表（有序、无序）前后需要用空行隔开，否则有些解释器不会解释为列表，列表的缩进必须一致，否则会警告

## MD033

Inline HTML
文档中不允许使用html语句

## MD034

Bare URL used
单纯的链接地址需要用尖括号 (<>) 包裹，否则有些解释器不会解释为链接

## MD035

Horizontal rule style
创建水平线时整篇文档要统一，要和文档中第一次创建水平线使用的符号一致

## MD036

Emphasis used instead of a heading
不能用强调来代替标题 ****

## MD037

Spaces inside emphasis markers
强调的符号和文字之间不能有空格

## MD038

Spaces inside code span elements
当用单反引号创建代码段的时候，单反引号和它们之间的代码不能有空格，如果要把单反引号嵌入到代码段的首尾，创建代码段的单反引号和嵌入的单反引号间要有一个空格隔开

## MD039

Spaces inside link text
链接名和包围它的中括号之间不能有空格，但链接名中间可以有空格

## MD040

Fenced code blocks should have a language specified
单独的代码块（此处是指上下用三个反引号包围的代码块）应该指定代码块的编程语言，这一点有助于解释器对代码进行代码高亮

## MD041

First line in file should be a top level heading
文档的第一个非空行应该是文档最高级的标题，默认是1级标题

## MD042

No empty links
链接的地址不能为空

## MD043

Required heading structure
要求标题遵循一定的结构，默认是没有规定的结构

## MD044

Proper names should have the correct capitalization
指定一些名称，会检查它是否有正确的大写

## MD045

Images should have alternate text (alt text)
图片链接必须包含描述文本

## MD046

Code block style
整篇文档采用一致的代码格式

## MD047

Files should end with a single newline character
文档末尾需要一个空行结尾
