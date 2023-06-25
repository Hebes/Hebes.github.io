# CSharp-params传递多个参数

C#开发语言中 params 是关键字，可以指定在参数数目可变处采用参数的方法参数。在函数的参数数目可变而执行的代码差异很小的时候很有用！

params关键字表示函数的参数是可变个数的,即可变的方法参数,例如Console.WriteLine( "{0},{1} "，i,j); 就像DELPHI 里WRITELN 函数一样,用于表示类型相同，但参数数量不确定.

在方法声明中的 params 关键字之后不允许任何其他参数，并且在方法声明中只允许一个 params 关键字。
关于参数数组，需掌握以下几点。

1. 若形参表中含一个参数数组，则该参数数组必须位于形参列表的最后；
2. 参数数组必须是一维数组；
3. 不允许将params修饰符与ref和out修饰符组合起来使用；
4. 与参数数组对应的实参可以是同一类型的数组名，也可以是任意多个与该数组的元素属于同一类型的变量；
5. 若实参是数组则按引用传递，若实参是变量或表达式则按值传递。
6. 用法：可变的方法参数，也称数组型参数，适合于方法的参数个数不知的情况，用于传递大量的数组集合参数；当使用

数组参数时，可通过使用params关键字在形参表中指定多种方法参数，并在方法的参数表中指定一个数组，形式为：方法修饰符

```C#
返回类型　方法名（params　类型[]　变量名）
{
   方法体
}
```

params使用代码演示

```C#
class Program
{
    static void Main(string[] args)
   {
      Sum(1,2,"a");
      Console.ReadKey();
   }
   static void Sum(params object[] numStack)
  {
      for (int i = 0; i < numStack.Length; i++)
      {
          Console.WriteLine(numStack[i]);
      }
　  }
}
输出结果：1 2 a
```
