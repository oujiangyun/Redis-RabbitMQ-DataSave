using Order.Entities;
using SqlSugar;

namespace Order.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class OmsCustomerOrderRepository : IOmsCustomerOrderRepository
    {
        private readonly ISqlSugarClient _db;

        public OmsCustomerOrderRepository(ISqlSugarClient db)
        {
            _db = db;
        }
        /// <summary>
        /// 批量插入客户订单数据
        /// </summary>
        /// <param name="entity">要插入的客户订单集合</param>
        /// <returns>
        /// 表示异步操作的任务，任务结果为布尔值：
        /// true  - 插入成功
        /// false - 插入失败
        /// </returns>    
        public async Task<bool> InsertAsync(List<CustomerOrder> entity)
        {
            try
            {
                var result = await _db.Insertable(entity).ExecuteCommandAsync();
                Console.WriteLine($"Insert 影响行数: {result}");
                return result > 0;

            }
            catch (Exception ex)
            {
                return false;

            }
        }
    }
}
