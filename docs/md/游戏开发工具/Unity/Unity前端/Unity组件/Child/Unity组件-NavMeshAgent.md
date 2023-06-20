# Unity组件-NavMeshAgent

**[导航网格代理 (NavMesh Agent)](<https://docs.unity.cn/cn/2018.4/Manual/class-NavMeshAgent.html>)**

**[Unity3D之Navigation导航系统学习及案例讲解(适合初学者)](<https://blog.csdn.net/qq_35361471/article/details/79857501>)**

## 介绍

NavMeshAgent 组件可帮助您创建在朝目标移动时能够彼此避开的角色。代理 (Agent) 使用导航网格来推断游戏世界，并知道如何避开彼此以及其他移动障碍物。寻路和空间推断是使用导航网格代理的脚本 API 进行处理的。

## 导航网格Nav Mesh

首先需要将地形设置为Navigation Static，如下图所示：

![1](\../Image/Unity组件-NavMeshAgent/1.png)

如果有不想烘焙的地方，取消勾选即可。打开Window-Navigation导航窗口，选择Bake页面，如下所示：

![2](\../Image/Unity组件-NavMeshAgent/2.png)

- Agent Radius：定义网格和地形边缘的距离
- Agent Height：定义可以通行的最高度
- Max Slope：定义可以爬上楼梯的最大坡度
- Step Height：定义可以登上台阶的最大高度
- Drop Height：允许最大下落距离
- Jump Distance：允许最大的跳跃距离

## Nav Mesh Agent

![1](https://docs.unity.cn/cn/2018.4/uploads/Main/NavMeshAgent.png)

| 属性 | 功能 |
| --- | --- |
| *Agent Size* |
| **Radius** | 代理的半径，用于计算障碍物与其他代理之间的碰撞。 |
| **Height** | 代理通过头顶障碍物时所需的高度间隙。 |
| **Base offset** | 碰撞圆柱体相对于变换轴心点的偏移。 |
| *Steering* |
| **Speed** | 最大移动速度（以世界单位/秒表示）。 |
| **Angular Speed** | 最大旋转速度（度/秒）。 |
| **Acceleration** | 最大加速度（以世界单位/平方秒表示）。 |
| **Stopping distance** | 当靠近目标位置的距离达到此值时，代理将停止。 |
| **Auto Braking** | 启用此属性后，代理在到达目标时将减速。对于巡逻等行为（这种情况下，代理应在多个点之间平滑移动）应禁用此属性 |
| *Obstacle Avoidance* |
| **Quality** | 障碍躲避质量。如果拥有大量代理，则可以通过降低障碍躲避质量来节省 CPU 时间。如果将躲避设置为无，则只会解析碰撞，而不会尝试主动躲避其他代理和障碍物。 |
| **Priority** | 执行避障时，此代理将忽略优先级较低的代理。该值应在 0–99 范围内，其中较低的数字表示较高的优先级。 |
| *Path Finding* |
| **Auto Traverse OffMesh Link** | 设置为 true 可自动遍历网格外链接 (Off-Mesh Link)。如果要使用动画或某种特定方式遍历网格外链接，则应关闭此功能。 |
| **Auto Repath** | 启用此属性后，代理将在到达部分路径末尾时尝试再次寻路。当没有到达目标的路径时，将生成一条部分路径通向与目标最近的可达位置。 |
| **Area Mask** | Area Mask 描述了代理在寻路时将考虑的[区域类型](https://docs.unity.cn/cn/2018.4/Manual/nav-AreasAndCosts.html)。在准备网格进行导航网格烘焙时，可设置每个网格区域类型。例如，可将楼梯标记为特殊区域类型，并禁止某些角色类型使用楼梯。 |

常用API:

- ActivateCurrentOffMeshLink ：激活或禁止当前off-MeshLink.
- CalculatePath ：计算到某个点的路径并储存
- CompleteOffMeshLink ：完成当前offMeshLink的移动
- Move ：移动到相对于当前位置的点
- ResetPath ：清除当前路径
- SetDestination ：设置目标点
- SetPath ：设置一条路线
- Warp ：瞬移到某点
- remainingDistance：到目标点的距离
- desiredVelocity：期望速度，方向指向的是到达目标点的最短路径的方向

## Nav Mesh Area

用来设置路径估值(Cost)，比如走楼梯，消耗体能20，而坐电梯，消耗体能5，自然会选择后者方式。

![3](\../Image/Unity组件-NavMeshAgent/3.png)

设置完消耗代价后，在Object面板中设置类型

![4](\../Image/Unity组件-NavMeshAgent/4.png)

Unity内置了三种area，Walkable设置可行走区域，NotWalkable设置不可行走区域，如果不想让角色行走的区域，可以设置成这个。需要注意，做了修改，一定要重新烘焙网格，设置才会有效哦！

## Off Mesh Links

地形之间可能有间隙，形成沟壑，或者是高台不能跳下，导航网格处于非连接的状态，角色无法直接跳跃，要绕一大圈才能到达目的地。off mesh links用于解决这种问题。

可以自动或者手动创建off mesh links，需要先设置bake选项卡中的Drop Height或者Jump Distance属性，前者控制跳跃高台最大高度，后者控制跳跃沟壑的最大距离。

![5](\../Image/Unity组件-NavMeshAgent/5.png)

选中需要创建Links的对象，在Object 选项卡内勾选Generate OffMeshLinks，再重新烘焙即可。需要注意，这里平台1设置了Links，而平台2没有设置。此时，角色只能从平台1跳到平台2，如下图箭头所示：

![6](\../Image/Unity组件-NavMeshAgent/6.png)

手动添加Links，需要为地形对象添加off mesh link组件，可以创建两个空对象，分别用来控制跳跃的开始和结束点，如下所示：

![7](\../Image/Unity组件-NavMeshAgent/7.png)

- Cost Override：路径估值，和之前的Area一样
- Bi Directional：控制跳跃是单向的还是双向的
- Activated：控制Link是否激活
- Auto Update Position：自动更新位置，当移动开始结束点的时候，自动更新

## Nav Mesh Obstacle

![8](\../Image/Unity组件-NavMeshAgent/8.png)

Shape：选择障碍的几何形状
Carve：如果勾选，会重新渲染网格，效果如下所示

![9](\../Image/Unity组件-NavMeshAgent/9.png)

## 移动代码

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
 
public class AIFindWay : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] Points;//通过数组形式可设置多个监测点
    int currentPoint;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Points[0].position);//通过Nav Mesh Agent组件的SetDestination 方法括号内为一个Vector3目标点
    }
 
    void Update()
    {
        if(agent.remainingDistance<agent.stoppingDistance)//到目标的剩余距离是否小于之前在 Inspector窗口中设置的停止距离
        {         
            currentPoint=(currentPoint+1)%Points.Length;//采用取余的方法实现敌人巡逻轨迹的循环
            agent.SetDestination(Points[currentPoint].position);
        }
    }
}
```

```C#
using UnityEngine;
using UnityEngine.AI;

namespace GhostRoom
{
    public class EnemyLeft : MonoBehaviour
    {

        private NavMeshAgent nav;//寻路者
        public Transform[] target;//寻路目标
        private int index = 0;//位置下标
        private float waitTime = 3f;//等待时间
        private float timer = 0;//计时器

        private void Start()
        {
            nav = transform.GetComponent<NavMeshAgent>();
            int range = Random.Range(0, target.Length);
            nav.destination = target[range].position;
        }

        private void Update()
        {
            if (nav.remainingDistance < 0.5f)
            {
                timer += Time.deltaTime;

                if (timer >= waitTime)
                {
                    index++;
                    // index %= 4;
                    int r = Random.Range(0, target.Length);
                    timer = 0;
                    nav.destination = target[r].position;
                }
            }

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                nav.ResetPath();
                nav.destination = other.transform.position;
            }
        }
    }
}
```

