# Unity功能-MeshRenderer变换material颜色

 原文地址：[Unity3D Mesh小课堂（四）MeshRenderer的material和sharedMaterial](https://blog.csdn.net/ecidevilin/article/details/52461525)

[MeshRenderer](http://www.ceeger.com/Components/class-MeshRenderer.html)（网格渲染器）从[MeshFilter](http://www.ceeger.com/Components/class-MeshFilter.html)（网格过滤器）获得几何形状，并根据[Mesh](http://www.ceeger.com/Manual/UsingtheMeshClass.html)进行渲染，而渲染所需要的贴图信息就来自与[Material](http://www.ceeger.com/Manual/Materials.html)

而MeshRenderer的Material类型的变量有两个：material和sharedMaterial。二者之间有什么联系呢？我们做个试验。

首先创建两个Cube，CubeA和CubeB，并新建一个Material材质球，把两个Cube里MeshRenderer的第一个Material都改成这个新建的材质。然后新建一个代码文件，把代码加到CubeA上，在start方法里面加入以下代码：

```c#
MeshRenderer mr = GetComponent<MeshRenderer> ();  
mr.material.color = Color.red; 
```

点击运行我们看到只有CubeA颜色变了。

![1](https://img-blog.csdn.net/20160908084322631)

如果换成

```c#
MeshRenderer mr = GetComponent<MeshRenderer> ();  
mr.sharedMaterial.color = Color.red;
```

就会发现，CubeA和CubeB的颜色都变了，并且我们新建的Material（材质球）的颜色也变了。

![2](https://img-blog.csdn.net/20160908084603353)

由此我们可以知道：

sharedMaterial是公用的Material，所有用到这个材质的MeshRendered都会引用这个Material。改变sharedMaterial的属性也会改变mat文件。

material是独立的Material，改变material的属性不会影响到其他对象，也不会影响mat文件。

但是，看问题不应该浮于表面。

我们继续做实验，删掉之前的代码加入下面这段：

```C#
MeshRenderer mr = GetComponent<MeshRenderer> ();  
Material smat = mr.sharedMaterial;  
Material mat = mr.material;  
Debug.Log (smat == mr.sharedMaterial);  
Debug.Log (mat == mr.sharedMaterial);  
Debug.Log (mat == mr.material); 
```
 
打印结果分别为False,True,True。

我们知道sharedMaterial和material并不是变量，而是属性（property），属性有set和get方法，当给属性赋值时会隐式的调用set方法，当获取属性值得时候会隐式的调用get方法。在这里我们把material的值变量假设为\_material，而sharedMaterial的值变量假设为\_sharedMaterial。

1. Material \_material;  
2. Material \_sharedMaterial;  

由此，我们就可以得出结论：

material的get方法里面，会判断\_material跟\_sharedMaterial是否相同，如果相同，返回\_material，如果不同，会新建一个\_sharedMaterial的拷贝，并赋值给\_material和\_sharedMaterial。大概是下面这样的逻辑：

```c#
public Material material {  
    get {  
        if (\_sharedMaterial == \_material) {  
            return \_material;  
        }  
        \_material = new Material (\_sharedMaterial);  
        \_sharedMaterial = \_material;  
        return \_material;  
    }  
}  
```

我们继续做实验，重新写入下段代码：

```c#
MeshRenderer mr = GetComponent<MeshRenderer> ();  
Material newMat = new Material (Shader.Find ("Standard"));  
mr.material = newMat;  
Debug.Log (newMat == mr.sharedMaterial);  
Debug.Log (newMat == mr.material);  
Material tmpMat = mr.material;  
Debug.Log (newMat == tmpMat);  
Debug.Log (tmpMat == mr.sharedMaterial);  
```

打印结果分别为True,False,False,True。

由此，我们可以得出结论：

material的set方法里面会把传入的值赋给\_sharedMaterial，并且会再新建一个\_sharedMaterial的拷贝，并赋值给\_material。（因为值不同，所以get的时候又会新建一个Material。）

```c#
public Material material {  
    get {  
        if (_sharedMaterial == _material) {  
            return _material;  
        }  
        _material = new Material (_sharedMaterial);  
        return _material;  
    }  
    set {  
         _sharedMaterial = value;  
         _material = new Material (_sharedMaterial);  
     }  
}  
```

至于SharedMaterial的get和set方法，似乎并不会修改变量的值，只是单纯的取值赋值而已。

最后总结一下二者的使用时机：

当只修改材质的参数的时候，使用material属性，确保其他对象不会受影响。

当需要修改材质的时候，直接赋值给sharedMaterial，否则赋值给material会产生内存泄露。
