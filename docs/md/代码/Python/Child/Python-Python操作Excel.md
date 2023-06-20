# 第103天： Python 操作 Excel

## 常用工具

数据处理是 Python 的一大应用场景，而 Excel 又是当前最流行的数据处理软件。因此用 Python 进行数据处理时，很容易会和 Excel 打起交道。得益于前人的辛勤劳作，Python 处理 Excel 已有很多现成的轮子，比如 xlrd & xlwt & xlutils 、 XlsxWriter 、 OpenPyXL ，而在 Windows 平台上可以直接调用 Microsoft Excel 的开放接口，这些都是比较常用的工具，还有其他一些优秀的工具这里就不一一介绍，接下来我们通过一个表格展示各工具之间的特点：

| 类型 | xlrd&xlwt&xlutils | XlsxWriter | OpenPyXL | Excel开放接口 |
| --- | --- | --- | --- | --- |
| 读取 | 支持 | 不支持 | 支持 | 支持 |
| 写入 | 支持 | 支持 | 支持 | 支持 |
| 修改 | 支持 | 不支持 | 支持 | 支持 |
| xls | 支持 | 不支持 | 不支持 | 支持 |
| xlsx | 高版本支持 | 支持 | 支持 | 支持 |
| 大文件 | 不支持 | 支持 | 支持 | 不支持 |
| 效率 | 快 | 快 | 快 | 超慢 |
| 功能 | 较弱 | 强大 | 一般 | 超强大 |

以上可以根据需求不同，选择合适的工具，现在为大家主要介绍下最常用的 xlrd & xlwt & xlutils 系列工具的使用。

## xlrd & xlwt & xlutils 介绍

xlrd&xlwt&xlutils 顾明思意是由以下三个库组成：

+ xlrd：用于读取 Excel 文件；
+ xlwt：用于写入 Excel 文件；
+ xlutils：用于操作 Excel 文件的实用工具，比如复制、分割、筛选等；

### 安装库

安装比较简单，直接用 pip 工具安装三个库即可，安装命令如下：

```python
pip3 install xlrd xlwt xlutils
```

安装完成提示 `Successfully installed xlrd-1.2.0 xlutils-2.0.0 xlwt-1.3.0` 即表示安装成功。

### 写入 Excel

接下来我们就从写入 Excel 开始，话不多说直接看代码如下：

```auto
# excel_w.py

# 导入 xlwt 库
import xlwt

# 创建 xls 文件对象
wb = xlwt.Workbook()

# 新增两个表单页
sh1 = wb.add_sheet('成绩')
sh2 = wb.add_sheet('汇总')

# 然后按照位置来添加数据,第一个参数是行，第二个参数是列
# 写入第一个sheet
sh1.write(0, 0, '姓名')
sh1.write(0, 1, '成绩')
sh1.write(1, 0, '张三')
sh1.write(1, 1, 88)
sh1.write(2, 0, '李四')
sh1.write(2, 1, 99.5)

# 写入第二个sheet
sh2.write(0, 0, '总分')
sh2.write(1, 0, 187.5)

# 最后保存文件即可
wb.save('test_w.xls')
```

然后执行命令 `python excel_w.py` 运行代码，结果会看到生成名为 `test_w.xls` 的 Excel 文件，打开文件查看如下图所示

![1](../Image/Python-Python%E6%93%8D%E4%BD%9CExcel/1.png)

![2](../Image/Python-Python%E6%93%8D%E4%BD%9CExcel/2.png)

以上就是写入 Excel 的代码，是不是很简单，下面我们再来看下读取 Excel 该如何操作。  

### 读取 Excel

读取 Excel 其实也不难，请看如下代码：

```auto
# excel_r.py

# 导入 xlrd 库
import xlrd

# 打开刚才我们写入的 test_w.xls 文件
wb = xlrd.open_workbook("test_w.xls")

# 获取并打印 sheet 数量
print( "sheet 数量:", wb.nsheets)

# 获取并打印 sheet 名称
print( "sheet 名称:", wb.sheet_names())

# 根据 sheet 索引获取内容
sh1 = wb.sheet_by_index(0)
# 或者
# 也可根据 sheet 名称获取内容
# sh = wb.sheet_by_name('成绩')

# 获取并打印该 sheet 行数和列数
print( u"sheet %s 共 %d 行 %d 列" % (sh1.name, sh1.nrows, sh1.ncols))

# 获取并打印某个单元格的值
print( "第一行第二列的值为:", sh1.cell_value(0, 1))

# 获取整行或整列的值
rows = sh1.row_values(0) # 获取第一行内容
cols = sh1.col_values(1) # 获取第二列内容

# 打印获取的行列值
print( "第一行的值为:", rows)
print( "第二列的值为:", cols)

# 获取单元格内容的数据类型
print( "第二行第一列的值类型为:", sh1.cell(1, 0).ctype)

# 遍历所有表单内容
for sh in wb.sheets():
    for r in range(sh.nrows):
        # 输出指定行
        print( sh.row(r))
```

我已经把每行代码都加了注释，应该可以很容易理解，接下来执行命令 `python excel_r.py` ，输出如下结果：

```auto
$ python excel_r.py
sheet 数量: 2
sheet 名称: ['成绩', '汇总']
sheet 成绩 共 3 行 2 列
第一行第二列的值为: 成绩
第一行的值为: ['姓名', '成绩']
第二列的值为: ['成绩', 88.0, 99.5]
第二行第一列的值为: 1
[text:'姓名', text:'成绩']
[text:'张三', number:88.0]
[text:'李四', number:99.5]
[text:'总分']
[number:187.5]
```

细心的朋友可能注意到，这里我们可以获取到单元格的类型，上面我们读取类型时获取的是数字1，那1表示什么类型，又都有什么类型呢？别急下面我们通过一个表格展示下：

| 数值 | 类型 | 说明 |
| --- | --- | --- |
| 0 | empty | 空 |
| 1 | string | 字符串 |
| 2 | number | 数字 |
| 3 | date | 日期 |
| 4 | boolean | 布尔值 |
| 5 | error | 错误 |

通过上面表格，我们可以知道刚获取单元格类型返回的数字1对应的就是字符串类型。

### 修改 excel

上面说了写入和读取 Excel 内容，接下来我们就说下更新修改 Excel 该如何操作，修改时就需要用到 `xlutils` 中的方法了。直接上代码，来看下最简单的修改操作：

```auto
# excel_u.py

# 导入相应模块
import xlrd
from xlutils.copy import copy

# 打开 excel 文件
readbook = xlrd.open_workbook("test_w.xls")

# 复制一份
wb = copy(readbook)

# 选取第一个表单
sh1 = wb.get_sheet(0)

# 在第四行新增写入数据
sh1.write(3, 0, '王亮')
sh1.write(3, 1, 59)

# 选取第二个表单
sh1 = wb.get_sheet(1)

# 替换总成绩数据
sh1.write(1, 0, 246.5)

# 保存
wb.save('test_w1.xls')
```

从上面代码可以看出，这里的修改 Excel 是通过 `xlutils` 库的 `copy` 方法将原来的 Excel 整个复制一份，然后再做修改操作，最后再保存。现在我们执行以下命令 `python excel_u.py` 看下修改结果如下：

![3](../Image/Python-Python%E6%93%8D%E4%BD%9CExcel/3.png)

![4](../Image/Python-Python%E6%93%8D%E4%BD%9CExcel/4.png)

### 格式转换操作  

在平时我们使用 Excel 时会对数据进行一下格式化，或者样式设置，在这里把上面介绍写入的代码简单修改下，使输出的格式稍微改变一下，代码如下：

```auto
# excel_w2.py

# 导入 xlwt 库
import xlwt

# 设置写出格式字体红色加粗
styleBR = xlwt.easyxf('font: name Times New Roman, color-index red, bold on')

# 设置数字型格式为小数点后保留两位
styleNum = xlwt.easyxf(num_format_str='#,##0.00')

# 设置日期型格式显示为YYYY-MM-DD
styleDate = xlwt.easyxf(num_format_str='YYYY-MM-DD')

# 创建 xls 文件对象
wb = xlwt.Workbook()

# 新增两个表单页
sh1 = wb.add_sheet('成绩')
sh2 = wb.add_sheet('汇总')

# 然后按照位置来添加数据,第一个参数是行，第二个参数是列
sh1.write(0, 0, '姓名', styleBR)   # 设置表头字体为红色加粗
sh1.write(0, 1, '日期', styleBR)   # 设置表头字体为红色加粗
sh1.write(0, 2, '成绩', styleBR)   # 设置表头字体为红色加粗

# 插入数据
sh1.write(1, 0, '张三',)
sh1.write(1, 1, '2019-01-01', styleDate)
sh1.write(1, 2, 88, styleNum)
sh1.write(2, 0, '李四')
sh1.write(2, 1, '2019-02-02')
sh1.write(2, 2, 99.5, styleNum)

# 设置单元格内容居中的格式
alignment = xlwt.Alignment()
alignment.horz = xlwt.Alignment.HORZ_CENTER
style = xlwt.XFStyle()
style.alignment = alignment

# 合并A4,B4单元格，并将内容设置为居中
sh1.write_merge(3, 3, 0, 1, '总分', style)

# 通过公式，计算C2+C3单元格的和
sh1.write(3, 2, xlwt.Formula("C2+C3"))

# 对 sheet2 写入数据
sh2.write(0, 0, '总分', styleBR)
sh2.write(1, 0, 187.5)

# 最后保存文件即可
wb.save('test_w3.xls')
```

然后我们执行命令 `python excel_w2.py` 运行以上代码，来输出文件 `test_w3.xls` ，我们来看看效果怎么样。

![5](../Image/Python-Python%E6%93%8D%E4%BD%9CExcel/5.png)

可以看出，使用代码我们可以对字体，颜色、对齐、合并等平时 Excel 的操作进行设置，也可以格式化日期和数字类型的数据。当然了这里我们只是介绍了部分功能，不过这已经足够我们日常使用了，想了解更多功能操作可以参考文末官网。

## 总结

本文为大家介绍了 Python 中如何操作 Excel 的常用类库，并详细介绍了下 xlrd & xlwt & xlutils 系列工具的使用，总体来看操作并不复杂，不过它对 xlsx 支持比较差，对修改其实支持也不太好，而且功能并不多，不过在 xls 操作中还是占有重要地位的，之后会为大家介绍其他常用 Excel 操作工具。

> 示例代码：<https://github.com/JustDoPython/python-100-day/tree/master/day-103>

## 参考

python-excel 官网：<http://www.python-excel.org/>
