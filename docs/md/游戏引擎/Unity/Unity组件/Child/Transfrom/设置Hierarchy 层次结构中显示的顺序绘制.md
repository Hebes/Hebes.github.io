# 设置Hierarchy 层次结构中显示的顺序绘制

按照 Hierarchy 层次结构中显示的顺序绘制。
如果两个UI元件重叠，则后一个元件将显示在前一个元件的顶部。

可以通过在 Transform component 的这些方法从脚本控制顺序：

GetSiblingIndex()
SetAsFirstSibling()
SetAsLastSibling()
SetSiblingIndex(int index)
GetSiblingIndex() : 获取 sibling 索引

如果 GameObject 共享相同的父级，这些 GameObject 被称为 sibling。
简单来讲就是同级。

sibling 索引显示每个 GameObject 在此 sibling Hierarchy 层次结构中的位置。

SetAsFirstSibling() : 设定为 sibling 的第一个
SetAsLastSibling() : 设定为 sibling 的最后一个
SetSiblingIndex(int index) : 直接设定 sibling 索引

更多可参看：

Unity - Scripting API: Transform.GetSiblingIndex
Unity - Scripting API: Transform.SetAsFirstSibling
Unity - Scripting API: Transform.SetAsLastSibling
Unity - Scripting API: Transform.SetSiblingIndex