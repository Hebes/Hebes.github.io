import io
import os

path="C:/Users/yet/Desktop/Hebes.github.io/docs/md/Unity前端"

letters = ("jpg","Png","png")
# 获取文件
def GetAllFile(letters):
    # all_files = [f for f in os.listdir(path )]#输出根path下的所有文件名到一个列表中
    # #对各个文件进行处理
    # print(all_files)
    for root, dirs, files in os.walk(path):
        for file in files:
            if os.path.join(root, file).endswith(letters):
                print(os.path.join(root, file))

GetAllFile(letters);
