using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager : Singleton<AStarManager>
{
    //地图宽高
    private int mapWidth;
    private int mapHeight;
    //地图所有格子
    public AStarGrid[,] grids;
    //开启列表和关闭列表
    private List<AStarGrid> openList = new List<AStarGrid>();
    private List<AStarGrid> closeList = new List<AStarGrid>();

    /// <summary>
    /// 初始化地图
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    public void InitMap(int w, int h)
    {
        this.mapWidth = w;
        this.mapHeight = h;
        grids = new AStarGrid[w, h];
        //根据宽高创建格子   阻挡可以随机
        for (int i = 0; i < w; ++i)
        {
            for (int j = 0; j < h; ++j)
            {
                //随机格子类型,实际项目地图信息应该从配置文件中读取
                grids[i, j] = new AStarGrid(i, j, Random.Range(0, 100) < 20 ? GridType.Stop : GridType.Walk);
            }
        }
    }

    /// <summary>
    /// 寻路
    /// </summary>
    /// <param name="start">起点</param>
    /// <param name="end">终点</param>
    /// <returns></returns>
    public List<AStarGrid> GetPath(Vector2 start, Vector2 end)
    {
        //判断起点 终点是否合法
        //1.是否在地图内
        //2.是否是阻挡
        if (!IsLegal((int)start.x, (int)start.y) || !IsLegal((int)end.x, (int)end.y))
        {
            Debug.Log("开始或者结束位置在地图外");
            return null;
        }

        AStarGrid startGrid = grids[(int)start.x, (int)start.y];
        AStarGrid endGrid = grids[(int)end.x, (int)end.y];
        if (startGrid.type == GridType.Stop || endGrid.type == GridType.Stop)
        {
            Debug.Log("开始或者结束是阻挡");
            return null;
        }
        closeList.Clear();
        openList.Clear();
        //开始点放入关闭列表
        startGrid.fater = null;
        startGrid.f = 0;
        startGrid.g = 0;
        startGrid.h = 0;
        closeList.Add(startGrid);

        while (true)
        {
            //从起点开始找周围的点，并放入开启列表中
            FindNearNodeToOpenList((int)startGrid.x - 1, (int)startGrid.y - 1, 1.4f, startGrid, endGrid);
            FindNearNodeToOpenList((int)startGrid.x, (int)startGrid.y - 1, 1f, startGrid, endGrid);
            FindNearNodeToOpenList((int)startGrid.x + 1, (int)startGrid.y - 1, 1.4f, startGrid, endGrid);
            FindNearNodeToOpenList((int)startGrid.x - 1, (int)startGrid.y, 1f, startGrid, endGrid);
            FindNearNodeToOpenList((int)startGrid.x + 1, (int)startGrid.y, 1f, startGrid, endGrid);
            FindNearNodeToOpenList((int)startGrid.x - 1, (int)startGrid.y + 1, 1.4f, startGrid, endGrid);
            FindNearNodeToOpenList((int)startGrid.x, (int)startGrid.y + 1, 1f, startGrid, endGrid);
            FindNearNodeToOpenList((int)startGrid.x + 1, (int)startGrid.y + 1, 1.4f, startGrid, endGrid);
            //找到开启列表中寻路消耗最小的点
            openList.Sort(SortBy);
            //放入关闭列表，并从开启列表中删除该点
            closeList.Add(openList[0]);
            startGrid = openList[0];
            openList.RemoveAt(0);
            //如果该点是终点，返回最终结果
            if (startGrid == endGrid)
            {
                List<AStarGrid> path = new List<AStarGrid>();
                path.Add(endGrid);
                while (endGrid.fater != null)
                {
                    //终点指向起点
                    path.Add(endGrid.fater);
                    endGrid = endGrid.fater;
                }
                //反转路径
                path.Reverse();
                return path;
            }
            //死路  开启列表为空
            if (openList.Count == 0)
            {
                Debug.Log("没有可以通向终点的路");
                return null;
            }
        }
    }

    /// <summary>
    /// 该位置是否合法
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsLegal(int x, int y)
    {
        if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 找到某个节点周围的点，并放入开启列表
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void FindNearNodeToOpenList(int x, int y, float g, AStarGrid fatherGird, AStarGrid endGird)
    {
        //坐标不合法
        if (!IsLegal(x, y))
        {
            return;
        }
        AStarGrid grid = grids[(int)x, (int)y];
        //格子为空或者格子为阻挡 或者 格子已经在开启列表或者关闭列表中（即格子已经遍历）
        if (grid == null || grid.type==GridType.Stop || closeList.Contains(grid) || openList.Contains(grid))
        {
            return;
        }

        //计算寻路消耗值 f=g+h
        grid.fater = fatherGird;
        grid.g = fatherGird.g + g;     //父亲离起点的距离+我离父亲的距离
        grid.h = Mathf.Abs(endGird.x - grid.x) + Mathf.Abs(endGird.y - grid.y);
        grid.f = grid.g + grid.h;

        openList.Add(grid);
    }

    /// <summary>
    /// 排序规则 从小到大
    /// </summary>
    private int SortBy(AStarGrid A,AStarGrid B)
    {
        if(A.f>B.f)
        {
            return 1;
        }else
        {
            return -1;
        }
    }
}
