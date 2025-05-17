using SqlSugar;

namespace Order.Entities
{
    /// <summary>
    /// 订单表的扩展字段
    /// </summary>
    [SugarTable("customer_order_extand")]
    public class CustomerOrderExtand
    {
        [SugarColumn(ColumnName = "OrderExtandId", IsPrimaryKey = true, ColumnDescription = "订单表的扩展字段")]
        public long OrderExtandId { get; set; }

        [SugarColumn(ColumnName = "OrderID", ColumnDescription = "订单Id")]
        public long OrderId { get; set; }

        [SugarColumn(ColumnName = "StockOutID", ColumnDescription = "出库Id")]
        public long StockOutId { get; set; }

        [SugarColumn(ColumnName = "PlatCode", Length = 20, IsNullable = true, ColumnDescription = "平台识别")]
        public string PlatCode { get; set; }

        [SugarColumn(ColumnName = "PlatShopCode", Length = 50, IsNullable = true)]
        public string PlatShopCode { get; set; }

        [SugarColumn(ColumnName = "PostbackMsg", Length = 255, IsNullable = true, ColumnDescription = "回传错误信息提示")]
        public string PostbackMsg { get; set; }
    }
}
