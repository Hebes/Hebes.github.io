# -*- coding: utf-8 -*-  

import os

path = "C:/Users/yet/Desktop/Hebes.github.io/docs/md/Unity前端/Unity功能" #文件夹目录


def file_name(file_dir):  
  for root, dirs, files in os.walk(file_dir): 
    # print(root) #当前目录路径 
    # print(dirs) #当前路径下所有子目录 
    # print(files) #当前路径下所有非目录子文件 
    # print(root+"/"+files) #当前路径下所有非目录子文件
    for file in files:
    #   NewRoot = str(root).replace('\\','//')
    # - [Unity编辑器-拾色器ColorPicker](/md/Unity前端/Unity编辑器/Child/Unity编辑器-拾色器ColorPicker.md)
      newFile = str(file).replace('.md','')
      newFile = str(newFile).replace('.txt','')
      newRoot = str(root).replace('C:/Users/yet/Desktop/Hebes.github.io/docs/','')
      AllPath = str("- ["+ newFile +"]("+ newRoot +"/" + file +")")
      if AllPath.find(".git\objects") != -1:
        continue
      if AllPath.find(".git\hooks") != -1:
        continue
      if AllPath.find(".png") !=  -1:
        continue
      if AllPath.find(".docx") !=  -1:
        continue
      if AllPath.find(".gif") !=  -1:
        continue
      if AllPath.find(".7z") !=  -1:
        continue
      if AllPath.find(".snippet") !=  -1:
        continue
      if AllPath.find(".unitypackage") !=  -1:
        continue
      if AllPath.find(".zip") !=  -1:
        continue
      if AllPath.find(".dll") !=  -1:
        continue
      if AllPath.find(".jpg") !=  -1:
        continue
      if AllPath.find(".py") !=  -1:
        continue
      if AllPath.find("_sidebar.md") !=  -1:
        continue
      print(AllPath)

file_name(path)

