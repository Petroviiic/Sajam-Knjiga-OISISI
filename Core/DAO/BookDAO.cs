using Core.Interfaces;
using Core.Models;
using Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO
{
    public class BookDAO : IBookDAO
    {
        //static moraju biti jer ce i bookservice a i relationservice (npr book autor ili nesto drugo) 
        //morati koristiti iste instance storagea
        private static readonly Storage<Book> storage = new Storage<Book>(FileConstants.BooksFile);
        private static readonly List<Book> books = storage.Load();

        public BookDAO() {}

        public Book? Remove(int id)
        {
            Book? book = GetById(id);

            if (book == null)
                return null;

            books.Remove(book);
            storage.Save(books);
            return book;
        }

        public Book? GetById(int id)
        {
            return books.Find(b => b.Id == id);
        }

        public List<Book> GetAll()
        {
            return books;
        }

        //Lana
        private int GenerateId()
        {
            return books.Count == 0 ? 1 : books.Max(b => b.Id) + 1;
        }

        public Book Add(Book book)
        {
            book.Id = GenerateId();
            books.Add(book);
            storage.Save(books);
            return book;
        }


        public Book? Update(Book book)
        {
            Book? oldBook = GetById(book.Id);

            if (oldBook == null)
                return null;

            oldBook.Title = book.Title;
            oldBook.ISBN = book.ISBN;
            oldBook.Genre = book.Genre;
            oldBook.YearPublished = book.YearPublished;
            oldBook.Price = book.Price;
            oldBook.PageNumber = book.PageNumber;
            oldBook.PublisherId = book.PublisherId;
            oldBook.AuthorIds = new List<int>(book.AuthorIds);


            storage.Save(books);
            return oldBook;
        }

        public List<Book> GetAll(int page, int pageSize, string sortCriteria, SortDirection sortDirection)
        {
            IEnumerable<Book> _books = books;

            // ISBN, naziv, cijena, godina izdanja, zanr
            _books = sortCriteria switch
            {
                "ISBN" => sortDirection == SortDirection.Ascending
                                                    ? _books.OrderBy(b => b.ISBN)
                                                    : _books.OrderByDescending(b => b.ISBN),
                "Title" or "Naziv" => sortDirection == SortDirection.Ascending
                                                    ? _books.OrderBy(b => b.Title)
                                                    : _books.OrderByDescending(b => b.Title),
                "Price" or "Cijena" => sortDirection == SortDirection.Ascending
                                                    ? _books.OrderBy(b => b.Price)         
                                                    : _books.OrderByDescending(b => b.Price),
                "YearPublished" or "Year" => sortDirection == SortDirection.Ascending
                                                    ? _books.OrderBy(b => b.YearPublished) 
                                                    : _books.OrderByDescending(b => b.YearPublished),
                "Genre" or "Zanr" => sortDirection == SortDirection.Ascending
                                                    ? _books.OrderBy(b => b.Genre)
                                                    : _books.OrderByDescending(b => b.Genre),
                _ => sortDirection == SortDirection.Ascending
                                                    ? _books.OrderBy(b => b.Id)
                                                    : _books.OrderByDescending(b => b.Id),
            };


            return _books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public bool BookExists(int id)
        {
            return books.Exists(b => b.Id == id);
        }
    }
}
