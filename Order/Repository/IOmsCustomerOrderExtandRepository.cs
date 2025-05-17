using Order.Entities;

namespace Order.Repository
{
    public interface IOmsCustomerOrderExtandRepository
    {
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="omsCustomerOrderExtands">要插入的实体列表</param>
        /// <returns>是否插入成功</returns>
        public Task<bool> Insert(List<CustomerOrderExtand> omsCustomerOrderExtands);
    }
}
