# ET6.0基础部分-ObjectWait

## 说明

用于等待，停止继续执行直到收到返回通知,才继续执行后面代码。

## 作用

wait用来阻塞逻辑,不会执行下面的逻辑

```C#
WaitType.Wait_CreateMyUnit waitCreateMyUnit = await zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_CreateMyUnit>();
```

Notif用来提示wait可以继续往下面走

```C#
// 通知场景切换协程继续往下走
session.DomainScene().GetComponent<ObjectWait>().Notify(new WaitType.Wait_CreateMyUnit() {Message = message});
```

两个都必须传入同一个类型,才能生效
