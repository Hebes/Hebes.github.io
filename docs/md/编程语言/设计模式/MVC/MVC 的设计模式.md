# MVC 的设计模式

https://zhuanlan.zhihu.com/p/81085291
d

## MVC 是什么？它是如何工作的？我们来解剖它

在本节课中我们要讨论的内容：

+   什么是 MVC？
+   它是如何工作的？

## 什么是 MVC

![](https://pic1.zhimg.com/v2-dc0d4008db85b3f5028618e524bf4c04_b.png)

MVC 由三个基本部分组成 - 模型（Model），视图（View）和控制器（Controller）。 它是用于实现应用程序的**用户界面层**的架构设计模式。 一个典型的实际应用程序通常具有以下层：

+   用户展现层
+   业务逻辑处理层
+   数据访问读取层
+   MVC 设计模式通常用于实现应用程序的用户界面层。

## MVC 如何工作

让我们了解 MVC 设计模式是如何与案例一起工作的。 假设我们想要查询特定学生的详细信息（即 ID 为 1 的学生信息），并在 HTML 表格中的网页上显示这些详细信息，如下所示。

![](https://pic4.zhimg.com/v2-0ace67e2c6e42f7f6de41a33e8db044f_b.png)

那么，从 Web 浏览器我们发出请求，URL 地址如下所示:

```text
http://52abp.com/student/details/1
```

![](https://pic4.zhimg.com/v2-c0f8ea102b68be933cf0ad8461caec93_b.png)

上图的意思如下：

+   当我们的请求到达服务器时，作为 MVC 设计模式下的 Controller，会接收请求并且处理它。
+   Controller 会创建模型（Model），该模型是一个类文件，会进行数据的展示。
+   在 Molde 中，除了数据本身，Model 还包含从底层数据源（如数据库）查询数据后的逻辑信息。
+   除了创建 Model 之外，控制器还选择 View 并将 Model 对象传递给该 View。
+   视图仅负责呈现 Modle 的数据。
+   视图会生成所需的 HTML 以显示模型数据，即 Controller 提供给它的学生数据。
+   然后，此 HTML 通过网络发送，最终呈现在发出请求的用户面前。

## Model （模型)

因此，在当前案例中 Model，是由 Student 类和管理学生数据的 StudentRepository 类组成，如下所示。

```text
public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
    }

 public interface IStudentRepository
    {
        Student GetStudent(int id);
        void Save(Student student);
    }

public class StudentRepository : IStudentRepository
    {
        public Student GetStudent(int id)
        {
            //写逻辑实现 查询学生详情信息
            throw new NotImplementedException();
        }

        public void Save(Student student)
        {
            //写逻辑实现保存学生信息
            throw new NotImplementedException();
        }
    }
```

我们使用**Student**类来保存学生数据，而**StudentRepository 类则负责查询并保存学生信息到数据库中。 如果要概括 model 的话，它就是 MVC 中用于包含一组数据的类和管理该数据的逻辑信息。** 表示数据的类是 Student 类，管理数据的类是 StudentRepository 类。

如果您想知道我们为什么使用`IStudentRepository`接口。 我们不能只使用没有接口的**StudentRepository**类。

但是其实我们是可以的，但是我们使用接口的原因，是因为接口，允许我们使用依赖注入，而依赖注入则可以帮助我们创建**低耦合且易于测试的系统**。 我们将在即将发布的视频中详细讨论**依赖注入**。

## View -视图

MVC 中的 View 应该只包含显示 Controller 提供给它的 Model 数据的逻辑。您可以将视图视为 HTML 模板。假设在我们的示例中，我们希望在 HTML 表中显示**Student**数据。

这种情况下的视图会和**Student**对象一起提供。 **Student**对象是将学生数据传递给视图的模型。 视图的唯一作用是将学生数据显示在 HTML 表中。 这是视图中的代码。

```text
@model StudentManagement.Model.Student

<!DOCTYPE html>
<html>
  <head>
    <title>学生页面详情</title>
  </head>
  <body>
    <table>
      <tr>
        <td>Id</td>
        <td>@model.Id</td>
      </tr>
      <tr>
        <td>名字</td>
        <td>@model.Name</td>
      </tr>
      <tr>
        <td>班级</td>
        <td>@model.ClassName</td>
      </tr>
    </table>
  </body>
</html>
```

在 MVC 中，View 仅负责呈现模型数据。 视图中不应该有复杂的逻辑。 视图中的逻辑必须非常少而且要小，并且它也必须仅用于呈现数据。 如果到达表示逻辑过于复杂的点，请考虑使用**ViewMode**l 或**View Component**。 **View Components**是此版本 MVC 中的新增功能。 我们可以在以后的课程中讨论它。

## Controller 控制器

当来自浏览器的请求到达我们的应用程序时，作为 MVC 中的控制器，它处理传入的 http 请求并响应用户的操作。

在这种情况下，用户已向 URL 发出请求（/ student/ details/1），因此该请求被映射到**StudentController**中的**Details**方法，并向其传递**Student**的 ID，在本例中为 1. 此映射为 由我们的 web 应用程序中定义的路由规则完成。

> 我们将在即将发布的视频中详细讨论 [http://ASP.NET](http://asp.net/) Core MVC 中的路由。  

```text
public class StudentController:Controller
    {
        private IStudentRepository _studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public IActionResult Details(int id)
        {
            Student model = _studentRepository.GetStudent(id);
            return View(model);
        }
    }
```

如您所见，从**Details**方法中的代码，控制器将生成模型，在这种情况下，Model 是**Student**对象。 要从基础数据(如数据库)源检索**Student**数据，控制器使用**StudentRepository**类。

一旦控制器使用所需数据构造了**Student**模型对象，它就会将该**Student**模型对象传递给视图。 然后，视图生成所需的 HTML，以显示 Controller 提供给它的**Student**数据。 然后，此 HTML 通过网络发送给发出请求的用户。

如果这一点令人困惑，或者无法理解，请不要担心，我们将通过为我们的应用程序，会在后面一步步的创建模型，视图和控制器来实现这一目标，我们将在此过程中进行更加清晰和明确。

## 小结

MVC 是用于实现应用程序的用户界面层的架构设计模式

+   模型(Model)：包含一组数据的类和管理该数据的逻辑信息。
+   View（视图）：包含显示逻辑，用于显示 Controller 提供给它的模型中数据。
+   Controller（控制器）:处理 Http 请求，调用模型，请选择一个视图来呈现该模型。

正如您所看到的，在 MVC 设计模式中，我们可以清楚地分离各个关注点，让他们各司其职。 每个组件都有一个非常具体的任务要做。 在我们的下一个视频中，我们将讨论在我们的 [http://asp.net](http://asp.net/) core 应用程序中设置 MVC 中间件。