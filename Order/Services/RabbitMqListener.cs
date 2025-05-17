using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Order.DTOS;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Order.Services
{
    /// <summary>
    /// RabbitMQ 后台监听服务（基于 BackgroundService）
    /// </summary>
    public class RabbitMqListener : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection _connection;  // RabbitMQ 连接
        private IModel _channel;          // RabbitMQ 通道

        /// <summary>
        /// 构造函数
        /// </summary>
        public RabbitMqListener(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// 核心执行方法（后台服务启动时运行）
        /// </summary>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 初始化RabbitMQ连接
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            //  批量处理配置
            int batchSize = 500;                     // 每批处理500条
            TimeSpan maxWait = TimeSpan.FromSeconds(5); // 最长等待5秒（即使未满500条也处理）
            var batchBuffer = new List<OrderMessageDto>(batchSize);  // 消息缓冲区
            var batchLock = new object();            // 线程锁
            DateTime lastFlush = DateTime.UtcNow;    // 上次处理时间

            // 配置RabbitMQ
            _channel.BasicQos(0, (ushort)batchSize, false);
            _channel.QueueDeclare(
                queue: "order_queue",
                durable: true,       // 队列持久化
                exclusive: false,
                autoDelete: false);

            // 创建消费者
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    // 解析消息
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var orderList = JsonConvert.DeserializeObject<List<OrderMessageDto>>(json);

                    // 批量处理逻辑
                    bool shouldFlush = false;
                    lock (batchLock)
                    {
                        batchBuffer.AddRange(orderList);
                        // 达到批量大小时触发处理
                        if (batchBuffer.Count >= batchSize)
                        {
                            shouldFlush = true;
                        }
                    }

                    // 执行批量处理
                    if (shouldFlush)
                    {
                        List<OrderMessageDto> toProcess;
                        lock (batchLock)
                        {
                            toProcess = batchBuffer.Take(batchSize).ToList();
                            batchBuffer.RemoveRange(0, batchSize);
                            lastFlush = DateTime.UtcNow;
                        }


                        using var scope = _scopeFactory.CreateScope();
                        var consumerService = scope.ServiceProvider.GetRequiredService<IOrderConsumerService>();
                        await consumerService.HandleMessageAsync(toProcess);
                        Console.WriteLine($"[批量处理-数量触发] 已处理 {toProcess.Count} 条订单");
                    }

                    // 手动确认消息
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[RabbitMQ 消费错误] {ex.Message}");
                    // 消息处理失败，重新入队
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            // 定时刷新处理（防止少量消息长期积压）
            _ = Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    List<OrderMessageDto> toProcess = null;
                    lock (batchLock)
                    {
                        // 检查是否满足时间条件
                        if (batchBuffer.Count > 0 && DateTime.UtcNow - lastFlush >= maxWait)
                        {
                            toProcess = batchBuffer.ToList();
                            batchBuffer.Clear();
                            lastFlush = DateTime.UtcNow;
                        }
                    }

                    if (toProcess != null)
                    {
                        try
                        {
                            Console.WriteLine($"[批量处理-时间触发] 处理 {toProcess.Count} 条订单");
                            using var scope = _scopeFactory.CreateScope();
                            var consumerService = scope.ServiceProvider.GetRequiredService<IOrderConsumerService>();
                            await consumerService.HandleMessageAsync(toProcess);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[定时处理错误]: {ex.Message}");
                        }
                    }

                    await Task.Delay(1000, stoppingToken);  // 每秒检查一次
                }
            }, stoppingToken);

            // 开始消费
            _channel.BasicConsume(
                queue: "order_queue",
                autoAck: false,  // 关闭自动确认
                consumer: consumer);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}