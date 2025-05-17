```markdown
# Redis-RabbitMQ-DataSave
一个用rabbitMQ+redis 实现的处理高并发入库的小demo
场景是，假如有N条订单消息，推来。该如何处理入库。
这里用rabbitMQ作为缓冲，用redis来作为去重，分块批量入库的小demo 。
1、数据库为MYSQL,数据库表的文件在SQL里面，先拿去创建好表
2、安装好 rabbitMQ\redis
```


