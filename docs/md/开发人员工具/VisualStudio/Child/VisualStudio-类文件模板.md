# VisualStudio-类文件模板

## VS2019更改类文件模板

1. 在VS的安装目录下找到新建类的模板代码。 我的路径是：

    > ‪D:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Enterprise\\Common7\\IDE\\ItemTemplates\\CSharp\\Code\\2052\\Class\\Class.cs

2. 打开类文件如下所示：

    ```csharp
    using System;
    using System.Collections.Generic;
    $if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
    $endif$using System.Text;
    $if$ ($targetframeworkversion$ >= 4.5)using System.Threading.Tasks;
    $endif$
    namespace $rootnamespace$
    {
    class $safeitemrootname$
    {
    }
    }
    ```

3. 添加自定义信息，更改类模板后。如下：

    ```csharp
    #region << 文 件 说 明 >>
    /*----------------------------------------------------------------
    // 文件名称：$safeitemname$
    // 创 建 者：作者名称
    // 创建时间：$time$
    // 文件版本：V1.0.0
    // ===============================================================
    // 功能描述：
    //
    //
    //----------------------------------------------------------------*/
    #endregion

    using System;
    using System.Collections.Generic;
    $if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
    $endif$using System.Text;
    $if$ ($targetframeworkversion$ >= 4.5)using System.Threading.Tasks;
    $endif$
    namespace $rootnamespace$
    {
        /// <summary>
        /// 
        /// </summary>
    public class $safeitemrootname$
        {
        }
    }
    ```

## 参考网站

**[Visual Studio2013中动态生成注释中的时间__修改模板文件](<https://blog.csdn.net/sx341125/article/details/53404205>)**
