using Newtonsoft.Json;
using Order.DTOS;
using Order.Entities;
using Order.Infrastructure;
using RabbitMQ.Client;
using System.Text;

namespace Order.Services
{
    /// <summary>
    /// 生产者
    /// </summary>
    public class OrderProducer : IOrderProducer
    {
        private readonly IModel _channel;

        public OrderProducer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _channel = factory.CreateConnection().CreateModel();
            _channel.QueueDeclare(queue: "order_queue", durable: true, exclusive: false, autoDelete: false);
        }

        /// <summary>
        /// 批量发送订单消息
        /// </summary>
        /// <param name="total">数量</param>
        /// <returns></returns>
        public async Task SendBatchOrders(int total)
        {
            int sentCount = 0;
            var orders = new List<OrderMessageDto>();
            for (int i = 0; i < total; i++)
            {
                var order = new CustomerOrder
                {
                    OrderId = SnowFlakeHelper.NextId(),                         // 唯一主键
                    CustomerId = 1,                                              // 客户ID（默认填 1）
                    WarehouseId = 1,                                             // 仓库ID（默认填 1）
                    ShopId = null,                                               // 可空
                    OrderNo = $"ORD-{SnowFlakeHelper.NextId()}",                          // 生成唯一订单号
                    ExtOrderNo = "",                                             // 外部订单号（空）
                    ExtAdditionalInfo = "",                                      // 外部附加信息（空）
                    OrderType = 1,                                               // 订单类型（如 1 表示普通订单）
                    OrderDate = DateTime.Now,                                    // 下单时间为当前时间
                    ShopName = "",                                               // 店铺名称（空）
                    ProductAmount = 0m,                                          // 金额相关默认为 0
                    FreightAmount = 0m,
                    TaxeAmount = 0m,
                    DiscountAmount = 0m,
                    TotalAmount = 0m,
                    Status = 0,                                                  // 默认状态为 0（待处理）
                    RefReceiveId = null,
                    IsRefStockOut = 0,                                           // 是否关联出库单，默认否（0）
                    RefStockOutIds = "",
                    ServiceFeeId = null,
                    Creator = 1,
                    CreateTime = DateTime.Now,
                    Modifier = null,
                    ModifierTime = null,
                    Channel = "",
                    Region = "",
                    ThirdRefId = "",
                    OrgOrderId = null,
                    MarketplaceId = "",
                    ExtOrderNo2 = "",
                    ExtAdditionalInfo2 = "",
                    HasBattery = false,
                    ECommercePostbackStatus = 0,
                    IsPush = false,
                    Vat = "",
                    Ioss = "",
                    Eori = "",
                    LastShippingDate = null,
                    LogisticsServiceName = "",
                    SystemOrderType = 1,
                    ForecastStatus = 1,
                    TaskId = ""
                };

                var extend = new CustomerOrderExtand
                {
                    OrderExtandId = SnowFlakeHelper.NextId(),
                    OrderId = order.OrderId,
                    StockOutId = SnowFlakeHelper.NextId()


                };

                orders.Add(new OrderMessageDto { Order = order, Extend = extend });


                if (orders.Count >= 1000)
                {
                    Publish(orders);
                    sentCount += orders.Count;
                    orders.Clear();
                }

            }
            if (orders.Any())
            {
                Publish(orders);
                sentCount += orders.Count;
            }

            Console.WriteLine($"发送消息统计: {sentCount}");




        }

        private void Publish(List<OrderMessageDto> orders)
        {
            var json = JsonConvert.SerializeObject(orders);
            var body = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish("", "order_queue", null, body);
        }
    }

    public static class SnowFlakeHelper
    {
        private static readonly SnowFlakeWorker _worker = new SnowFlakeWorker(1, 1);

        public static long NextId() => _worker.NextId();
    }

}
