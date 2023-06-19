# Unity功能-CharacterController

角色控制器

```c#
CharacterController _controller;
_controller.Move(MoveDirection.normalized * Time.deltaTime * Speed); //不受重力影响
_controller.SimpleMove(MoveDirection.normalized * Speed);//受重力影响
```
