CREATE TABLE `customer_order_extand` (
  `OrderExtandId` bigint NOT NULL COMMENT '订单表的扩展字段',
  `OrderID` bigint NOT NULL COMMENT '订单Id',
  `StockOutID` bigint NOT NULL COMMENT '出库Id',
  `PlatCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '平台识别',
  `PlatShopCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `PostbackMsg` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '回传错误信息提示',
  PRIMARY KEY (`OrderExtandId`) USING BTREE,
  UNIQUE KEY `oms_customer_order_extand_orderid` (`OrderID`) USING BTREE,
  UNIQUE KEY `oms_customer_order_extand_stockoutid` (`StockOutID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci ROW_FORMAT=DYNAMIC;
