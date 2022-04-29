using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CustomerOrderService.Tests {
  [TestFixture]
  public class CustomerOrderServiceTests {
    #region Tests
    [Test]
    public void ApplyDiscount_WhenBasicCustomer_DoesNotApplyDiscount() {
      // Arrange
      Customer basicCustomer = new Customer {
        CustomerId = 1,
        CustomerName = "George",
        CustomerType = CustomerType.Basic,
      };

      Order order = new Order {
        OrderId = 1,
        ProductId = 212,
        ProductQuantity = 1,
        Amount = 150,
      };

      CustomerOrderService customerOrderService = new CustomerOrderService();

      // Act
      customerOrderService.ApplyDiscount(basicCustomer, order);

      // Assert
      Assert.AreEqual(150, order.Amount);
    }

    [Test]
    public void ApplyDiscount_WhenPremiumCustomer_Applies10PercentDiscount() {
      // Arrange
      Customer premiumCustomer = new Customer {
        CustomerId = 1,
        CustomerName = "George",
        CustomerType = CustomerType.Premium,
      };

      Order order = new Order {
        OrderId = 1,
        ProductId = 212,
        ProductQuantity = 1,
        Amount = 150,
      };

      CustomerOrderService customerOrderService = new CustomerOrderService();

      // Act
      customerOrderService.ApplyDiscount(premiumCustomer, order);

      // Assert
      Assert.AreEqual(135, order.Amount);
    }

    [Test]
    public void ApplyDiscount_WhenSpecialCustomer_Applies20PercentDiscount() {
      // Arrange
      Customer specialCustomer = new Customer {
        CustomerId = 1,
        CustomerName = "George",
        CustomerType = CustomerType.Special,
      };

      Order order = new Order {
        OrderId = 1,
        ProductId = 212,
        ProductQuantity = 1,
        Amount = 150,
      };

      CustomerOrderService customerOrderService = new CustomerOrderService();

      // Act
      customerOrderService.ApplyDiscount(specialCustomer, order);

      // Assert
      Assert.AreEqual(120, order.Amount);
    }
    #endregion

    #region TestCase
    [TestCase(CustomerType.Premium, 150, 135)]
    [TestCase(CustomerType.Special, 150, 120)]
    public void ApplyDiscount_WhenCustomerIsNotBasicCustomer_AppliesDiscount(CustomerType customerType, decimal orderAmount, decimal expectedAmountAfterDiscount) {
      // Arrange
      Customer customer = new Customer {
        CustomerId = 1,
        CustomerName = "George",
        CustomerType = customerType,
      };

      Order order = new Order {
        OrderId = 1,
        ProductId = 212,
        ProductQuantity = 1,
        Amount = orderAmount,
      };

      CustomerOrderService customerOrderService = new CustomerOrderService();

      // Act
      customerOrderService.ApplyDiscount(customer, order);

      // Assert
      Assert.AreEqual(expectedAmountAfterDiscount, order.Amount);
    }
    #endregion

    #region TestCaseSource
    [TestCaseSource(nameof(CustomerOrderServiceTestCases))]
    public void ApplyDiscount_WhenPassedCustomerAndOrder_AppliesDiscountBasedOnCustomerType(CustomerOrderServiceTestCase testCase) {
      // Arrange
      CustomerOrderService customerOrderService = new CustomerOrderService();

      // Act
      customerOrderService.ApplyDiscount(testCase.Customer, testCase.Order);

      // Assert
      Assert.AreEqual(testCase.ExpectedAmountAfterDiscount, testCase.Order.Amount);
    }
    #endregion

    #region CustomerTestCases
    private static IEnumerable<CustomerOrderServiceTestCase> CustomerOrderServiceTestCases() {
      return new List<CustomerOrderServiceTestCase> {
        new CustomerOrderServiceTestCase {
          Customer = new CustomerTestCase(CustomerType.Basic),
          Order = new OrderTestCase(100),
          ExpectedAmountAfterDiscount = 100,
        },
        new CustomerOrderServiceTestCase {
          Customer = new CustomerTestCase(CustomerType.Basic),
          Order = new OrderTestCase(150),
          ExpectedAmountAfterDiscount = 150,
        },
        new CustomerOrderServiceTestCase {
          Customer = new CustomerTestCase(CustomerType.Premium),
          Order = new OrderTestCase(100),
          ExpectedAmountAfterDiscount = 90,
        },
        new CustomerOrderServiceTestCase {
          Customer = new CustomerTestCase(CustomerType.Premium),
          Order = new OrderTestCase(150),
          ExpectedAmountAfterDiscount = 135,
        },
        new CustomerOrderServiceTestCase {
          Customer = new CustomerTestCase(CustomerType.Special),
          Order = new OrderTestCase(100),
          ExpectedAmountAfterDiscount = 80,
        },
        new CustomerOrderServiceTestCase {
          Customer = new CustomerTestCase(CustomerType.Special),
          Order = new OrderTestCase(150),
          ExpectedAmountAfterDiscount = 120,
        },
      }.AsEnumerable();
    }
    #endregion

    #region CustomerOrderServiceTestCase classes
    public class CustomerOrderServiceTestCase {
      public Customer Customer { get; set; }
      public Order Order { get; set; }
      public decimal ExpectedAmountAfterDiscount { get; set; }
      public CustomerOrderServiceTestCase() {
        Customer = new Customer {
          CustomerId = 1,
          CustomerName = "Bill",
          CustomerType = CustomerType.Basic,
        };
        Order = new Order {
          OrderId = 1,
          ProductId = 212,
          ProductQuantity = 1,
          Amount = 100,
        };
        ExpectedAmountAfterDiscount = 100;
      }
      // Important! Override ToString( ) so tests will show in Test Explorer
      public override string ToString() {
        return $"{Customer.CustomerType}, {Order.Amount}, {ExpectedAmountAfterDiscount}";
      }
    }

    public class CustomerTestCase : Customer {
      public CustomerTestCase(CustomerType customerType) {
        CustomerId = 1;
        CustomerName = "Bill";
        CustomerType = customerType;
      }
    }

    public class OrderTestCase : Order {
      public OrderTestCase(decimal amount) {
        OrderId = 1;
        ProductId = 212;
        ProductQuantity = 1;
        Amount = amount;
      }
    }
    #endregion
  }
}
