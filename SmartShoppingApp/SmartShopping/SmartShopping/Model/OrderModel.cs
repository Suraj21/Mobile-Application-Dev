using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShopping.Model
{
    public class OrderModel
    {
        public string OrderId { get; set; }

        public string UserId { get; set; }

        public int orderStatus { get; set; }

        public PaymentDetails PaymentDetails { get; set; }

        public List<Product> products { get; set; }

    }

    public class PaymentDetails
    {
        public string PaymentId { get; set; }

        public string PaymentMode { get; set; }

        public string PaymentStatus { get; set; }

        public decimal TotalSellprice { get; set; }

    }

    public class Product
    {
        public int ProductId { get; set; }

        public string ProductDescription { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SellPrice { get; set; }

        public decimal Discount { get; set; }

        public string ModifiedDate { get; set; }

        public decimal Quantity { get; set; }

    }
}
