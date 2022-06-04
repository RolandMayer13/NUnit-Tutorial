using System.Collections.Generic;

namespace AccountService {
  public interface IBookService {
    string GetISBNFor(string bookTitle);
    IEnumerable<string> GetBooksForCategory(string categoryId);
  }
}
