import requests  ###爬虫模块,获取网页文本
import re  ###正则表达式模块,从网页文本中提取所需要的信息
import os
import numpy as np

# https://zhuanlan.zhihu.com/p/149422113 代码参考
# https://blog.csdn.net/Ssuper_X/article/details/109694479 获取浏览器的标识
# https://blog.csdn.net/qdPython/article/details/120845987 python创建数组的详细操作方法
# https://blog.csdn.net/qq_34511096/article/details/127978118 Python正则匹配re模块
# https://blog.csdn.net/weixin_42608414/article/details/109923442 在python的List中使用for循环语句的技巧汇编
# https://zhuanlan.zhihu.com/p/139596371 Python 正则表达re模块之findall()详解
# https://zhuanlan.zhihu.com/p/210779471 Python中列表、元组和数组的区别和骚操作
# https://blog.csdn.net/daoxiaxingcai46/article/details/78269910 python 中列表（list）合并、数组（array）合并

headers = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36",
}

# 使用时修改url即可
url = "https://www.cnblogs.com/shamoyuu/p/CropCamera.html"

ImageTypeList = ["png", "jpg", "git"]

a = []

def get_html(url):
    html = requests.get(url, headers=headers).text
    print("获取Html内容完成!")
    return html


def parse_html(html_text):
    print("开始匹配图片链接...")
    for imageType in ImageTypeList:
        print("匹配后缀"+imageType)
        picre = re.compile(r"[a-zA-z]+://[^\s]*\." + imageType)  # 本正则式得到.jpg结尾的url
        pic_list = re.findall(picre, html_text)
        for pic_listTemp in pic_list:
            a.append(pic_listTemp);
    print("匹配完成!!")
    return np.array(a)


def download(file_path, pic_url):
    r = requests.get(pic_url, headers=headers)
    with open(file_path, "wb") as f:
        f.write(r.content)


def main():
    html_text = get_html(url)
    pic_list = parse_html(html_text)
    os.makedirs("../pic/", exist_ok=True)  # 输出目录
    for pic_url in pic_list:
        file_name = pic_url.split("/")[-1]
        file_path = "../pic/" + file_name
        print("下载路径是:"+file_path)
        download(file_path, pic_url)


if __name__ == "__main__":
    main()
    print("下载完成!")
