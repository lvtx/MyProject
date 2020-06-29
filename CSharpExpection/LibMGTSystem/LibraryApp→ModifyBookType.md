### 对书籍类型的增删改查
1. 首先从数据库中查找出的书籍类型然后绑定到DataGrid上  
   `1.查找类型的方法为bookTypeEntity.GetAllClients`  
   `2.DataGrid实例为dgrdShowBookType`
2. 添加新的书籍类型  
   `1.StatusBar添加一个"+"按钮，绑定New命令`  
   `2.在New_Executed中添加New命令的逻辑`  
   `3.单击"+"按钮执行完New命令后触发BookTypes_CollectionChanged事件继续执行后续逻辑`  
3. 删除书籍类型  
   `1.StatusBar添加一个"删除"按钮，绑定Delete命令`  
   `2.在Delete_Executed中添加Delete命令的逻辑`  
4. 修改书籍类型  
   `直接在DataGrid的Cell上修改`
5. 保存所有修改
   `1.StatusBar添加一个"保存"按钮，绑定Save命令`  
   `2.在Save_Executed中添加Save命令的逻辑`  
6. 显示状态  
   `定义一个Status依赖属性绑定到txtStatus控件显示状态`