# ET6.0基础部分-标签-BsonDictionaryOptions

BsonDictionaryOptions可以序列化和反序列化,并且可以网络传输,存储进数据库中,需要添加字典类型到数据库可以添加这个标签

```C#
[BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
```
