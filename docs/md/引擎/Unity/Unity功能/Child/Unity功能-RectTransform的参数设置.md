# RectTransform的参数设置

1.改变RectTransform的top

GetComponent<RectTransform>().offsetMax = new Vector2(GetComponent<RectTransform>().offsetMax.x, top);

2.改变RectTransform的bottom

GetComponent<RectTransform>().offsetMin = new Vector2(GetComponent<RectTransform>().offsetMin.x, bottom);

3.改变RectTransform的width，height

GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

4.改变RectTransform的pos

GetComponent<RectTransform>().anchoredPosition3D = new Vector3(posx,posy,posz);

GetComponent<RectTransform>().anchoredPosition = new Vector2(posx,posy);

1.改变RectTransform的Left和Buttom
 
 
GetComponent<RectTransform>().offsetMax = new Vector2(left, top);
offsetMax是一个Vector2类型

offsetMax.x即为RectTransform中的Left

offsetMax.y即为RectTransform中的Buttom

2.改变RectTransform的Right和Top
GetComponent<RectTransform>().offsetMin = new Vector2(right, bottom);
offsetMin是一个Vector2类型

offsetMin.x即为RectTransform中的Right

offsetMin.y即为RectTransform中的Botttom

3.改变RectTransform的width，height
GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
sizeDelta是一个Vector2类型

sizeDelta.x即为RectTransform中的width

sizeDelta.y即为RectTransform中的height

4.改变RectTransform的pos
GetComponent<RectTransform>().anchoredPosition3D = new Vector3(posx,posy,posz);
//修改位置
GetComponent<RectTransform>().anchoredPosition = new Vector2(posx,posy);//修改Pivot位置
anchoredPosition3D:

anchoredPosition:

5.改变RectTransform的锚点
GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);