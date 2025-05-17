namespace Order.Services
{
    /// <summary>
    /// 订单消息生产者接口
    /// 定义发送批量订单消息的能力
    /// </summary>
    public interface IOrderProducer
    {
        /// <summary>
        /// 发送批量订单消息
        /// </summary>
        /// <param name="total">要发送的订单总数</param>
        /// <returns>操作的任务</returns>

        Task SendBatchOrders(int total);
    }
}