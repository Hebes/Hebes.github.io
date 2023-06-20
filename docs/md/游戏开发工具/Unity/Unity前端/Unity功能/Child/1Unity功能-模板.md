# Unity功能-末尾显示省略号

Unity Text文字超框，末尾显示‘...’省略号

```C#
// 超框显示...
public static void SetTextWithEllipsis(this Text textComponent, string value)
{
    var generator = new TextGenerator();
    var rectTransform = textComponent.GetComponent<RectTransform>();
    var settings = textComponent.GetGenerationSettings(rectTransform.rect.size);
    generator.Populate(value, settings);
    var characterCountVisible = generator.characterCountVisible;
    var updatedText = value;
    if (value.Length > characterCountVisible)
    {
        updatedText = value.Substring(0, characterCountVisible - 3);
        updatedText += "…";
    }
    textComponent.text = updatedText;
}
```

调用方式：在给Text赋值的时候调用一次即可

```C#
text.SetTextWithEllipsis(titleDesc);
```
