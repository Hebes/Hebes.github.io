# Unity功能-物体移动

## 移动一

[【Unity】如何优雅地移动物体-8个方法](<https://blog.csdn.net/GG_and_DD/article/details/126917358>)

## 移动二

```c#
//更改坐标移动
transform.position += transform.forward * Time.deltaTime * speed;
//坐标位移
transform.Translate(transform.forward * Time.deltaTime * speed);
//刚体移动
rigidbody.MovePosition(transform.position + tranform.forward * Time.deltaTime * speed);
//刚体初速度
rigidbody.velocity = transform.forward * speed;
//带重力移动
characterController.SimpleMove(Vector3);
//不带重力移动
characterController.Move(Vector3);
//固定速度移动至目标,接近时减速
transform.position = Vector3.MoveTowards(position1, position2, speed * Time.deltaTime);
//迅速接近,之后再变为设定的速度
transform.position = Vector3.Lerp(position1, position2, speed * Time.deltaTime);
```
