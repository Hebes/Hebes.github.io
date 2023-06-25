# MD文档-MarkDown添加图片的三种方式

https://blog.csdn.net/SlaughterDevil/article/details/79255933

**[Markdown插入图片 详细例子](<https://blog.csdn.net/xapxxf/article/details/105133999>)**

图片转化为base64字符串

```Python
import base64
f=open('723.png','rb') #二进制方式打开图文件
ls_f=base64.b64encode(f.read()) #读取文件内容，转换为base64编码
f.close()
print(ls_f)
```

base64字符串转化为图片

```Python
import base64
bs='iVBORw0KGgoAAAANSUhEUg....' # 太长了省略
imgdata=base64.b64decode(bs)
file=open('2.jpg','wb')
file.write(imgdata)
file.close()
```
