# Unity功能-读写Excel表格

**[Unity实用功能之读写Excel表格](<https://juejin.cn/post/6991464138782277645>)**

**[【Unity3D读取数据】—读取Excel文件](<https://juejin.cn/post/7094532297810001927>)**

## 概述

在项目开发过程中，经常会用到大量的可编辑的数据，而这些数据使用Json，XML等形式存储又比较麻烦 PS：对于不懂电脑的客户来说完全就是看天书，后期编辑也比较费事。所以就有了使用Excel表格进行数据的存储和读取。比如：人员名单（姓名，班级，学号等信息）。所以本篇文章就分享一下如何使用Unity读写Excel表格。

## 准备工作

此篇文章中使用的是Unity2019.4.17版本，VS2017。需要引入EPPlus.dll，Excel.dll 和ICSharpCode.SharpZipLib库文件。

## 读取Excel表格

下图是我们要读取的数据

![image.png](https://p9-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/fec56acfcfb740d5ae6936e27b4a9478~tplv-k3u1fbpfcp-zoom-in-crop-mark:3024:0:0:0.image)

读取Excel需要用到Excel.dll 和ICSharpCode.SharpZipLib库文件。将其放到Plugins文件夹下。 首先需要引入命名空间

1.  using System.IO；此命名空间主要用于对Excel文件的加载作用，没有此命名空间将无法使用FileStream加载文件
2.  using Excel;此命名空间主要用于对Excel的读取功能。 使用如下脚本，首先加载Excel文件

`FileStream fileStream = File.Open(Application.streamingAssetsPath + "/"+ 表格名, FileMode.Open, FileAccess.Read);`

上一句代码为Unity的StreamingAssets目录下的.xlsx文件的路径：Application.streamingAssetsPath + "/表格名.xlsx"

读取文件之后要对文件进行类似实例化解析操作,

`IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);`

将所有数据全部读取出来

`DataSet result = excelDataReader.AsDataSet();`

接下来获取表格的行数和列数

```ini
// 获取表格有多少列
int columns = result.Tables[0].Columns.Count；
// 获取表格有多少行 
int rows = result.Tables[0].Rows.Count;
```

接下来我们要把读取到的每一行每一列数据进行整合，通过for循环，将每一行的数据进行记录，首先读取每一行，然后在读取这一行中的每一列，从而获得整行数据信息。具体代码信息如下

```ini
        // 根据行列依次打印表格中的每个数据 
        List<string> excelDta = new List<string>();

        //第一行为表头，不读取。没有表头从0开始
        for (int i = 1; i < rows; i++)
        {
            value = null;
            all = null;
            for (int j = 0; j < columns; j++)
            {
                // 获取表格中指定行指定列的数据 
                value = result.Tables[0].Rows[i][j].ToString();
                if (value == "")
                {
                    continue; 
                }
                all = all + value + "|";
            }
            if (all != null)
            {
                //print(all);
                excelDta.Add(all);
            }
        }
```

在这里，每一行中每一列数据是使用“|进行分割的，接下来要做的是将读取到的每一行数据存到定义好的类中。方便以后好调用。 首先定义数据类型，要有ID（序号），姓名，班级，学号，电话。 代码如下：

```csharp
public class PlayerInfo
{
    // 序号
    public string ID { get; set; }
    // 姓名
    public string Name { get; set; }
    // 学号
    public string Number { get; set; }
    // 班级
    public string Class { get; set; }
    /// 手机号
    public string Mobile { get; set; }

    public PlayerInfo(string id, string name,string class,string number,string mobile)
    {
        this.ID = id;
        this.Name = name;
        this.Number = number;
        this.Class = class;
        this.Mobile = mobile;
    }
}
```

接下来遍历读取到的数据进行处理然后存储到List中，首先使用Split("|")，读数据进行截取，组成一个是字符串数据，然后在new一个PlayerInfo,传入参数，最后将new的PlayerInfo存入List中。

```ini
string[] item = data_1[i].Split('|');
```

```css
PlayerInfo playerinfo = new PlayerInfo(item[0], item[1], item[2], mobile_Enc);
```

```ini
List<PlayerInfo> Players = new List<PlayerInfo>();
Players.Add(playerinfo);
```

到此，所有数据也就全部读取出来了，可以再控制台进行一下打印，输入结果如下，证明数据读取成功

![image.png](https://p9-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/3ff762cc46244cf78860934028f3208d~tplv-k3u1fbpfcp-zoom-in-crop-mark:3024:0:0:0.image)

## 写入Excel

最终结果是要在上述表格中添加一列，如下所示

![image.png](https://p6-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/8282762d864241d9bb4991e236c7d617~tplv-k3u1fbpfcp-zoom-in-crop-mark:3024:0:0:0.image)

首先，需要引入EPPlus.dll库文件，其次，需要引用命名空间：using OfficeOpenXml;以便对Excel表格进行写入 同样，首先需要打开文件，确认文件是否存在，不存在需要自动创建一个文件

```ini
        //文件路径
        string path = Application.streamingAssetsPath + "/表格名.xlsx";
        FileInfo newFile = new FileInfo(path);
        //判断文件是否中存在
        if (!newFile.Exists)
        {
            //创建一个新的excel文件
            newFile = new FileInfo(path);
        }
```

接下来进行文件的写入，在这之前，我们需要要写入的文件存入一个list中，同样使用"|"对每一列数据进行分割，呀添加的数据为

`"3|王五|20210103|三年一班|11111111111"`

通过ExcelPackage打开文件

`using (ExcelPackage package = new ExcelPackage(newFile))`

然后进行数据分割，存储,这里之所以使用一整串字符串是为了以后存储数据较多的时候，可以直接进行遍历，省的一点点进行添加

```ini
 string[] messages = newList[i].Split('|');  
 string itemId = messages[0];
 string itemName = messages[1];
 string itemNumber = messages[2];
 string imageClass = messages[3];
 string imageMobile = messages[4];
 //添加第四行数据
 worksheet.Cells["A4"].Value = itemId;
 worksheet.Cells["B4"].Value = itemName;
 worksheet.Cells["C4"].Value = itemNumber;
 worksheet.Cells["D4"].Value = imageClass;
 worksheet.Cells["E4"].Value = imageMobile;
```

最后，对表格进行保存

`package.Save();`

## 注意

经测试发现如下几个问题： 1、在打包之后或者没打包的时候就会出现读取不到excel数据，须将  
Unity\\Editor\\Data\\Mono\\lib\\mono\\unity目录下的一系列i18n相关dll导入项目Plugins文件夹中。 2、如果xlsx文件的后缀为.xlsx，读取的代码应该为

`IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);`

若使用CreateBinaryReader读取，则在excelReader.AsDataSet();会报错NullReferenceException: Object reference not set to an instance of an object

3、如果xlsx文件的后缀为.xls，读取的代码应该为

`IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);`

若使用CreateOpenXmlReader读取，则在CreateOpenXmlReader处会报错ArgumentNullException: Value cannot be null.

## 写在最后

整体项目案例会在后期整理好之后分享给大家，如有错误之处还请多多指出。