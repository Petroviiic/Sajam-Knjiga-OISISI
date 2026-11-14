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
    }
}
