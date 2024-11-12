using Store.DEMO.Core.Entites.Order;

namespace Store.DEMO.Core.Dtos.Orders
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PicureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}