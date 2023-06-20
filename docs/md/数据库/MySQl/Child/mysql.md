# mysql

```mysql

		mysql -uroot - p


										数据库相关



	#####查询所有数据库

			show databases; 显示所有已经存在的数据库

	####创建数据库

			-格式：create database 数据库名
			create database db1;   //db1都是随便的名字

	####查看数据库详情
			
			-格式：show create database 数据库名
			show create database db1;  //db1都是随便的名字
		
	####创建数据库指定字符集
			
			-格式：create database 数据库名 character set utf8; (utf8/gbk)
			create database db2 character set gbk;  //db2都是随便的名字
			
	####删除数据库
	
			-格式：drop database 数据库名
			drop database db2;  //db2都是随便的名字
	
	####使用数据库
	
			-格式：use 数据库名
			use db1;   //db1都是随便的名字

				
				
									表相关的//必须先使用数据库才能执行相关的sql



	####创建表
			
			-格式：create table 表名（字段1名	字段1类型,字段2名	字段2类型);  char(10)定长(固定长度)varchar（变长）
			create table person(name varchar(10),age int);//10表示字符长度
			create table t_user (id int auto_increment,username varcher(30),password varcher(30),age int,email varcher(50),primary key(id)) default charset=utf8;
	####查询所有表
		
			-格式：show tables; 
			show tables;

	####查询单个表
			
			-格式：show create table 表名;
			show create table person;

	####数据表的引擎

			-innodb:支持数据库的高级操作，如 事务，外键等；
			-myisam:只支持基础的增删改查；

	####创建表的时候指定引擎和字符集
	
			-格式：create table 表名（字段1名	字段1类型,字段2名 字段2类型) engine=(myisam/innodb) default charset=(gbk/utf8);
			engine=myisam charset=gbk; (myisam/innodb) (gbk/utf8);
create table t_user (id int auto_increment,name varchar(20) unique not null,password char(20) not null,age int,email varchar(50),primary key(id)) default charset=utf8;

	####查看表字段信息
		
			-格式：desc 表名；
			desc person；//person是自己打的

	####删除表
		
			-格式：drop table 表名；
			drop table person;

	###修改表名
	
			rename table 原名 to 新名；
			
	###修改引擎和字符集
		
			alter table stu engine=myisam charset=gbk;

	###添加表字段
	
			-最后添加：alter table 表名 add 字段名 字段类型；
			-最前面添加：alter table 表名 add 字段名 字段类型 first；
			-在XXX的后面添加：alter table 表名 add 字段名 字段类型 after 字段名；
			-测试在person最后面添加一个gender字符串类型，在最前面添加一个id整数；
				在name后面添加一个money;

	###删除字段
		
			-格式：alter table 表名 drop 字段名；
			alter table person drop money;

	####修改字段名称和类型

			-格式：alter table 表名 change 原字段名 新字段名 字段类型；
			alter table person change gender money int;

	####修改字段类型和位置
		
			-格式：alter table 表名 modify 字段名 字段类型 first/(after) XXX；
			alter table person modify money int after id;



			
									数据相关sql




	####准备相关数据
		
			create table student(id int,name varchar(10),age int,gender varchar(2));

	####插入数据
		
			insert into student values(1,'tom',18,'男');
			-全表格式：insert into 表名 values (值1,值2,值3);

			insert into student (id,name) values(2,'刘备');
			-指定字段格式：insert into 表名 values (字段1,字段2....) values(值1,值2....);

			insert into student values(9,'汤森',144,'男'),(8,'tom',30,'女'),(4,'sb',58,'男'),(6,'无空',16,'男');
			-批量插入：insert into 表名 values(值1,值2,值3)，(值1,值2,值3)，(值1,值2,值3)，(值1,值2,值3);
	
			insert into student (id,name,age) values (2,'刘备',20),(2,'刘备',20),(2,'刘备',20),(2,'刘备',20),(2,'刘备',20),(2,'刘备',20);
			-批量插入：insert into 表名 values(字段1,字段2....) values(值1,值2....),(值1,值2....),(值1,值2....),(值1,值2....);

	####查询数据
		
			select * from student;					//查询全部字段
			select name from student;				//查询全部名字字段
			select name,age from student;				//查询全部名字和年龄字段

	####条件查询
			
			select * from student where id<5;			//查询id小于5的
			select * from student where gender='男';		//查询性别是男的

			select 查询条件 from 表名 where 条件表达式;
			select ename,sal,comm from emp where sal>3000; 

	####如果window系统命令行写中文sql出错 则执行以下代码-----------------------------------------------------window限定
	
			set names gbk;

	####删除数据
	
			delete from student;					//清除表里面的所有内容
			delete from student where id=1;			//清除表里面的id=1的
			delete from student where age>20;			//清除表里面age>20的

	####修改数据
			
			update 表名 set 改变条件 where 选择条件；
			update student set age=500 where id=4(name=无空);	
			update student set age=15,gender='男' where id=4(name=无空);	//多项数据修改

			update 表名 set 改变条件;
			update student set age=10;						//全都的年龄都修改
		




										键约束





	####自增      
				
			1.自增会在曾经出现的最大值的基础上+1；
			2.自增数值不会因为删除数据减少；
			3.使用delete删除整个表的数据时，自增数值不会清零;

			primary key auto_increment-----------唯一,非空;

			create table t2(id int primary key auto_increment,name varchar(10));
				insert into t2 values(null,'Lucy');
				insert into t2 values(null,'Hanmeimei');
				insert into t2 values(10,'LiLei');
				insert into t2 values(null,'Lily');

	####注释-----------------创建表声明字段的时候可以通过添加comment给字段添加注释；
	
			comment
		
			create table t3(id int primary key auto_increment comment '这是主键字段',age int comment '这是年龄字段');
	
			show create table t3;				//查看


	####`和' 的区别
		
			` : 用来修饰表名和字段名的 可以省略
			create table `t4`(`id` int,`name` varchar(10));
			' : 用来修饰字符串
			
	####数据的沉余----------数据的重复----------拆分表的形式解决沉问题;
			
			create table item(id int primary key auto_increment,title varchar(10),price int,num int,cid int);
			create table category(id int primary key auto_increment,name varchar(10));
			
	####事务
			-数据库中执行sql语句的工作单元，此工作单元不可拆分，能够保证全部成功或全部失败
			-事务的ACID特性：
				Atomicity:原子性,最小不可拆分，保证全部成功，全部失败;
				Consistency:一致性,从一个一致状态到另一个一致状态
				Isolation:隔离性，多个事务之间互不影响
				Durability:持久性，事务完成后数据提交到数据库文件中 持久保存;			

			create table person(id int primary key auto_increment,name varchar(10),money int);

				insert into person values(null,'超人',50);
				insert into person values(null,'钢铁侠',30000);

				update person set money=5050 where id=1;
				update person set money=25000 where id=2;

	####查看数据库的提交状态
	
			show variables like '%autocommmit%';


	####设置自动提交开关----------0：关闭   1：开启
	
			set autocommmit=0/1;

	#### 手动提交

			commit-----提交

	####事务回滚
	
			rollback;--------此指令会将数据库中的数据回滚到上此提交的点;


	####保存回滚点
		
			savepoint s1;
			
			rollback to s1;-----------使用s1这个回滚点;




											sql分类
		
			
			
			
	####DDL:Date Definition Language (数据定义语言);
		
			-包括：creat,drop,alter,truncate
			-不支持事务的

			####truncate
	
					-删除表并且创建一个相同的空表，此时表中的自增数值清零;
					-格式：truncate table 表名;
					-truncate:删除表创建新表 drop：删除表 delete:只删除数据     效率：drop>truncate>delete;

	####DML：Date Manipulation Language (数据操作语言)

			-包括：insert,update,delete,select(DQL)
			-支持事务
	
	####DQL：Date Query Language (数据查询语言)
	
			-只包括select
			
	####TCL:Transaction Control Language (事务控制语言)
	
			-包括：commit,rollback,savepoint,rollback to
		
	####DCL:Date control Language (数据控制语言)
	
			-用来分配用户权限相关的sql




										数据库的数据类型
	
		
	####整型
	
			-常用类型：int(m)  bigint(m)--------m代表显示长度
	
			create table a(num int(10) zerofill);
			insert into a values(132);
			select * from a;

	####浮点型
	
			-常用类型：double(m,d)-------m代表总长度，d代表小数类型       76.232  m=5  d=3
			  	 decimal(m,d)------超高精度小数

	####字符串
		
			-char：固定长度			char(10)		'abc'	占10	执行效率高	最大255
			-varchar:长度可变			varchar(10)		'abc'	占3	节省空间	最大65535---------用的多
			-text：长度可变，用于保存大文本
	
	####日期
			-date：只保存年月日
			-time：只保存时分秒
			-datetime：保存年月日时分秒，默认值为null,最大值 9999-12-31
			-timestamp：保存年月日时分秒,保存距离1970年1月1日的毫秒数，默认值为当前时间,最大值2038年1月19号
			
			create table d(d1 date,d2 time,d3 datetime,d4 timestamp);
				insert into d values('2018-04-23',null,null,null);
	
				insert into d values(null,'12:38:45','2018-05-12 12:38:33',null);

	####导入数据
		
			-学生机(linux)
				source /home/soft01/桌面/tables.sql;
			-个人电脑(windows)
				source c:/tables.sql;
	
	####is null 和 is not null
		
			1.查询有没有奖金的员工信息
				select * from emp where comm is null;
			2.查询有奖金的员工信息
				select * from emp where comm is not null;

	####别名
		
			select ename from emp;
			select ename as '名字' from emp;			//起别名
			select ename '名字' from emp;				//起别名
			select ename 名字 from emp;				//起别名

	####去重复查询
			
			distinct
			
			select distinct job from emp;
			
	####比较运算符 
			
			>
			<
		 	>=
	 		<=
		 	=
		 	!=和<>			//不等于
			
	####and和or
		
			-and相当于java中的&&
			-or相当于java中的||

				1.查询工资小于2000并且时10号部门的员工信息
					select * from emp where sal<2000 and deptno=10;
				2.查询有奖金comm或者工资大于30000的员工姓名,工资,奖金
					select ename,sal,comm from emp where comm is not null or sal>3000; 
				3.查询t_item表中的单价price大于500并且库存num大于100的商品信息		
					select * from item where price>500 and num>100;

	####in
			-查询某个字段的值等于u多个值的时候使用in关键字

			1.查询工资为800,3000,5000,1600的所有员工信息
				select * from emp where  sal=500 or sal=3000 or sal=5000 or sal=1600;
				select * from emp where sal in (800,3000,5000,1600);	

	####between 

			-select * from 表名 where 参数 beteen 参数值 and 参数值;								
				2.查询员工工资在2000至4000之间的员工信息
					select * from emp where sal between 2000 and 4000;
			
	####like 
		
			-用来模糊查找
			-_:代表单个未知字符
								第一个字符a 				 a_
								第二个字符a				_a%
								倒数第三个字符是a			%a__
								包含a					%a%
							
			-%：代表0或者多个位置字符           
								
								 以a开头      a%       abc   aasdjklqwj    都可以匹配
								 以a结尾	    %a

			-like相关案例
					
				1.查询所有记事本价格(title中包含记事本)
					select title,price from t_item where title like '%记事本%';
				2.查询单价低于100的记事本信息
					select title from t_item where price<100 and title like '%记事本%';
				3.查询单价低于50到200之间的得力商品(title包含得力)
					select * from t_item where price between 50 and 200 and title like '%得力%';
				4.查询有图片的得力商品
					select * from t_item where image is not null and title like '%得力%';
				5.查询有赠品的商品信息(sell_point字段包含 赠)
					select * from t_item where sell_point and title like '%赠%';
				6.查询不包含得力的商品
					select title from t_item where not title like '%得力%';
				7.查询价格介于50到200之间之外的记事本信息
					select * from t_item where price not between 50 and 200 and title like '%记事本%';

	####排序
			
			order by
				-order by写在where条件的后面，如果没有where写在表名后面
				-by后面写排序的字段名称
				-默认是升序，也可指定升序降序：asc 升序	desc降序

				select ename,sal from where emp order by sal;				//升序
				select ename,sal from where emp order by sal desc;			//降序

			-案例：	
				1.查询员工姓名和工资 按照工资降序排序
					select ename,sal from where emp order by sal;
				2.查询所有的dell商品按照价格的降序排序
					select * from t_item where title like '%dell%' order by price;
				3.查询所有员工的姓名，工资，部门编号 按照部门标号升序排序
					select ename,sal,deptno from emp where order by deptno; 
				4.查询所有员工的姓名，工资，部门编号 按照部门标号升序排序,工资降序--------两个字段的升降序
					select ename,sal,deptno from emp where order by deptno,sal desc; 
				
	####分页查询
			
			limit
				-limit 跳过条数，每页条数;			算法 ： (跳过的条数-1)*每页的条数,每页的条数
				-limit 通常写在sql语句的最后面
			
			1.查询所有商品，按照单价升序，查询第二页 每页7条数据
				select * from t_item order by pricr limit 7,7;
			2.查询工资排名的前三名的三位员工的信息
				select * from emp order by sal desc limit 0,3;
			3.查询拿最低工资的员工姓名和编号
				select ename,empno from emp order by sal limit 0,1;
			4.查询商品表的记事本,第三页 每页两条数据
				seleect * from t_itme whhere title '%记事本%' limit 4,2; 

	####数值计算
			
			+
			-
			*
			/	
			%等于mod(x,y)
			
			1.查询员工的姓名，工资，年终奖	(年终奖=月薪*5)
				select ename,sal,sal*5 年终奖 from emp;
			2.查询商品单价，库存和总金额	(单价*库存)
				select price,num,price*num 总金额 from t_item;
			3.修改所有员工的工资，每人涨薪20元
				update emp set sal=sal+20;
		
	####日期相关函数
			
			-SQL的helloword					select 'helloword';
			-获得当前时间	年月日 时分秒	now()			select now();
			-获得单前年月日					select curdate();
			-获得当前的时分秒					select curtime();
			-从年月日时分秒中提取年月日				select date(now());
			-从年月日时分秒中提取时分秒				select time(now());		//里面的now()可以写字段名称
			-从年月日时分秒中提取时间分量	年，月，日，时，分，秒
				年：select extract(year from now());
				月：select extract(month from now());
				日：select extract(day from now());
				时：select extract(hour from now());
				分：select extract(minute from now());
				秒：select extract(second from now());
			-日期和时化						date_format(日期，格式);		
				四位年 %Y		两位年 %y
				两位月 %m		一位月 %c
				日 %d
				24小时 %H		12小时 %h
				分钟 %i
				秒 %s
				1.把now() 格式改成 年月日 时分秒
					select date_format(now(),'%Y年%m月%d日 %H时%i分%s秒');
			-把非标准格式转回标准格式				str_to_date(非标准格式的字符串，格式);
				1.把14.08.2018 09：10：20 转回标准格式
					select str_to_date('14.08.2018 09:10:20','%d.%m.%Y %H:%i:%s');
	
	####ifnull函数
			
			-age=ifnull(x,y)			如果x的值为null则age=y,如果不为null则age=x;
			
			1.修改员工表的奖金，如果奖金值为null修改成0,不为null则不变
				update emp set comm=ifnull(comm,0);
				

	####聚和函数
			
			-对多行数据进行统计 ： 求和 秋平均值 最大值 最小值 统计数量

			-求和		sum(字段名)

							1.求10号部门工资总和
								select sum(sal) from emp where deptno=10;
							2.查询单价在100以内的商品库存总量
								select sum(num) from t_item where price<100;
			
			-平均值	avg(字段名);

							1.查询20号部门的平均工资
								select avg(sal) from emp where deptno=20;
							2.查询记事本的平均价格
								select avg(price) from t_item where title like '%记事本%';

			-最大值	max(字段名)	最小值		min(字段名)
							1.查询所有员工的最大工资和最小工资
								select max(sal),min(sal) from emp;

			-统计数量	count(字段名)	一般使用count(*)----------不能有null值不然不能统计进去
							1.统计工资在2000以下的员工有多少人
								select count(*) from emp where sal<2000; 	

	####字符串相关函数
			
			-字符串拼接		
					-格式：concat(a1,a2)
						select concat('abc','mm');
							1.查询员工姓名和工资 在工资后面添加'元'字
								select ename,concat(sal,'元') from emp;		
				
			-获取字符串的长度	
					-格式：char_length('abc');
							1.查询员工姓名和姓名的长度
								select ename,char_length(ename) from emp;	
				
			-获取一个字符串在另一个字符串中出现的位置			
					-格式：instr('abcdefg','d')-----从1开始
						select instr('abcdefg','d');
					-格式：locate('d','abcdefg');
						select locate('d','abcdefg');

			-插入字符串		
					-格式:insert(str,start,length,newstr);
						select insert('abcdfg',3,2,'m');	
			
			-转大小写
					-格式：upper(str);
					-格式:lower(str);
						select upper('Nba'),lower('CBA');
			
			-从左边截取和从有边截取
					-格式：left(str,count);	
						select left('abcdefg',2);
					-格式：right(str,count);					
						select right('abcdefg',2);
							
			-截取字符串
					-格式：substring(str,start);
						select substring('abcdefg',2);
					-格式：substring(str,start,end);
						select substring('abcdefg',2,3);

			-去字符串两端的空白
					-格式：trim(str)------去空白只是去两端的
						select trim('      	a    b      ');
			
			-重复 
					-格式：repeat(str,count);
						select repeat('ab',2);
			
			-替换					
					-格式：replace(str,old,new);
						select replace('abcdefgcc','c','k');
	
			-反转
			
					-格式：reverse(str)
						select reverse('abcdefg');
	
	####数学相关函数
			
			-向下取整	
					-格式：floor()
						select floor(3.14);
			
			-四舍五入
					-格式：round()
						select round(3.8);

			-四舍五入指定小数位数
					-格式：round()
						select round(3.825525,2);
		
			-非四舍五入
					-格式：truncate()
						select truncate(3.2853,2);
			
			-随机数	
					-格式:rand()	-----	0-1不包含1
						select rand()
							0-8的随机数
								select floor(rand()*9);
							3-5的随机数
								select floor(rand()*3+3);



									分组查询
		
		
	
	####分组查询 
			
			limit 
			
			-group by SQL中的位置
				select * from 表名 where ...... group by .....having..... order by ....limit...;
		
			1.查询每个部门的最高工资
				select max(sal) from emp group by deptno;
			2.查询每个职位的平均工资
				select job,avg(sal) from emp group by job;
			3.查询每个领导手下的人数
				select mgr,count(*) from emp where mgr is not null group by magr; 
			4.查询每个部门每个领导下的人数
				select deptno,mgr,count(*) from emp where mgr is not null group by deptno,magr;
			5.查询emp表中每个部门的编号，人数，工资总和，最后根据人数进行升序排列，如果人数一致，根据工资总和降序排列。	
				select deptno,count(*) c,sum(sal) s from emp group by deptno order by c,s desc;
			6.查询工资在1000~3000之间的员工信息，每个部门的编号，平均工资，最低工资，最高工资，根据平均工资进行升序排列。
				select deptno,avg(sal) a,max(sal),min(sal) from emp where sal between 1000 and 3000 group by deptno order by a;
			7.查询含有上级领导的员工，每个职业的人数，工资的总和，平均工资，最低工资，最后根据人数进行降序排列，如果人数一致，根据平均工资进行升序排列
				select job,count(*) c,sum(sal),avg(sal) a,min(sal) from emp where mgr is not null group by job order by c desc,a;
			8.查询每个部门的平均工资，要求查询平均工资大于2000
				-以下时错误写法：where后面不能写聚合函数
				select deptno,avg(sal) a from emp where a>2000;
				-正确写法：
				select deptno,avg(sal) a from emp group by deptno having a>2000;

	####having-----过滤	
			
			-having要和gronup by 结合使用，写在group by 的后面，用于在SQL语局中添加聚合函数的条件
			-where后面写普通条件，having后面写聚合函数的条加	
			
			1.查询emp表中每个部门的平均工资高于2000的部门编号,部门人数，部门工资,最后根据平均工资排序降序
				select deptno,avg(sal) a,count(*) from emp group by deptno having a>2000 order by a desc;
			2.查询商品表中每个分类的平均单价，要求显示平均单价低于100的信息
				select catengory_id,avg(price) a from t_item group by catengory_id having a<100;
			3.查询每个分类对应的库存数量，显示高于199999的库存数量
				select catengory_id,sum(num) s from t_item group by catengory_id having s>199999;
			4.查询emp表中工资在1000-3000之间的员工，每个部门的编号。工资总和，平均工资，过滤掉平均工资低于2000的部门信息，按照平均工资升序排序
				select deptno,sum(sal),avg(sal) a from emp where sal between 1000 and 3000 gronup by deptno and havinga>=2000 prder by a;
			5.查询emp表中名字不时以s开头，每个职位的人数，工资总和，最高工资，过滤掉平均工资高于3000的职位，根据人数升序排序，如果一致则更具工资总和降序排序
				select job,count(*) c,sum(sal) s,max(sal) from emp where ename not like 's%' group by job having by avg(sal)<=3000 order by 				c,s.desc;
			6.(提高题)查询每年入职的人数
				select extract(year from hiredate) year,count(*) from emp group by year;

	####子查询(嵌套查询)
			
			-子查询可以写在where或者having后面当查询条件的值
			-写在创建表的时候
				create table t_emp as (select * from emp where sal<2000);		//创建新的表格sal小于2000
			-可以写在from后面,把查询结果当成一个虚拟的表
				select ename,sal from (select * from emp where sal<2000) e;		//e-----别名必须写	
				

			select * from 表名 where 字段=(select 聚合函数 from 表名)
			
			1.查询emp表中最高工资的员工信息
				select * from emp where sal=(select max(sal) from emp);
			2.查询工资高于20号部门平均工资的所有员工信息
				select * from emp where sal>(select avg(sal) from emp where deptno=20);
			3.查询和jones做相同工作的
				select * from emp where job=(select job from emp where ename='jones') and ename!='jones';
			4.查询最悲惨员工(工资最低)的同事们的信息
				select * from emp where deptno=(select deptno from emp where sal=(select min(sal) from emp)) 
				and sal!=(select mmin(sal)from emp);
			5.查询最后入职的员工信息
				select * from emp where hiredata=(select max(hiredate) from emp);		
			6.查询King这个哥们的部门名称是什么(需要使用dept表)
				select dname from dept where deptno=(select deptno from emp where ename='King');
			7.查询名字中不包含a并且工资高于10号部门平均工资的员工信息
				select * from emp where ename not like '%a%' and sal>(select avg(sal) from emp where deptno=10);
		####in-----------类似包含概念
			8.查询有员工的部门详情(需要用到部门表)
				select * from dept where deptno in(select distinct deptno from emp);
			9.扩展题(难度最高)查询平均员工工资最高的部门信息	
				select * from dept where deptno in(select deptno from emp group by deptno having avg(sal)=(select avg(sal) a from emp grout 				by deptno order by a desc limit 0,1));
			
	####关联查询
			
			-同时查询多张表的数据称为关联查询
			
			1.查询每个员工的姓名和所占部门的名称
				select e.ename,d.dname
				from emp e,dept d
				where e.deptno=d.dtptno;
			2.查询每个员工的姓名、工资、部门名称、部门地点
				select e.ename,e.sal,d.dname,d.loc
				from emp e,dept d
				where e.deptno=d.deptno;
				
			3.查询在纽约工作的所有员工信息
				select e.* 
				from emp e,dept d
				where e.deptno=d.deptno and d.loc='new york';

	####笛卡尔积
			
			-如果关联查询不写关联关系，则会得到两张表数据的乘积，这个乘积称为笛卡尔积
			-笛卡尔积是一个错误的执行结果，在工作切记不要出现，数据如果太大可以导致内存溢出
		
	####等值连接和内连接
			
			-等值连接：	select * from a,b where a.x=b.x and a.y=XXXX;
			-内连接：	select * from a join b on a.x=b.x where a.y=XXXX;--------------以后主要使用的内连接
		
			1.查询在纽约工作的所有员工信息
				select e.* 
				from emp e join dept d 
				on e.deptno=d.deptno
				where d.loc='new york';

	####外连接
			
			-左外连接：以join左边的表为主表，查询所有数据，右边的表只显示有关系的数据
			-右外连接：以join右边的表为主表，查询所有数据，左边的表是显示有的数据			

			-左外连接：select * from a letf join b on a.x=b.x where a.y=XXXX;		
				select * 
				from emp e left join dept d 
				on e.deptno=d.deptno;
			-右外连接：select * from a right join b on a.x=b.x where a.y=XXXX;		另一种右外连接：
				select * 											select * 
				from emp e right join dept d 								from dept d left join emp e
				on e.deptno=d.deptno;									on e.deptno=d.deptno;

	####关联查询的总结
			
			-等值连接、内连接、外连接都是关联查询的查询方式，使用哪一种取决具体业务需求
				1.如果查询两张表的交集数据使用，等值连接或内连接(推荐)
				2.如果查询一张表全部数据另外一张是交集数据则使用外连接，左外或右外都可以




									表设计之关联关系
	
		
		
	####一对一
		
			-什么是一对一：ab两张表，a表中的一条数据对应b表中   的一条数据，同时b表的一条数据对应a表中的一条数据，此时两张表的关系称为一对一关系
			-主键是表示数据唯一性的，外键是用来建立关系的
			-如何让两张表关联关系？在从表中添加一个外键指向主表的主键
			-练习：
				user: id int,username varchar(10),password varchar(10)
				userinfo:nick varchar(10),qq varchar(10),phone varchar(15) uid int
				
				insert into user values(1,'libai','admin'),(2,'liubei','123456');
				insert into userinfo values('李白','66668888','13838383388',1),('刘备','3334444','13333333333',2);
			保存以下数据
				1.查询李白的用户名和密码
					select u.username,u.paseord
					from user u join userinfo ui
					on u.id=ui.uid
					where ui.nick='李白';
				2.查询每个用户的用户名和密码
					select u.username,ui.nick
					from user u join userinfo ui
					on u.id=ui.uid;
				3.查询liubei的所有数据
					select *
					from user u join userinfo ui
					on u.id=ui.uid
					where u.username='lliubei';

	####一对多
			
			-什么是一对多：ab两张表 a表中的一条数据对应b表中的多条,同时b表中的一条数据对应a表中的一条,此时两张表的关系为一对多
			-如何建立关系：在两张表中 多的表中  添加外键  指向另外一张表的  主键
			练习：
				create database db5;
				create table emp(id int primary key auto_increment,name varchar(10),age int,sal int,deptid int) engine=myisam charset=utf8;
				create table dept(id int primary key auto_increment,name varchar(10),loc varchar(10));
				insert into emp values(null,'白骨精',28,3000),(null,'蜘蛛精',32,2000),(null,'钢铁侠',45,8888),(null,'美国队长',252,6000),(null,'路费',18,100),(null,'那每',20,500);
				insert into dept values(null,'妖怪部','盘丝洞'),(null,'英雄部','美国'),(nul,'海贼部','日本');	
			1.查询每个员工的姓名 工资和部门名称
				select e.name,e.sal,d.name
				from emp e join dept d
				on e.deptid=d.id;
			2.查询路费的工资和工作地点
				select e.name,d.loc
				from emp e join dept d
				on e.deptid=d.id;
				where e.name='路费';
			3.查询英雄部门的所有员工信息
				select e.*
				from emp e join dept d
				on e.deptid=d.id
				where d.name='英雄部';

	####多对多
			
			-什么是多对多：ab两张表:a表中一条数据对应b表中对条数据，同时b表中一条数据对应a表中的对条数据，称为多对多
			-如何建立建立关系
				create table student(id int primary key auto_increment,name varchar(10));
				create table teacher(id int primary key auto_increment,name varchar(10));
				create table t_s(tid int,sid int);
				insert into teachaer values(null,'传奇老师'),(null,'苍老师');
				insert into student values(null,'刘德华'),(null,'张学友'),(null,'小明'),(null,'小红');
				insert into t_s(1,1),(1,2),(2,1),(2,2),(2,3),(2,4);
			1.查询每个学生对应的老师姓名
				select s.name,t.name
				from student s join t_s ts
				on s.id=ts.sid
				from t_s ts join teacher t
				on ts.tid=t.id;
			2.查询刘德华的老师姓名
				select t.name
				from student s join t_s ts
				on s.id=ts.sid
				from t_s ts join teacher t
				on ts.tid=t.id
				where s.name='刘德华';
			3.查询苍老师的学生信息
				select s.name
				from student s join t_s ts
				on s.id=ts.sid
				from t_s ts join teacher t
				on ts.tid=t.id
				where t.name='苍老师';

	####自关联
		
			-什么是自关联：当前表的外键指向自己表的主键这种称为自关联
			-应用场景：用于保存有层级关系，并且不确定有多是多少层的数据
			-可以保存一对一或者一对多的数据
		
	####连接方式和关联关系的区别
			
			-连接方式：包括等值连接、内连接、外连接，是指关联查询的查询方式	
			-关联关系：指表设计时两张表之间存在的逻辑关系包括 一对一、一对多
	
	####表设计案例：权限管理
		
			1.创建三张主表，用户表user:id,name 角色表role:id,name 权限表module:id,name
				create table user(name varchar(10));
				create table role(name varchar(10));
				create table module(name varchar(10));
			2.创建2张关系表：用户～角色关系表u_r:uid,rid 角色～权限关系表r_m:rid,mid
				create table u_r(uid int,rid int);
				create table r_m(rid int,mid int);
			3.用户表插入：刘德华、张学友、凤姐
				insert into user values('刘德华'),('张学友'),('凤姐');
			4.权限表插入：男浏览、男评论、男发帖、男删帖、女浏览、女评论、女发帖、女删帖
				insert into module values('男浏览'),('男评论'),('男发帖'),('男删帖'),('女浏览'),('女评论'),('女发帖'),('女删帖');
			5.角色表插入：男会员、男管理员、女游客、女会员
				insert into role values('男会员'),('男管理员'),('女游客'),('女会员');
			6.保存以下关系：男会员对应权限：男浏览、男评论、男发帖；男管理员对应权限：男浏览、男评论、男发帖、男删帖；
			  女游客对应权限：女浏览；女会员对应权限：女浏览、女评论、女发帖；
				insert into r_m values(1,1),(1,2),(1,3),(2,1),(2,2),(2,3),(2,4),(3,5),(4,6),(4,7),(4,8);
			7.用户和角色保存以下关系 刘德华 男会员，张学友 男管理员，凤姐 女会员和男会员。
				insert into r_m values(1,1),(2,2),(3,4),(3,1);
			8.查询刘德华所拥有的权限名称
				1.子查询
				-得到刘德华的id
				-select id from user whwere name='刘德华';
				-通过用户id得到所对应的角色id
				-select rid from u_r where uid=(select id from user whwere name='刘德华');
				-通过角色id找到对应的权限id
				-select mid from r_m where rid in(select rid from u_r where uid=(select id from user whwere name='刘德华'));
				-通过权限id得到权限的名字
				-select name from moduie where id in(select mid from r_m where rid in(select rid from u_r where uid=(select id from user 					 whwere name='刘德华')));
				-2.内连
				sleect m.name
				from user u join u_r ur
				on u.id=ur.uid
				from role r join u_r ur
				join r_m rm
				on rm.rid=ur.rid
				where u.name='刘德华'; 
				
			9.查询拥有男浏览权限的用户有哪些
				
				leect u.name
				from user u join u_r ur
				on u.id=ur.uid
				from role r join u_r ur
				join r_m rm
				on rm.rid=ur.rid
				where m.name='男浏览';
	
			10.查询每个用户拥有的权限名称

				leect u.name,m.name
				from user u join u_r ur
				on u.id=ur.uid
				from role r join u_r ur
				join r_m rm
				on rm.rid=ur.rid;
				
	####面试题
		
			1.创建交易流水表	trade(id,time,money,type,pid);
				cerate table trade(id int primary key auto_increment,time date,money int,type varchar(10),pid int);
			2.创建人物表		person(id,name,gender,rel);
				create table person(in int primary key auto_increment,name varchar(10),gender varchar(10),rel varchar(10));
			测试数据：
				-刘德华 男 亲戚	收 现金 500，	给他发了50 现金
				-杨幂 女 亲戚		收100	给他发了2000	微信
				-马云	男	同事	收50000	给他发了10	支付宝
				-特朗普 男	朋友	收1000	给他发了100	微信
				-貂蝉	女	朋友	收0	给他发了20000	现金
				insert into person values(null,'刘德华','男','亲戚'),
							(null,'杨幂','女','亲戚'),
							(null,'马云','男','同事'),
							(null,'特朗普','男','朋友'),
							(null,'貂蝉','女','朋友');

				insert into trade values(null,2018-3-10,500,'现金',1),
								(null,2018-3-10,-50,'现金',1),
								(null,2018-3-12,100,'微信',2),
								(null,2018-3-13,-2000,'微信',2),
								(null,2018-3-14,50000,'支付宝',3)，
								(null,2018-3-15,-10,'支付宝',3)，
								(null,2018-3-15,1000,'微信',4)，
								(null,2018-3-16,-100,'微信',4)，
								(null,2018-3-20,-20000,'现金',5);

				1.统计2018年2月15号到现在的所有红包收益
					select sum(money) from trade where time>str_to_date('2018年2月15','%Y年%c月%号');
				2.查询2018年2月15号到现在 金额大于100 所有女性亲戚的名字和金额		
					select p.name,t.money
					from trade t join person p
					on t.pid=p.id
					where time>str_to_date('2018年2月15','%Y年%c月%号')
					and t.money not between -100 and 100
					and p.gender='女'
					and p.rel='亲戚';
				3.查询三个平台分别收入的红包金额
					select type,sum(money) from trade where money>0 group by type;


	####视图
			
			-数据库中存在的表和视图都是其内部的对象，视图可以理解成时一个虚拟的表，数据来自原表，视图本质上就是取代了一段SQL语句
			-为什么时用视图：因为有些数据查询的SQL语句比较长，每次书写比较麻烦，使用视图可以起到SQL语句重用的作用，提高开发效率，可以隐藏敏感信息
			
			-格式：create view 视图名 as 子查询;
				-创建10号部门员工的视图
				create  view v_emp_10 as (select * from emp where deptno=10);
				-创建没有工资的员工信息
				create  view v_emp_nosal as (select ename,job,deptno,mgr from emp);		//隐藏了工资敏感信息
			练习：
				1.创建部门为20号部门并且工资小于3000的视图
					cerate view v_emp_20 as (select * from where deptno=20 and sal<3000);
				2.创建每个部门平均工资，工资总和，最大工资，最小工资的视图
					create view v_emp_sal as (select  deptno,avg(sal),sum(sal),max(sal),min(sal)from emp group by deptno);
		
	#####视图分类
			
			-简单视图：创建视图的子查询中，不包含去重、函数、分组、关联查询的视图称为简单视图，简单视图可以为对称数据进行增删改查操作
			-复杂视图：包含去重、函数、分组、关联查询的视图称为复杂视图，一般只进行查询操作
	
	####对视图中的数据进行增删改查
			
			-视图的操作方式和表的方式一样

			1.插入数据

				insert into v_emp_10 (empno,ename,deptno) values(10001,'钢铁侠',10);
				insert into v_emp_10 (empno,ename,deptno) values(10002,'葫芦娃',20);	//原表中有  视图中没有 称为视图污染
			-网视图中插入一条 视图中不可见但是在原表中存在的数据--------称为数据污染
			-解决数据污染：在创建视图的时候添加 with check option 关键字
				create view v_emp_30 as (select * from emp where deptno=30) with check option;
				insert into v_emp_30 (empno,ename,deptno) values(10003,'蜘蛛侠',20);//报错
				insert into v_emp_30 (empno,ename,deptno) values(10003,'喜喜好',30);//不会报错
			
			2.修改数据

				-只能修改视图中存在的数据
				update v_emp_30 set ename='兼并写' where empno=10004;
				update v_emp_30 set ename='葫芦娃' where empne=10001;

			3.删除数据
					
				-只能删除视图中存在的数据
				delete from v_emp_30 where empne=10004;//视图中存在
				delete from v_emp_30 where empne=10001;//视图中不存在
				
	####修改视图
			
			-格式：create or replace view 视图名 as 子查询;
				create or replace view v_emp_20 as (select * from emp);
	
	###删除视图
			
			drop view v_emp_20;

	####视图别名
			
			-如果创建视图的时候使用了别名，则后面的各种操作只能使用别名
			create view v_emp_20 as (select ename name,sal from emp);
			update v_emp_20 set name='aaaa' where name='钢铁侠';	//成功
			update v_emp_20 set ename='aaaa' where name='钢铁侠';	//失败
	
	####约束	
		
			-什么是约束：约束是给表字段添加的限制条件
			
			####非空约束--------not null
				-限制字段的只不能为null
					create table t1(id int,age int not null);
					insert into t1 values(1,2);		//成功
					insert into t1 values(2,null);		//失败

			####唯一约束--------unique
				-限制字段的值不能重复
					create table t2(id int,age int unique);
					insert into t2 values(1,20);
					insert into t2 values(2,20);
			
			####主键约束 --------primary key
				-限制字段值唯一并且非空
				-创建表时添加
					table t_pri(id int primary key auto_increment);
				-创建表后添加主键约束
					create table t_pri2(id int);
					alter table t_pri2 add primary key(id);
			
			####删除主键约束
				alter table t_pri2 drop primary key;
				查看下：desc t_pri2;

			####默认约束
				-给字段添加默认值，当字段不赋值的时候，此字段的值生效
					create table t2(id int,age int default 20);
					insert into t3 values(1,88);
					insert into t3 (id) values(2);
					insert into t3 values(3,null);
			
			####检查约束-------check		
				-mysql中没有效果，但是语法不报错
					create table t4(id int,age int check(age>10));
				
			####外键约束	
				-字段的值可以为null可以重复，但是不能是不存的值
				-被依赖的数据不能先删除
				-被依赖的表不能先删除	
				
				-测试外键约束
					1.创建部门表
						create table dept(id int primary key auto_increment,name varchar(10));
						create table emp(id int primary key auto_increment,name varchar(10),deptid int,constraint
							fk_dept foreign key(deptid) references dept(id));
				-格式：在创建表最后一个字段后面添加 constraint 约束名 foreign key (外键字段名称) references 被依赖的表名(被依赖的字段名);
				
				-插入数据
				insert into dept values(null,'神仙'),(null,'妖怪');
					-测试
					insert into emp values(null,'悟空',1);			//成功
					insert into emp values(null,'赛亚人',3);		//失败
					delete from dept where id=1;				//失败  被关联的数据

	####索引
			
			-什么是索引：索引是数据库中用来提高查询效率的技术，类似于目录
			-为什么使用索引：如果不使用索引，查询数据数据时会依次遍历每一个保存数据的磁盘块，直到找到目标数据为止，使用索引后，磁盘块会以树状结构保存，查询数据时
					会大大降低磁盘的访问量，从而提高查询效率
			
			-索引越多越好吗？
				索引会占用储存空间，只对常用的查询字段创建索引
			-有索引一定好吗？
				如果数据量小的话，添加索引反而会降低查询效率

			-索引的分类
				1.聚集索引(聚簇索引)：通过主键的创建的索引称为聚集索引，一个表只能有一个聚集索引，添加了主键约束的表会自动创建聚集索引，聚集索引的树状结构保存了数据
				2.非聚集索引：通过非主键字段创建的索引称为非聚集索引，一个表面可以有多个，树状结构中不保存数据，只保存指针(磁盘块地址)
			
	####导入索引测试数据
			
			1.linux系统	把文件解压到桌面
				在终端中执行
					source /home/soft01/桌面/item_backup.sql;
			2.window系统	把文件解压到c盘根目录
				在命令行中执行
					source c:/item_backup.sql;

			show tables;					//看是否有item2
			select count(*) from item2;			//查看是否时172万多条数据
			select * from item2 where title='100';	//看查询时间时多少  1.15秒

	####如何创建聚集索引
				
				-格式：create index 索引名 on 表明(字段名(长度));
					-给title字段添加索引
					create index index_item2_title on item2(title);
					-再次查看时间
					select * from item2 where title='100';//0.04秒
		
	####查看索引
			
			show index from item2;
		
	####删除索引
		
			-格式：drop index 索引名称 on 表名;
			drop index index_item2_title on item2;
	
	####复合索引
		
			-通过多个字段创建的索引称为复合索引
			-应用场景：频繁使用某个字段作为查询条件的时候，可以为这几个字段创建复合索引
			-创建	标题和价格的索引
				create index index_item2_title_price on item2(title,price);

	


每个部门的人数,根据人数排序
	select deptno,count(*) a from emp group by deptno order by a desc;
每个部门中，每个主管的手下人数
	select deptno,mgr,count(*) from emp where mgr is not null group by deptno,magr;
每种工作的平均工资
	select job,avg(sal)from emp group by job; 
每年的入职人数
	select extract(year from hiredate) year,count(*) from emp group by year;
少于等于3个人的部门信息
	select * from dept where depton in(select count(*) a from group by deptno a<=3);
拿最低工资的员工信息
	select * from emp where sal=(select min(sal) from emp);
只有一个下属的主管信息
	select * from emp where empno in(select mgr from emp mgr is not null gorup by mgr having count(*)=1);
平均工资最高的部门编号
	select deptno from emp group by deptno having avg(sal)=(select avg(sal) a from emp group by deptno order by a desc limit 0,1);
下属人数最多的人，查询其个人信息
	
拿最低工资的人的信息
	select deptno from emp where sal=(select min(sal) from emp);
最后入职的员工信息
	select * from emp where hiredata=(select max(hiredate) from emp);
工资多于平均工资的员工信息
查询员工信息，部门名称
	select e.*,d.dname
	from e.emp,dept d
	where e.deptno=d.deptno;
员工信息，部门名称，所在城市
	select e.*,d.dname,d.loc
	from emp e,dept d
	where e.deptno=d.deptno;
DALLAS 市所有的员工信息
	select e.*
	from emp e,dept d
	on e.deptno=d.deptno 
	where d.loc='DALLAS';
按城市分组，计算每个城市的员工数量
	select d.loc,count(ename)
	from emp e right join dept d
	on e.deptno=d.deptno group by d.loc;
查询员工信息和他的主管姓名
	select e.ename,m.ename
	from emp e join emp m
	on e.mgr=m.empno;
员工信息，员工主管名字，部门名
	select e.ename,m.ename,d.dname
	from emp e join emp m 
	on e.mgr=m.empno
	join dept d
	on e.deptno=d.deptno;
员工和他所在部门名
	select e.*,d.ename
	from emp e join dept d
	on e.deptno=d.deptno;
案例：查询emp表中所有员工的姓名以及该员工上级领导的编号，姓名，职位，工资
	select e.ename,m.emptno,m.ename,m.job,m.sal
	from emp e left join emp m
	on e.mgr=m.empno;
案例：查询emp表中名字中没有字母'K'的所有员工的编号，姓名，职位以及所在部门的编号，名称，地址
	select e.empno,e.ename,e.job,d.*
	from emp e join dept d
	on e.deptno=d.deptno
	where e.ename not like '%k%';
案例：查询dept表中所有的部门的所有的信息，以及与之关联的emp表中员工的编号，姓名，职位，工资
	select d.*,e.empno,e.ename,e.job,e.sal
	from emp e right join dept d
	on e.deptno=d.deptno;
```