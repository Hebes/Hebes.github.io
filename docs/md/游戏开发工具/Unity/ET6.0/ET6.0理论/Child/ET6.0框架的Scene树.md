# ET6.0框架的Scene树

**[游戏客户端的Scene层次结构](<https://blog.csdn.net/Q540670228/article/details/123416617>)**

![domain](..\Image\ET3.png)

``` text
domain就是指这个entity属于哪个scene，毕竟一个进程上可以容纳多个scene
domain还有个很重要的作用，就是设置domain的时候才会执行反序列化system，还有注册eventsystem
domain简单说是指属于哪个scene, 每个entity都有个domain字段，这样写逻辑方便能拿到自己scene上的数据

多进程多scene，具体scene放到那个进程完全取决于配制，全放到一个进程就是allserver了

如果是个很大的scene，需要容纳很多人，可能就需要单独占用一个进程，这样才能完全利用一个核

把每个scene都分配一个进程，就跟5.0差不多了
```

在ET框架下，Scene即为场景作为根节点，根节点下可以存放多个实体或组件。但Scenen本质也是实体，所以Scene之间也会有层次关系。

**游戏客户端的Scene层次结构:**

- **GameScene**
游戏客户端全局的Scene根节点，用于提供游戏客户端全局且必要的基础功能组件（资源加载管理组件，计时器组件等）

- **ZoneScene**
用于提供玩家全局游戏业务功能逻辑组件（例如基础UI，背包界面等）
通过添加自定义SceneType和修改Excel配置,就可以启动不同类型的Scene,为服务器进程添加不同的功能
Scene可以在游戏开服前配置好在不同的进程中启动时创建,也可以在游戏运行时动态创建Scene(例如副本,分线场景),同时也可以对Scene进行动态回收

- **CurrentScene**
代表玩家当前所在的地图场景，一般用于挂载当前场景相关的组件，切换或释放场景时回收所有实体及组件。

**总结：**
Scene相当于数据结构中的根节点,实体Entity可以挂载在Scene下,Entity下也可以挂载其他的Entity
Entity的Zone则代表当前所在Scene的逻辑索引ID,在服务器被当作区服的索引ID

```c#
//尽量不通过domain字段获取scene
挂载Entity的实体可以.DomainScene();//获取当前场景,该方法不能在服务端用
.ZoneScene();
```

**游戏服务端Scene层次结构:**

- **GameScene**
​类似客户端，其用来挂载全局服务端所需的基础功能必备组件

- **ZoneScene**
可以创建多个不同功能的ZoneScene, 每个不同功能的ZoneScene下挂载其应该具有的功能组件，例如网关下的NetKcpComponent，定位服务器的LocationComponent等等，一般通过SceneType的枚举对其进行逻辑分发。
不同ZoneScene可以存在一个进程上面，也可以每个都ZoneScene运行在一个单独的进程上。
   不同ZoneScene进程甚至可以分布在服务器集群上，大大提高了运行效率。
​Scene可以动态创建和销毁（用于制作副本等临时场景）
