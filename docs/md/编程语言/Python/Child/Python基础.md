# Python

Python 是一种解释型、面向对象、动态数据类型的高级程序设计语言。

Python 由 Guido van Rossum 于 1989 年底发明，第一个公开发行版发行于 1991 年。

## Python 变量与数据类型

**声明变量:**

Python 中的变量不需要声明，每个变量在使用前都必须赋值，变量赋值以后该变量才会被创建。在 Python 中，变量就是变量，它没有类型，我们所说的"类型"是变量所指的内存中对象的类型。

```python
name = "neo"
```

**变量赋值:**

在 Python 中，等号 = 是赋值语句，可以把任意数据类型赋值给变量，同一个变量可以反复赋值，而且可以是不同类型的变量。

```python
a = 123 # a 是整数
a = 'abc' # a 是字符串
```

```python
a = b = c = 1
```

```python
a, b, c = 1, 2, "neo"
```

**常量:**

所谓常量就是不能变的变量，比如常用的数学常数 π 就是一个常量。在 Python 中，通常用全部大写的变量名表示常量

```python
BI = 3.14
```

## 数据类型

```python
#!/usr/bin/python3
 
counter = 100          # 整型变量
miles   = 1000.0       # 浮点型变量
name    = "test"     # 字符串
 
print (counter)
print (miles)
print (name)
```

**数字类型转换:**

- int(x) 将x转换为一个整数。
- float(x) 将x转换到一个浮点数。
- complex(x) 将x转换到一个复数，实数部分为 x，虚数部分为 0。
- complex(x, y) 将 x 和 y 转换到一个复数，实数部分为 x，虚数部分为 y。x 和 y 是数字表达式。额外说明

**数值运算示例：**

```python
print (5 + 4)  # 加法   输出 9
print (4.3 - 2) # 减法   输出 2.3
print (3 * 7)  # 乘法  输出 21
print (2 / 4)  # 除法，得到一个浮点数    输出 0.5
print (2 // 4) # 除法，得到一个整数 输出 0
print (17 % 3) # 取余   输出 2
print (2 ** 5) # 乘方  输出 32
```

### String（字符串）

```python
s = '学习Python'
# 切片
s[0], s[-1], s[3:], s[::-1] # '优', 'n', 'ython', 'nohtyP的习学'
# 替换，还可以使用正则表达式替换
s.replace('Python', 'Java') # '学习Java'
# 查找，find()、index()、rfind()、rindex()
s.find('P')     # 3, 返回第一次出现的子串的下标
s.find('h', 2)      # 6, 设定下标2开始查找
s.find('23333') # -1, 查找不到返回-1
s.index('y')    # 4, 返回第一次出现的子串的下标
s.index('P')    # 不同与find(), 查找不到会抛出异常
# 转大小写, upper()、lower()、swapcase()、capitalize()、istitle()、isupper()、islower()
s.upper()       # '学习PYTHON'
s.swapcase()        # '学习pYTHON', 大小写互换
s.istitle()     # True
s.islower()     # False
# 去空格,strip()、lstrip()、rstrip()
# 格式化
s1 = '%s %s' % ('Windrivder', 21)   # 'Windrivder 21'
s2 = '{}, {}'.format(21, 'Windridver')  # 推荐使用format格式化字符串
s3 = '{0}, {1}, {0}'.format('Windrivder', 21)
s4 = '{name}: {age}'.format(age=21, name='Windrivder')
# 连接与分割，使用 + 连接字符串，每次操作会重新计算、开辟、释放内存，效率很低，所以推荐使用join
l = ['2017', '03', '29', '22:00']
s5 = '-'.join(l)           # '2017-03-29-22:00'
s6 = s5.split('-')         # ['2017', '03', '29', '22:00']
```

[判断字符串结尾](<https://blog.csdn.net/doiido/article/details/43418471>)

另外还有一点需要注意的是字符串编码，所有的 Python 字符串都是 Unicode 字符串，当需要将文件保存到外设或进行网络传输时，就要进行编码转换，将字符转换为字节，以提高效率。

```python
# encode 将字符转换为字节
str = '学习Python'
print (str.encode())        # 默认编码是 UTF-8  输出：b'\xe5\xad\xa6\xe4\xb9\xa0Python'
print (str.encode('gbk'))      # 输出  b'\xd1\xa7\xcf\xb0Python'
# decode 将字节转换为字符
print (str.encode().decode('utf8'))   # 输出 '学习Python'
print (str.encode('gbk').decode('gbk'))             # 输出 '学习Python'
```

### List（列表）

列表是写在方括号 [] 之间、用逗号分隔开的元素列表，列表可以完成大多数集合类的数据结构实现。列表中元素的类型可以不相同，它支持数字，字符串甚至可以包含列表（所谓嵌套），列表中的元素是可以改变。

示例：

```python
Weekday = ['Monday','Tuesday','Wednesday','Thursday','Friday']
print(Weekday[0])   # 输出 Monday

#list 搜索
print(Weekday.index("Wednesday"))

#list 增加元素
Weekday.append("new")
print(Weekday)

# list 删除
Weekday.remove("Thursday")
print(Weekday)
```

### Tuple（元组）

元组（tuple）与列表类似，不同之处在于元组的元素不能修改。元组写在小括号 () 里，元素之间用逗号隔开，组中的元素类型也可以不相同。

示例：

```python
letters = ('a','b','c','d','e','f','g')
print(letters[0])  # 输出 'a'
print(letters[0:3])  # 输出一组 ('a', 'b', 'c')
```

Sets（集合）

集合（set）是一个无序不重复元素的序列，使用大括号 {} 或者 set() 函数创建集合，注意：创建一个空集合必须用 set() 而不是 {} ，因为 {} 是用来创建一个空字典。

集合不能被切片也不能被索引，除了做集合运算之外，集合元素可以被添加还有删除：

示例：

```python
a_set = {1,2,3,4}
# 添加
a_set.add(5)
print(a_set)  # 输出{1, 2, 3, 4, 5}
# 删除
a_set.discard(5)
print(a_set)  # 输出{1, 2, 3, 4}
```

### Dictionary（字典）

字典是一种映射类型，它的元素是键值对，字典的关键字必须为不可变类型，且不能重复。创建空字典使用 {} 。

示例：

```python
Logo_code = {
 'BIDU':'Baidu',
 'SINA':'Sina',
 'YOKU':'Youku'
 }
print(Logo_code)
# 输出{'BIDU': 'Baidu', 'YOKU': 'Youku', 'SINA': 'Sina'}
print (Logo_code['SINA'])       # 输出键为 'one' 的值
print (Logo_code.keys())   # 输出所有键
print (Logo_code.values()) # 输出所有值
print (len(Logo_code))  # 输出字段长度
```

## Python 流程控制

### if 语句

if 语句表示如何发生什么样的条件，执行什么样的逻辑。

语法：

```python
if 判断条件：
    执行语句……
else：
    执行语句……
```

示例：

```python
# x = int(input("Please enter an integer: "))
x = -5
if  x < 0:
    x = 0
    print('Negative changed to zero')
elif x == 0:
    print('Zero')
elif x == 1:
    print('Single')
else:
    print('More')
```

可能会有零到多个 elif 部分，else 是可选的。关键字 ‘elif’ 是 ’else if’ 的缩写，这个可以有效地避免过深的缩进。if … elif … elif … 序列用于替代其它语言中的 switch 或 case 语句。

### for 循环

Python for 循环可以遍历任何序列的项目，如一个 列表 或者一个 字符串。

语法：

for 循环的语法格式如下：

```python
'''
for 后跟变量名，in 后跟序列，注意加冒号
for 循环每次从序列中取一个值放到变量中
此处的序列主要指 列表  元组   字符串   文件
'''
for iterating_var in sequence:
   statements(s)
```

示例如下：

```python
for letter in 'Python':     # 第一个实例
   print('当前字母 :', letter)

fruits = ['banana', 'apple',  'mango']
for fruit in fruits:        # 第二个实例
   print('当前字母 :', fruit)

print("Good bye!")
```

也可以通过索引地址来遍历内容

```python
fruits = ['banana', 'apple',  'mango']
for index in range(len(fruits)):
   print('当前水果 :', fruits[index])

print("Good bye!")
```

### while 循环

Python 编程中 while 语句用于循环执行程序，即在某条件下，循环执行某段程序，以处理需要重复处理的相同任务。其基本形式为：

语法：

```python
while 判断条件：
    执行语句……
```

示例：

```python
count = 0
while (count < 9):
   print( 'The count is:', count)
   count = count + 1
 
print("Good bye!")
```

也可以在 while 循环中添加判断逻辑

```python
count = 0
while count < 5:
   print(count, " is  less than 6")
   count = count + 1
else:
   print(count, " is not less than 6")
```

### range() 函数

如果你需要一个数值序列，内置函数 range() 会很方便，它生成一个等差级数链表:

语法：

```python
range (start， end， scan):
```

参数含义：

- start:计数从 start 开始。默认是从 0 开始。例如 range(5) 等价于 range(0, 5);
- end:计数到 end 结束，但不包括 end.例如：range(0, 5) 是[0, 1, 2, 3, 4]没有 5
- scan：每次跳跃的间距，默认为1。例如：range(0, 5) 等价于 range(0, 5, 1)

示例：

```python
for i in range(6):
 print(i)
print(range(6),'finish')

for i in range(6,10):
 print(i)
print(range(6,10),'finish')

for i in range(6,12,2):
 print(i)
print(range(6,12,2),'finish')
```

需要迭代链表索引的话，如下所示结合使 用 range() 和 len():

```python
a = ['i', 'love', 'coding', 'and', 'free']
for i in range(len(a)):
	print(i, a[i])
```

### break 用法

break 语句可以跳出 for 和 while 的循环体。如果你从 for 或 while 循环中终止，任何对应的循环 else 块将不执行。

示例：

```python
for letter in 'ityouknow':     # 第一个实例
   if letter == 'n':        # 字母为 n 时中断
      break
   print ('当前字母 :', letter)
```

### continue 用法

continue 语句被用来跳过当前循环块中的剩余语句，然后继续进行下一轮循环。

示例：

```python
for letter in 'ityouknow':     # 第一个实例
   if letter == 'n':        # 字母为 n 时跳过输出
      continue
   print ('当前字母 :', letter)
```

### pass 语句

Python pass 是空语句，是为了保持程序结构的完整性。它用于那些语法上必须要有什么语句，但程序什么也不做的场合.

示例：

```python
while True:
  pass  # Busy-wait for keyboard interrupt (Ctrl+C)


# 这通常用于创建最小结构的类:

class MyEmptyClass:
  pass
```

## Python函数

函数是组织好的，可重复使用的，用来实现单一，或相关联功能的代码段，所以我经常说函数是程序员规模化使用的基础。

函数能提高应用的模块性，和代码的重复利用率。在程序设计中，常将一些常用的功能模块编写成函数，放在函数库中供公共选用。善于利用函数，可以减少重复编写程序段的工作量。

定义一个函数:

定义一个函数有如下几个步骤

- 函数代码块以 def 关键词开头，后接函数标识符名称和圆括号()。
- 任何传入参数和自变量必须放在圆括号中间。圆括号之间可以用于定义参数。
- 函数的第一行语句可以选择性地使用文档字符串—用于存放函数说明。
- 函数内容以冒号起始，并且缩进。
- return [表达式] 结束函数，选择性地返回一个值给调用方。不带表达式的return相当于返回 None。

语法：

```python
def 函数名（参数列表）:
    函数体
```

**示例:**

```python
#定义一个函数
def hello() :
   print("Hello World!")

#调用函数
hello()
```

**一个带参数的示例：**

```python
#定义一个函数
def helloN(name) :
   print("Hello World!", name)

#调用函数
helloN('neo')
```

**多个返回值:**

```python
#定义多个返回值函数
def more(x, y):
    nx = x + 2
    ny = y - 2
    return nx, ny

#调用函数
x, y = more(10, 10)
print(x, y)
```

**递归函数:**

有时候我们需要反复调用某个函数得到一个最后的值，这个时候使用递归函数是最好的解决方案。

编程语言中，函数Func(Type a,……)直接或间接调用函数本身，则该函数称为递归函数。递归函数不能定义为内联函数

举个例子，我们来计算阶乘n! = 1 x 2 x 3 x ... x n，用函数fact(n)表示，可以看出：

```python
fact(n) = n! = 1 x 2 x 3 x ... x (n-1) x n = (n-1)! x n = fact(n-1) x n
```

所以，fact(n)可以表示为n x fact(n-1)，只有n=1时需要特殊处理。

于是，fact(n)用递归的方式写出来就是：

```python
def fact(n):
    if n==1:
        return 1
    return n * fact(n - 1)
```

这样一个递归函数就定义完了。

我们试着调用一下 6 的阶乘是多少，可以这样调用：

```python
print(fact(6))
# 输出内容
# 720
```

## Python 模块和包

模块、包、库之间的概念：

- 模块(module)其实就是 py 文件，里面定义了一些函数、类、变量等
- 包(package)是多个模块的聚合体形成的文件夹，里面可以是多个 py 文件，也可以嵌套文件夹
- 库是参考其他编程语言的说法，是指完成一定功能的代码集合，在 Python 中的形式就是模块和包

模块其实一个 py 文件，用来封装一组功能；包是将一类模块归集到一起，比模块的概念更大一些；库就是由其它程序员封装好的功能组，一般比包的概念更大一些。

### 模块

由上面的内容我们得知模块就是一个 py 文件，这个文本文件中存储着一组功能，方面我们再次使用的时候，提高代码的复用率。我们成这一个的一个 py 文件为  Python 模块（Module）。其他 Python 脚本中，通过 import 载入定义好的 Python 模块。

定义和调用 Python 模块

我们先来看如何定义一个 Python 模块。

定义一个 hello.py 模块，内容如下：

```python
def sayhello(  ):
   print("Hello World!")
```

通常我们使用 import 语句来引入模块，语法如下：

```python
import module1[, module2[,... moduleN]]
```

当解释器遇到 import 语句，如果模块在当前的搜索路径就会被导入。调用的时候使用 模块名.函数名 来进行调用

以上的示例为例，我们新建 do.py 文件调用 hello.py 模块中方法。

do.py 文件内容如下：

```python
# 导入模块
import hello
 
# 现在可以调用模块里包含的函数了
hello.sayhello()
```

> 一个模块只会被导入一次，不管你执行了多少次import。这样可以防止导入模块被一遍又一遍地执行。

在 do.py 页面执行快捷键 ctrl+b 控制台输出：Hello World!，证明调用 hello.py 中的方法成功。

这就是一个模块的定义和调用的示例，是不是也很简单。

from ... import ...

模块提供了类似名字空间的限制，允许 Python 从模块中导入指定的符号（变量、函数、类等）到当前模块。导入后，这些符号就可以直接使用，而不需要前缀模块名。

语法如下：

```python
from modname import name1[, name2[, ... nameN]]
```

例如，要导入模块 hello 的 sayhello 函数，使用如下语句：

```python
## 直接导入方法
from hello import sayhello
sayhello()
```

from … import * 语句
把一个模块的所有内容全都导入到当前的命名空间也是可行的，只需使用如下声明：

```python
from modname import *
```

这提供了一个简单的方法来导入一个模块中的所有项目。

我们在 hello.py 中再添加一个 world 方法。

```python
def world():
   print("Python World!")
```

在 do.py 文件中引入全部方法进行调用。

```python
## 导入所有方法
from hello import *
sayhello()
world()
```

执行后输出：

```python
Hello World!
Python World!
```

证明 hello 模块中的两个方法都可以直接调用，实际项目中不推荐被过多地使用。

### 包

包（package）是 Python 中对模块的更高一级的抽象。简单来说，Python 允许用户把目录当成模块看待。这样一来，目录中的不同模块文件，就变成了「包」里面的子模块。此外，包目录下还可以有子目录，这些子目录也可以是 Python 包。这种分层，对模块识别、管理，都是非常有好处的。

特别地，对于一些大型 Python 工具包，内里可能有成百上千个不同功能的模块。科学计算领域，SciPy, NumPy, Matplotlib 等第三方工具，都是用包的形式发布的。

包定义

常见的包结构如下：

```python
pakageName
-------__init__.py
-------moduleName1.py
-------moduleName2.py
------- ...
```

包路径下必须存在 \__init__.py 文件。

示例：

我们创建一个 cal 的包，包中有一个计算器的 model ，结构如下：

```python
cal
-------__init__.py
-------calculator.py
```

calculator.py 模块的代码如下：

```python
def add(a,b) :
   return a+b

def reduce(a,b) :
   return a-b

def multiply(a,b) :
   return a*b

def divide(a,b) :
   return a/b
```

使用 Python 包

Python 包的使用和模块的使用类似，下面是导入的语法：

```python
import 包名.包名.模块名
```

比如我们在 do.py 中导入 calculator.py

```python
# 导入包
import cal.calculator
# 使用包的模块的方法
print(cal.calculator.add(1,2))
```

但是导入调用的时候报名比较长，这样就可以使用from ... import ...语句来简化一下。

```python
# 导入包
from cal import calculator
# 使用包的模块的方法
print(calculator.multiply(3,6))
```

当包名越来越长的时候效果也会越好。

## 参考链接
