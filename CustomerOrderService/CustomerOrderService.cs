namespace CustomerOrderService {
  public class CustomerOrderService {
    public void ApplyDiscount(Customer customer, Order order) {
      if(customer.CustomerType == CustomerType.Premium) {
        // 10% discount
        order.Amount = order.Amount - ((order.Amount * 10) / 100);
      } else if(customer.CustomerType == CustomerType.Special) {
        // 20% discount
        order.Amount = order.Amount - ((order.Amount * 20) / 100);
      }
    }
  }
}
