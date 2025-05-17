using Order.Entities;

namespace Order.DTOS
{
    /// <summary>
    /// 订单消息体
    /// </summary>
    public class OrderMessageDto
    {
        /// <summary>
        /// 订单表
        /// </summary>
        public CustomerOrder Order { get; set; }

        /// <summary>
        /// 订单表的扩展字段
        /// </summary>
        public CustomerOrderExtand Extend { get; set; }
    }
}
