# Unity组件-InputField

InputField组件：

Text Component（文本组件）：此输入域的文本显示组件，任何带有Text组件的物体。

Text（文本）：此输入域的初始值。

Character Limit（字符数量限制）：限定此输入域最大输入的字符数，0为不限制。

Content Type（内容类型）：限定此输入域的内容类型，包括数字、密码等，常用的类型如下：

Standard（标准类型）：什么字符都能输入，只要是当前字体支持的。

Integer Number（整数类型）：只能输入一个整数。

Decimal Number（十进制数）：能输入整数或小数。

Alpha numeric（文字和数字）：能输入数字和字母。

Name（姓名类型）：能输入英文及其他文字，当输入英文时自动姓名化（首字母大写）。

Password（密码类型）：输入的字符隐藏为星号。

Line Type（换行方式）：当输入的内容超过输入域边界时：

                single Line（单一行）：超过边界也不换行，继续延伸此行，输入域中的内容只有一行；

                multi Line Submit（多行）：超过边界则换行，输入域中内容有多行；

                multi Line Newline（多行）：超过边界则新建换行，输入域中内容有多行。

Placeholder（位置标示）：此输入域的输入位控制符，任何带有Text组件的物体。

注意：Placeholder对应的Text也为此输入框的提示语显示：（例如Enter text...为提示语，当输入框内容为空时，提示语可见，内容不为空时，提示语不可见）

Caret blink rate（光标闪烁速度）：标示输入光标的闪烁速度。

Hide mobile input（手机端隐藏输入）：这个没测试过有什么效果。

On Value Changed：值改变时触发消息。

End Edit：结束编辑时触发消息。
