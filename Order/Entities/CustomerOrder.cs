using SqlSugar;

namespace Order.Entities
{
    /// <summary>
    /// 订单表
    /// </summary>
    [SugarTable("customer_order")]
    public class CustomerOrder
    {
        [SugarColumn(ColumnName = "OrderID", IsPrimaryKey = true)]
        public long OrderId { get; set; }

        [SugarColumn(ColumnName = "CustomerID")]
        public long CustomerId { get; set; }

        [SugarColumn(ColumnName = "WarehouseID")]
        public long WarehouseId { get; set; }

        [SugarColumn(ColumnName = "ShopID", IsNullable = true)]
        public long? ShopId { get; set; }

        [SugarColumn(ColumnName = "OrderNo", Length = 100)]
        public string OrderNo { get; set; }

        [SugarColumn(ColumnName = "ExtOrderNo", Length = 200, IsNullable = true)]
        public string ExtOrderNo { get; set; }

        [SugarColumn(ColumnName = "ExtAdditionalInfo", Length = 999, IsNullable = true)]
        public string ExtAdditionalInfo { get; set; }

        [SugarColumn(ColumnName = "OrderType")]
        public int OrderType { get; set; }

        [SugarColumn(ColumnName = "OrderDate", IsNullable = true)]
        public DateTime? OrderDate { get; set; }

        [SugarColumn(ColumnName = "ShopName", Length = 100, IsNullable = true)]
        public string ShopName { get; set; }

        [SugarColumn(ColumnName = "ProductAmount", DecimalDigits = 4, IsNullable = true)]
        public decimal? ProductAmount { get; set; }

        [SugarColumn(ColumnName = "FreightAmount", DecimalDigits = 4, IsNullable = true)]
        public decimal? FreightAmount { get; set; }

        [SugarColumn(ColumnName = "TaxeAmount", DecimalDigits = 4, IsNullable = true)]
        public decimal? TaxeAmount { get; set; }

        [SugarColumn(ColumnName = "DiscountAmount", DecimalDigits = 4, IsNullable = true)]
        public decimal? DiscountAmount { get; set; }

        [SugarColumn(ColumnName = "TotalAmount", DecimalDigits = 4, IsNullable = true)]
        public decimal? TotalAmount { get; set; }

        [SugarColumn(ColumnName = "Status", IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(ColumnName = "RefReceiveID", IsNullable = true)]
        public long? RefReceiveId { get; set; }

        [SugarColumn(ColumnName = "IsRefStockOut")]
        public int IsRefStockOut { get; set; }

        [SugarColumn(ColumnName = "RefStockOutIDs", IsNullable = true)]
        public string RefStockOutIds { get; set; }

        [SugarColumn(ColumnName = "ServiceFeeID", IsNullable = true)]
        public int? ServiceFeeId { get; set; }

        [SugarColumn(ColumnName = "Creator")]
        public long Creator { get; set; }

        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "Modifier", IsNullable = true)]
        public long? Modifier { get; set; }

        [SugarColumn(ColumnName = "ModifierTime", IsNullable = true)]
        public DateTime? ModifierTime { get; set; }

        [SugarColumn(ColumnName = "Channel", Length = 200, IsNullable = true)]
        public string Channel { get; set; }

        [SugarColumn(ColumnName = "Region", Length = 50, IsNullable = true)]
        public string Region { get; set; }

        [SugarColumn(ColumnName = "ThirdRefID", Length = 255, IsNullable = true)]
        public string ThirdRefId { get; set; }

        [SugarColumn(ColumnName = "OrgOrderID", IsNullable = true)]
        public long? OrgOrderId { get; set; }

        [SugarColumn(ColumnName = "MarketplaceId", Length = 100, IsNullable = true)]
        public string MarketplaceId { get; set; }

        [SugarColumn(ColumnName = "ExtOrderNo2", Length = 255, IsNullable = true)]
        public string ExtOrderNo2 { get; set; }

        [SugarColumn(ColumnName = "ExtAdditionalInfo2", Length = 255, IsNullable = true)]
        public string ExtAdditionalInfo2 { get; set; }

        [SugarColumn(ColumnName = "HasBattery", IsNullable = true)]
        public bool? HasBattery { get; set; }

        [SugarColumn(ColumnName = "ECommercePostbackStatus", IsNullable = true)]
        public int? ECommercePostbackStatus { get; set; }

        [SugarColumn(ColumnName = "IsPush", IsNullable = true)]
        public bool? IsPush { get; set; }

        [SugarColumn(ColumnName = "Vat", Length = 255, IsNullable = true)]
        public string Vat { get; set; }

        [SugarColumn(ColumnName = "Ioss", Length = 255, IsNullable = true)]
        public string Ioss { get; set; }

        [SugarColumn(ColumnName = "Eori", Length = 255, IsNullable = true)]
        public string Eori { get; set; }

        [SugarColumn(ColumnName = "LastShippingDate", IsNullable = true)]
        public DateTime? LastShippingDate { get; set; }

        [SugarColumn(ColumnName = "LogisticsServiceName", Length = 32, IsNullable = true)]
        public string LogisticsServiceName { get; set; }

        [SugarColumn(ColumnName = "SystemOrderType")]
        public int SystemOrderType { get; set; }

        [SugarColumn(ColumnName = "ForecastStatus", IsNullable = true)]
        public int? ForecastStatus { get; set; }

        [SugarColumn(ColumnName = "TaskId", Length = 32, IsNullable = true)]
        public string TaskId { get; set; }
    }

}
