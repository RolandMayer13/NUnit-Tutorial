namespace CustomerOrderService {
  public class Customer {
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public CustomerType CustomerType { get; set; }
  }

  public enum CustomerType {
    Basic,
    Premium,
    Special
  }
}
