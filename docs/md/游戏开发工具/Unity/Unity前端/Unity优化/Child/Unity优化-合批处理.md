# Unity优化-合批处理

## 一.静态合批

1. 标记为Batching Static的物体（标记后物体运行不能移动、旋转、缩放）
2. 在使用相同材质球的条件下
3. 在项目打包的时候unity会自动将这些物体合并到一个大Mesh
  
缺点

1. 打包后体积增大
2. 运行时内存占用增大

## 二.动态批处理

1. 不超过300个顶点
2. 不超过900个属性
3. 不包含镜像的scale缩放
4. 材质一样
5. 物体的lightmap指向的位置一样

缺点

1. 动态合批在降低drawcall的同时会额外的cpu性能消耗
  
合批中断情况

1. 使用多pass shader物体会禁用dynamic batching
2. 多个gameobject须共享同一材质
3. 一个gameobject接受多个光照会导致附加多个pass导致合批失败
  
## 三.GPU Instancing

1. 材质需要开启GPUInstancing
2. 同材质，同mesh的物体

缺点

1. 不支持skinnedmeshrenderer

合批中断情况

1. 缩放为负值
2. 代码改变材质变量不算同一个材质
3. 受限常量缓冲区在不同设备的大小上限，同批个数可能不同
4. 只支持一个实时光

## 四.SRP Batcher

1. shader种的变体一致

合批中断情况

1. 不支持粒子和蒙皮网格
2. shader变体不一致，如surface options不一致
3. 位置不相邻中间夹杂着不同shader或不同变体的其它物体

## 五.总结

1. 优先级顺序

SRP Batcher>静态合批>GPU Instancing>动态批处理

![1](\../Image/Unity优化-合批处理/1.png)

## 来源网站

**[萧寒大大](<https://blog.csdn.net/m0_46712616/article/details/129435129>)**
