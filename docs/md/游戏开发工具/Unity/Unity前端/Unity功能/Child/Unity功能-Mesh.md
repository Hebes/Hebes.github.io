# Unity功能-Mesh

## 创建线

```c#
private Mesh CreateLineMesh(Vector3 start, Vector3 end)
{
    var vertices = new List<Vector3>{start, end};
    var indices = new List<int>{0,1};
    
    Mesh mesh = new Mesh();
    mesh.SetVertices(vertices);
    mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
    return mesh;
}
```

## 创建圆

```c#
private Mesh CreateCircleMesh(float radius)
{
    List<Vector3> vertexList = new List<Vector3>();
    List<int> indexList = new List<int>();
    for(float i = 0; i < 360f; i += 5f)
    {
        float rad = Mathf.Deg2Rad * i;
        float cosA = Mathf.Cos(rad);
        float sinA = Mathf.Sin(rad);
        vertexList.Add(new Vector3(radius * cosA, radius * sinA, 0));
        if(i != 0)
        {
            vertexList.Add(new Vector3(radius * cosA, radius * sinA, 0));
        }        
    }
    vertexList.Add(new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * 0), radius * Mathf.Sin(Mathf.Deg2Rad * 0), 0));
    for(int i = 0; i < 144; i++)
    {
        indexList.Add(i);
    }
    Mesh mesh = new Mesh();
    mesh.SetVertices(vertexList);
    mesh.SetIndices(indexList.ToArray(), MeshTopology.Lines, 0);
    return mesh;
}
```

## 创建正方形

```c#
private Mesh CreatePlaneMesh(Vector2 size)
{
    var vertices = new List<Vector3>();
    var indices = new List<int>();
    
    var x = size.x * 0.5f;
    var z = size.y * 0.5f;
    
    vertices.Add(new Vector3(x, 0f, z));
    vertices.Add(new Vector3(-x, 0f, z));
    vertices.Add(new Vector3(-x, 0f, -z));
    vertices.Add(new Vector3(x, 0f, -z));
    
    indices.Add(0);
    indices.Add(1);
    indices.Add(2);
    indices.Add(0);
    indices.Add(2);
    indices.Add(3);
    
    Mesh mesh = new Mesh();
    mesh.SetVertices(vertices);
    mesh.SetTriangles(indices, 0);
    mesh.RecalculateNormals();
    
    return mesh;
}
```

## 创建圆锥

```c#
private Mesh CreateConeMesh(float radius, float height)
{
    var vertices = new List<Vector3>();
    var indices = new List<int>();
    vertices.Add(Vector3.zero);
    vertices.Add(Vector3.up * height);
    var temp = new List<Vector3>();
    
    //底圆面
    for(var i = 0f; i < 360f; i += 30)
    {
        var rad = Mathf.Deg2Rad * i;
        var x = radius * Mathf.Cos(rad);
        var z = radius * Mathf.Sin(rad);
        
        temp.Add(new Vector3(x, 0f, z));
    }
    
    vertices.AddRange(temp);
    vertices.AddRange(temp);
    
    for(var i = 2; i <= 13; i++)
    {
        indices.Add(i);
        if(i < 13)
        {
            indices.Add(i +1);            
        }
        else
        {
            indices.ADd(2);
        }
        indices.Add(0);
    }
    
    for(var i = 14; i <= 25; i++)
    {
        indices.Add(i);
        indices.Add(1);
        if(i <25)
        {
            indices.Add(i + 1);
        }
        else
        {
            indices.Add(14);
        }
    }
    
    Mesh mesh = new Mesh();
    mesh.SetVertices(vertices);
    mesh.SetTriangles(indices, 0);
    mesh.RecalculateNormals();
    
    return mesh;
}
```
