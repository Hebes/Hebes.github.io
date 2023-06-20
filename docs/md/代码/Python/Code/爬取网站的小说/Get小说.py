import requests###爬虫模块,获取网页文本
import re###正则表达式模块,从网页文本中提取所需要的信息

httpUrl="http://m.60ks.cc/155/155605/51418917.html"
head={"User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36"}
# 获取网页内容
def GetHtmlContent(url):
    r = requests.get(url,headers = head,timeout=30)
    r.encoding = 'gbk'
    print(r.text)


GetHtmlContent(httpUrl)
