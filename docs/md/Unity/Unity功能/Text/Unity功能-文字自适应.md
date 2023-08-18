# Unity功能-文字自适应

## 第一种方案 

实际效果

![1](\../Image/Unity功能-文字自适应/1.png)

结构

![2](\../Image/Unity功能-文字自适应/2.png)

设置

![3](\../Image/Unity功能-文字自适应/3.png)
![4](\../Image/Unity功能-文字自适应/4.png)
![5](\../Image/Unity功能-文字自适应/5.png)

Unity不支持ContentSizeFitter 的嵌套，如果使用了嵌套则会出现警告，并且有些时候没有效果。

但是实际应用时候肯定会碰到嵌套的情况，这怎么办呢？

其实我们只需要在最外层加一个ContentSizeFitter，里面使用VertivalLayoutGroup和HorizontalLayoutGroup布局，并且勾上ChildControlsSize的Height或Width（根据实际情况，适配高就勾Height，适配宽就勾Width）

但是会碰到很坑爹的Image之类的组件，使用了ChildControlsSize后这些组件的高或宽会变成0，这时候关键来了，加上LayoutElement，并且设置MinHeight或者MinWidth就可以完美解决。

## 第二种方案 世界模式

![9](\../Image/Unity功能-文字自适应/9.png)

![6](\../Image/Unity功能-文字自适应/7.png)

![7](\../Image/Unity功能-文字自适应/7.png)

![8](\../Image/Unity功能-文字自适应/8.png)

## 参考网站

**[内容大小适配器 (Content Size Fitter)](<https://docs.unity.cn/cn/2019.4/Manual/script-ContentSizeFitter.html>)**

**[UGUI ContentSizeFitter 嵌套 适配](<https://blog.csdn.net/yhjsspz/article/details/108836206>)**
