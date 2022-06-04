using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountService.Tests {
  // https://methodpoet.com/unit-testing-with-moq/

  [TestFixture]
  public class AccountServiceTests {
    private MockRepository mockRepository;

    private Mock<IBookService> mockBookService;
    private Mock<IEmailSender> mockEmailSender;

    [SetUp]
    public void SetUp() {
      this.mockRepository = new MockRepository(MockBehavior.Strict);

      this.mockBookService = this.mockRepository.Create<IBookService>();
      this.mockEmailSender = this.mockRepository.Create<IEmailSender>();
    }

    private AccountService CreateService() {
      return new AccountService(
          this.mockBookService.Object,
          this.mockEmailSender.Object);
    }

    [Test]
    public void GetAllBooksForCategory_WhenPassedCategoryId_ReturnsCollectionOfAvailableBooks() {
      // Arrange
      var service = this.CreateService();
      string categoryId = "UnitTesting";
      var availableBooks = new List<string> {
        "The Art of Unit Testing",
        "Test-Driven Development",
        "Working Effectively with Legacy Code"
      };

      mockBookService
        .Setup(bookService => bookService.GetBooksForCategory(categoryId))
        .Returns(availableBooks);

      // Act
      var result = service.GetAllBooksForCategory(
        categoryId);

      // Assert
      Assert.IsNotEmpty(result);
      Assert.AreEqual(3, result.Count());
      CollectionAssert.AreEqual(availableBooks, result);
      this.mockRepository.VerifyAll();
    }

    [Test]
    public void GetBookISBN_WhenBookIsFound_ReturnsISBN() {
      // Arrange
      var service = this.CreateService();
      string categoryId = "UnitTesting";
      string searchTerm = "The Art of Unit Testing";
      var availableBooks = new List<string> {
        "The Art of Unit Testing",
        "Test-Driven Development",
        "Working Effectively with Legacy Code"
      };
      string expectedResult = "0-9020-7656-6";

      mockBookService
        .Setup(bookService => bookService.GetBooksForCategory(categoryId))
        .Returns(availableBooks);
      mockBookService
        .Setup(bookService => bookService.GetISBNFor(searchTerm))
        .Returns(expectedResult);

      // Act
      var result = service.GetBookISBN(
        categoryId,
        searchTerm);

      // Assert
      Assert.AreEqual(expectedResult, result);
      this.mockRepository.VerifyAll();
    }

    [Test]
    public void GetBookISBN_WhenBookIsNotFound_ReturnsEmptyString() {
      // Arrange
      var service = this.CreateService();
      string categoryId = "UnitTesting";
      string searchTerm = "Missing Book";
      var availableBooks = new List<string> {
        "The Art of Unit Testing",
        "Test-Driven Development",
        "Working Effectively with Legacy Code"
      };

      mockBookService
        .Setup(bookService => bookService.GetBooksForCategory(categoryId))
        .Returns(availableBooks);

      // Act
      var result = service.GetBookISBN(
        categoryId,
        searchTerm);

      // Assert
      Assert.IsEmpty(result);
      this.mockRepository.VerifyAll();
    }

    [Test]
    public void SendEmail_WhenPassedEmailAddressAndBookTitle_SendsEmail() {
      // Arrange
      var service = this.CreateService();
      string emailAddress = "test@gmail.com";
      string bookTitle = "Test - Driven Development";
      string expectedSubject = "Awesome Book";
      string expectedBody = $"Hi,\n\nThis book is awesome: Test - Driven Development.\nCheck it out.";

      string actualSubject = String.Empty;
      string actualBody = String.Empty;

      //mockEmailSender
      //  .Setup(emailSender => emailSender.SendEmail(emailAddress, expectedSubject, expectedBody))
      //  .Verifiable();
      mockEmailSender
        .Setup(emailSender => emailSender.SendEmail(emailAddress, It.IsAny<string>(), It.IsAny<string>()))
        .Callback<string, string, string>((to, subject, body) => {
          actualSubject = subject;
          actualBody = body;
        })
        .Verifiable();

      // Act
      service.SendEmail(
        emailAddress,
        bookTitle);

      // Assert
      Assert.AreEqual(expectedSubject, actualSubject);
      Assert.AreEqual(expectedBody, actualBody);
      this.mockRepository.VerifyAll();
    }
  }
}
