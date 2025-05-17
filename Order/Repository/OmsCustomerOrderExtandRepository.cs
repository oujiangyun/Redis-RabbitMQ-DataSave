using Order.Entities;
using SqlSugar;

namespace Order.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class OmsCustomerOrderExtandRepository : IOmsCustomerOrderExtandRepository
    {
        private readonly ISqlSugarClient _db;

        public OmsCustomerOrderExtandRepository(ISqlSugarClient db)
        {
            _db = db;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="omsCustomerOrderExtands">要插入的实体列表</param>
        /// <returns>是否插入成功</returns>
        public async Task<bool> Insert(List<CustomerOrderExtand> omsCustomerOrderExtands)
        {
            var result = await _db.Insertable(omsCustomerOrderExtands).ExecuteCommandAsync();
            Console.WriteLine($"Insert 影响行数: {result}");
            return result > 0;
        }

    }
}
