# Unity功能-设置颜色

```C#
Image.Color=new Color(1,1,1,1)或者（255/255f，255/255f，255/255f，255/255f）；
```

Unity中已经提供了现成的方法，可以直接调用：

ColorUtility.TryParseHtmlString：传入的字符串是 “#FFFFFF”的格式，用法如下：

```C#
Color color;
ColorUtility.TryParseHtmlString（"#FECEE1", out color）;
image.Color=color;
```

Color转回去使用下面的方法：

ColorUtility.ToHtmlStringRGB：传入的是一个Color结构体，返回一个字符串，形式是“FFFFFF”，用法如下：

```C#
input16Color.text = ColorUtility.ToHtmlStringRGB(nowColor);
```

## 来源网站

**[Color与十六进制颜色互相转换](<https://blog.csdn.net/pz789as/article/details/81669946>)**
