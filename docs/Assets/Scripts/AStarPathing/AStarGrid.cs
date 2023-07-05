using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A*寻路格子类
/// </summary>
public class AStarGrid
{
    //寻路消耗
    public float f;
    //距离起点的距离
    public float g;
    //距离终点的距离
    public float h;
    
    //坐标
    public int x;
    public int y;

    //父对象
    public AStarGrid fater;
    //格子类型
    public GridType type;

    public AStarGrid(int x,int y,GridType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }

}
/// <summary>
/// 格子类型
/// </summary>
public enum GridType
{
    Walk,      //无，可以到达的点
    Stop,      //障碍物
}
