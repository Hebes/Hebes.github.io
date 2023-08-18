# UGUI

## 参考

**[Unity设置默认字体](<https://blog.csdn.net/zcaixzy5211314/article/details/79549149>)**
**[UGUI系列导航帖](<https://blog.csdn.net/zcaixzy5211314/article/details/86515168>)**

## 6.UGUI的Text实现首行缩进的办法

```javascript {.line-numbers}
Text txt = GetComponent<Text>();
string str = txt. text;
str = "\u3000\u3000" +str;
txt. text = str;
```
