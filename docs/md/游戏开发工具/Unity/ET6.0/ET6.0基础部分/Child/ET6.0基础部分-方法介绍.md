# ET6.0基础部分-方法介绍

## AddChild、AddComponent区别

AddChild可以添加多个,常用于添加实体,比如实操中添加Entiy的computer,作用:
比如当前场景.AddChild电脑,就是电脑的父物体是当前场景,那么销毁当前场景的时候也会调用销毁电脑的方法

AddComponent只能添加一个,常用于添加组件

参考链接: **[Entity 中AddChild和AddComponent有什么区别?](https://et-framework.cn/d/765-entity-addchildaddcomponent)**

## ZonScene()

只能在游戏客户端启动,服务端是没有的,获取ZonScene

## DomainScene()

获取当前的Scene,例如:currentScene的挂载的一个脚本中调用,获取的就是currentScene

客户端和服务端都有,机器人也有
