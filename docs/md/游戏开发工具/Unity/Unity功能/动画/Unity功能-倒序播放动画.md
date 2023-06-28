# Unity功能-倒序播放动画

```C#
gameObject.animation["anim"].speed = -1; 
gameObject.animation["anim"].time = gameObject.animation["anim"].length;
gameObject.animation.Play("anim");
```
