# Unity组件详解

## 按键影响UI

UI组件中Navigation改为None则不再影响

## 按钮类UI

Navigation选项:改为none之后按键无法控制

## RawImage

可播放视频:场景中创建Video Player和RawImage, 文件中创建Render Texture, 将RawImage和Render Texture拖给Video Player

## 滚动条消失问题

设置: content中添加GridLayoutGroup组件,ContentSizeFitter组件

例:竖直滚动时GridLayoutGroup设置为Vertical,ContentSizeFitter的vertical

## Scroll Rect

Movement Type   Unrestricted 滑到底不停 Elastic 回弹    Clamped 滑到底停止

Inertia 惯性

ScrollSensitivity 滚轮滚动速度

## Mask

滚动窗口中可看见的部分使用Mask,可隐藏其他区域

## Content

Content添加组件 Grid Layout Group 以格子形式限制Content的子类

    Herizontal Layout Group 水平形式限制子类

    Vertical Layout Group 竖直形式限制子类

Content添加组件 Content Size Fitter 自动调整滑块大小

content置顶问题: content的RectTransform组件总的Pivot属性修改可更改位置

## Text相关

ContentSize Fitter 自适应大小

OutLine 描边

Shadow 阴影

## 富文本

```Text
<b>粗体</b>
<i>斜体</i>
<size=50>大小</size>
<color=#ff000000>颜色</color>
<color=red>颜色</color>
```


https://juejin.cn/post/7094536454533038111