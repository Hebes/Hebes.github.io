# Git-命令

git指令

```css
#配置用户名
git config --global user.name "用户名"
#配置邮箱
git config --global user.email "登录邮箱"
#忽略文件权限
git config --global core.fileMode false #所有版本库
#查看文件权限
cat .git/config
#禁用safe.derectory检测
git config --global --add safe.directory * #(prefix)///路径
#或者直接修改配置文件
[safe]
directory = *


#初始化本地库
git init
#从git中删除file的二进制文件
git rm --cached <file>
#强制允许某文件夹内文件
git add -f <file>

#上传|强制上传
git push origin 分支名 --f
#取消上次提交记录
git reset --soft HEAD~


#修改.gitignore后更新
#清除本地当前的Git缓存
git rm -r --cached .
#应用.gitignore等本地配置文件重新建立Git索引
git add .
#提交当前Git版本并备注说明
git commit -m 'update .gitignore'


#误在其他分支开发
git stash //在主分支使用该命令,保证正在进行的操作,恢复master分支的状态
git checkout dev //切换到自己的本地开发分支
git stash pop //释放暂存的工作到当前分支

#远端强行覆盖本地代码
git fetch --all && git reset --hard origin/master && git pull
```
