#!/usr/bin/python
# -*- coding: UTF-8 -*-

import os

# def file_name(file_dir):  
#   for root, dirs, files in os.walk(file_dir): 
#     # print(root) #当前目录路径 
#     # print(dirs) #当前路径下所有子目录 
#     # print(files) #当前路径下所有非目录子文件 
#     # print(root+"/"+files) #当前路径下所有非目录子文件
#     for file in files:
#     #   NewRoot = str(root).replace('\\','//')
#     # - [Unity编辑器-拾色器ColorPicker](/md/Unity前端/Unity编辑器/Child/Unity编辑器-拾色器ColorPicker.md)
#       newFile = str(file).replace('.md','')
#       newFile = str(newFile).replace('.txt','')
#       newRoot = str(root).replace('C:/Users/yet/Desktop/Hebes.github.io/docs/','')
#       AllPath = str("- ["+ newFile +"]("+ newRoot +"/" + file +")")
#       if AllPath.find(".git\objects") != -1:
#         continue
#       if AllPath.find(".git\hooks") != -1:
#         continue
#       if AllPath.find(".png") !=  -1:
#         continue
#       if AllPath.find(".docx") !=  -1:
#         continue
#       if AllPath.find(".gif") !=  -1:
#         continue
#       if AllPath.find(".7z") !=  -1:
#         continue
#       if AllPath.find(".snippet") !=  -1:
#         continue
#       if AllPath.find(".unitypackage") !=  -1:
#         continue
#       if AllPath.find(".zip") !=  -1:
#         continue
#       if AllPath.find(".dll") !=  -1:
#         continue
#       if AllPath.find(".jpg") !=  -1:
#         continue
#       if AllPath.find(".py") !=  -1:i
#       continue
#       if AllPath.find("_sidebar.md") !=  -1:
#         continue
#       print(AllPath)

def GetAlllPath(file_dir):
  list=[]
  for root, dirs, files in os.walk(file_dir): 
    # print(root) #当前目录路径 
    # print(dirs) #当前路径下所有子目录 
    # print(files) #当前路径下所有非目录子文件 
    # print(root+"/"+files) #当前路径下所有非目录子文件
    for file_name in files:
      list.append(os.path.join(root, file_name))
      # print(os.path.join(root, file_name))
  return list

if __name__ == '__main__':
  path = input("请输入文件夹路径: ")
  for root, dirs, files in os.walk(path): 
    for file_name in files:
      if file_name.find(".jpg") !=  -1:
          continue
      if file_name.find(".png") !=  -1:
          continue
      str2 = os.path.join(root, file_name)
      str2 = str2.replace('\\','/')
      str2 = str2.replace('C:/Users/16073/Desktop/Hebes.github.io/docs','')
      temp1 = str("- ["+ file_name +"]("+ str2 +")")
      print(temp1)