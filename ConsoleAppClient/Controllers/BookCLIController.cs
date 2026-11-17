using ConsoleAppClient.Utils;
using Core.Models;
using Core.Services;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppClient.Controllers
{
    public class BookCLIController
    {
        private BookService bookService;
        private AuthorBookService authorBookService;
        private PublisherService publisherService;

        public BookCLIController(BookService bookService)
        {
            this.bookService = bookService;
            authorBookService = new AuthorBookService();
            publisherService = new PublisherService();
        }

        public void RunMenu()
        {
            while (true)
            {
                showMenu();

                string userInput = System.Console.ReadLine() ?? "0";
                if (userInput == "0") 
                    break;

                handleMenuInput(userInput);
            }
        }

        private void showMenu()
        {
            System.Console.WriteLine("\nBook menu:");
            System.Console.WriteLine("1: Add book");
            System.Console.WriteLine("2: Show all books");
            System.Console.WriteLine("3: Update book");
            System.Console.WriteLine("4: Remove book");
            System.Console.WriteLine("0: Back");
        }

        private void handleMenuInput(string input)
        {
            switch (input)
            {
                case "1": addBook(); break;
                case "2": showAllBooks(); break;
                case "3": updateBook(); break;
                case "4": removeBook(); break;
            }
        }


        private void addBook()
        {
            try
            {
                Book book = inputBook();
                Book savedBook = bookService.Add(book);

                foreach (int authorId in savedBook.AuthorIds)
                    authorBookService.AddAuthorToBook(savedBook.Id, authorId);

                publisherService.AddBook(savedBook.PublisherId, savedBook.Id);
                System.Console.WriteLine("Book added.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void showAllBooks()
        {
            printBooks(bookService.GetAll());
        }

        private void printBooks(List<Book> books)
        {
            System.Console.WriteLine("\nBooks:");
            foreach (Book b in books)
            {
                System.Console.WriteLine(b);
                System.Console.WriteLine("---------------------");
            }
        }

        private Book inputBook()
        {
            System.Console.WriteLine("Enter title:");
            string title = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter publication year:");
            int publicationYear = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Enter genre:");
            string genre = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter ISBN:");
            string isbn = System.Console.ReadLine() ?? string.Empty;

            System.Console.WriteLine("Enter price:");
            double price = ConsoleViewUtils.SafeInputDouble();

            System.Console.WriteLine("Enter page number:");
            int pageNumber = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Enter publisher id:");
            int publisherId = ConsoleViewUtils.SafeInputInt();

            System.Console.WriteLine("Enter author ids separated by comma (example: 1,2):");
            List<int> authorIds = ConsoleViewUtils.ParseIntList(System.Console.ReadLine() ?? string.Empty);

            return new Book
            {
                Title = title,
                ISBN = isbn,
                Genre = genre,
                YearPublished = publicationYear,
                Price = price,
                PageNumber = pageNumber,
                PublisherId = publisherId,
                AuthorIds = authorIds
            };
        }

        private void updateBook()
        {
            int id = inputId();
            Book? existingBook = bookService.GetById(id);
            if (existingBook == null)
            {
                System.Console.WriteLine("Book not found.");
                return;
            }

            int oldPublisherId = existingBook.PublisherId;

            try
            {
                Book book = inputBook();
                book.Id = id;

                Book? updatedBook = bookService.Update(book);
                if (updatedBook == null)
                {
                    System.Console.WriteLine("Book not found.");
                    return;
                }

                authorBookService.RemoveByBook(id);
                foreach (int authorId in book.AuthorIds)
                    authorBookService.AddAuthorToBook(id, authorId);

                if (oldPublisherId != book.PublisherId)
                    publisherService.RemoveBook(oldPublisherId, id);

                publisherService.AddBook(book.PublisherId, id);
                System.Console.WriteLine("Book updated.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private int inputId()
        {
            System.Console.WriteLine("Enter book id:");
            return ConsoleViewUtils.SafeInputInt();
        }

        private void removeBook()
        {
            int id = inputId();
            Book? removedBook = bookService.Remove(id);
            System.Console.WriteLine(removedBook == null ? "Book not found." : "Book removed.");
        }

     
    }
}
