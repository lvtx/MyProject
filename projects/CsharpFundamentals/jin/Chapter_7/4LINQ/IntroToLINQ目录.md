### 1. 使用C#的逻辑运算符

```C#
        static void TestLogicalOperator()
        {
            var files = from fileName in Directory.GetFiles
                        ("D:\\windows\\files\\document\\ebook\\教材\\大学\\软件\\操作系统")
                        where (File.GetLastWriteTime(fileName) < DateTime.Now.AddDays(-1))
                        && Path.GetExtension(fileName).ToUpper() == ".PDF"
                        select new FileInfo(fileName);
            foreach (var file in files)
            {
                Console.WriteLine(file.Name);
            }
            Console.ReadLine();
        }
```

### 2.Distinct示例：消除重复项

```C#
        static void ListMemberNamesOfEnumerable()
        {
            MemberInfo[] members = typeof(Enumerable).
                GetMembers(BindingFlags.Static | BindingFlags.Public);
            var methods = (from method in members
                           select method.Name).Distinct();
            int count = 0;
            foreach (var method in methods)
            {
                count++;
                Console.WriteLine("{0}:{1}",method,count);
            }
        }
```

