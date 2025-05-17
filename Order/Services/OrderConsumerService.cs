using Order.DTOS;
using Order.Entities;
using Order.Repository;
using SqlSugar;
namespace Order.Services
{
    public class OrderConsumerService : IOrderConsumerService
    {
        private readonly ISqlSugarClient _db;
        private readonly RedisHelper _redis;
        private readonly IOmsCustomerOrderExtandRepository _omsCustomerOrderExtandRepository;
        private readonly IOmsCustomerOrderRepository _omsCustomerOrderRepository;

        public OrderConsumerService(ISqlSugarClient db, RedisHelper redis, IOmsCustomerOrderExtandRepository omsCustomerOrderExtandRepository, IOmsCustomerOrderRepository omsCustomerOrderRepository)
        {

            _db = db;
            _redis = redis;
            _omsCustomerOrderExtandRepository = omsCustomerOrderExtandRepository;
            _omsCustomerOrderRepository = omsCustomerOrderRepository;
        }



        /// <summary>
        /// 异步处理订单消息
        /// </summary>
        /// <param name="orders">要处理的订单消息集合</param>
        /// <returns>表示异步操作的任务</returns>
        public async Task HandleMessageAsync(List<OrderMessageDto> orders)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // 准备两个批量列表
            var newOrders = new List<CustomerOrder>();
            var newExtends = new List<CustomerOrderExtand>();

            foreach (var msg in orders)
            {
                string redisKey = $"order_lock:{msg.Order.OrderNo}";
                if (_redis.TryAcquireLock(redisKey, TimeSpan.FromHours(1))) // 先加锁
                {
                    newOrders.Add(msg.Order);
                    newExtends.Add(msg.Extend);
                }
                else
                {
                    Console.WriteLine($"订单已处理过：{msg.Order.OrderNo}");
                }
            }

            // 处理逻辑（入库）
            await HandleInsert(newOrders, newExtends);




            stopwatch.Stop();
            Console.WriteLine($"处理 {orders.Count} 条消息（有效 {newOrders.Count} 条）总耗时：{stopwatch.ElapsedMilliseconds} ms");
        }


        /// <summary>
        /// 异步批量添加订单及扩展信息
        /// </summary>
        /// <param name="newOrders">主订单数据集合</param>
        /// <param name="newExtends">订单扩展数据集合</param>
        /// <returns>表示异步操作的任务</returns>

        public async Task HandleInsert(List<CustomerOrder> newOrders, List<CustomerOrderExtand> newExtends)
        {
            int batchSize = 500;

            for (int i = 0; i < newOrders.Count; i += batchSize)
            {
                var orderBatch = newOrders.Skip(i).Take(batchSize).ToList();
                var extBatch = newExtends.Skip(i).Take(batchSize).ToList();

                // Redis key 检查是否已处理
                var insertableOrders = new List<CustomerOrder>();
                var insertableExtends = new List<CustomerOrderExtand>();
                var redisKeys = new List<string>();

                for (int j = 0; j < orderBatch.Count; j++)
                {
                    var order = orderBatch[j];

                    insertableOrders.Add(order);
                    insertableExtends.Add(extBatch[j]);

                }

                if (!insertableOrders.Any()) continue;

                var result = await _db.Ado.UseTranAsync(async () =>
                {
                    await _omsCustomerOrderRepository.InsertAsync(insertableOrders);
                    await _omsCustomerOrderExtandRepository.Insert(insertableExtends);
                });

                if (!result.IsSuccess)
                {
                    Console.WriteLine($"批次插入失败：{result.ErrorMessage}");
                }
                else
                {


                    Console.WriteLine($"成功插入一批数据，数量：{insertableOrders.Count}");
                }
            }
        }






    }

}
