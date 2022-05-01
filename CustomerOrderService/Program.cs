using System;

namespace CustomerOrderService {
  class Program {
    static void Main(string[] args) {
      var customer = new Customer {
        CustomerId = 1,
        CustomerName = "George",
        CustomerType = CustomerType.Premium,
      };

      var order = new Order {
        OrderId = 1,
        ProductId = 212,
        ProductQuantity = 1,
        Amount = 150,
      };

      var customerOrderService = new CustomerOrderService();

      Console.WriteLine($"{customer.CustomerName} is a {customer.CustomerType} customer.");
      Console.WriteLine($"Order amount before discount: {order.Amount}");
      customerOrderService.ApplyDiscount(customer, order);
      Console.WriteLine($"Order amount after discount: {order.Amount}");
    }
  }
}
