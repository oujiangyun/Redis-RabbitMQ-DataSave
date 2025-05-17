using Order.DTOS;
using Order.Entities;

namespace Order.Services
{
    /// <summary>
    /// 订单消费者服务接口
    /// 处理订单消息和订单数据插入的核心服务
    /// </summary>
    public interface IOrderConsumerService
    {
        /// <summary>
        /// 异步处理订单消息
        /// </summary>
        /// <param name="orders">要处理的订单消息集合</param>
        /// <returns>表示异步操作的任务</returns>
        Task HandleMessageAsync(List<OrderMessageDto> orders);

        /// <summary>
        /// 异步批量添加订单及扩展信息
        /// </summary>
        /// <param name="newOrders">主订单数据集合</param>
        /// <param name="newExtends">订单扩展数据集合</param>
        /// <returns>表示异步操作的任务</returns>
        Task HandleInsert(List<CustomerOrder> newOrders, List<CustomerOrderExtand> newExtends);
    }
}
