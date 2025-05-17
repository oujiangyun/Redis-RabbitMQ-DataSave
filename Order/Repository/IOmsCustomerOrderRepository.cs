using Order.Entities;

namespace Order.Repository
{
    /// <summary>
    /// 客户订单仓储接口
    /// 定义客户订单数据访问操作
    /// </summary>
    public interface IOmsCustomerOrderRepository
    {
        /// <summary>
        /// 批量插入客户订单数据
        /// </summary>
        /// <param name="entity">要插入的客户订单集合</param>
        /// <returns>
        /// 表示异步操作的任务，任务结果为布尔值：
        /// true  - 插入成功
        /// false - 插入失败
        /// </returns>    
        Task<bool> InsertAsync(List<CustomerOrder> entity);
    }
}