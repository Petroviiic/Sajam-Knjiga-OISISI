using Core.DAO;
using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class BookService
    {
        private readonly BookDAO bookDAO;
        private readonly AuthorBookRelationDAO authorBookRelationDAO;
        private readonly PurchaseDAO purchaseDAO;
        private readonly WishlistDAO wishlistDAO;
        private readonly PublisherDAO publisherDAO;
        private readonly AuthorDAO authorDAO;

        public BookService()
        {
            bookDAO = new BookDAO();
            authorBookRelationDAO = new AuthorBookRelationDAO();
            purchaseDAO = new PurchaseDAO();
            wishlistDAO = new WishlistDAO();
            publisherDAO = new PublisherDAO();
            authorDAO = new AuthorDAO();
        }

        public Book Add(Book book)
        {
            validate(book);
            return bookDAO.Add(book);
        }

        public Book? Update(Book book)
        {
            validate(book);
            return bookDAO.Update(book);
        }

        public Book? Remove(int id)
        {
            Book? book = bookDAO.GetById(id);
            if (book == null)
                return null;

            var relations = authorBookRelationDAO.GetByBookId(id);
            foreach (var rel in relations)
                authorBookRelationDAO.Remove(rel.AuthorId, rel.BookId);

            var purchases = purchaseDAO.GetByBook(id);
            foreach (var p in purchases)
                purchaseDAO.Remove(p.Id);

            wishlistDAO.RemoveByBookId(id);

            Publisher? publisher = publisherDAO.GetById(book.PublisherId);
            if (publisher != null)
            {
                publisher.PublishedBookIds.Remove(id);
                publisherDAO.Update(publisher);
            }

            return bookDAO.Remove(id);
        }

        public Book? GetById(int id)
        {
            return bookDAO.GetById(id);
        }

        public List<Book> GetAll()
        {
            return bookDAO.GetAll();
        }

        public List<Book> GetAll(int page, int pageSize, string sort, SortDirection dir)
        {
            return bookDAO.GetAll(page, pageSize, sort, dir);
        }


        public List<Book> Search(string query)
        {
            var books = bookDAO.GetAll();

            if (string.IsNullOrWhiteSpace(query))
                return books;

            string q = query.Trim().ToLower();

            return books.Where(b => b.Title.ToLower().Contains(q) || b.ISBN.ToLower().Contains(q)).ToList();
        }


        private void validate(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new Exception("Naziv knjige je obavezan.");

            if (string.IsNullOrWhiteSpace(book.ISBN))
                throw new Exception("ISBN je obavezan.");

            if (book.Price <= 0)
                throw new Exception("Cijena mora biti veca od 0.");

            if (book.YearPublished < 1000 || book.YearPublished > DateTime.Now.Year)
                throw new Exception("Godina izdanja nije ispravna.");

            if (publisherDAO.GetById(book.PublisherId) == null)
                throw new Exception($"Izdavac sa ID-jem {book.PublisherId} ne postoji.");

            if (book.AuthorIds == null)
                throw new Exception("Knjiga mora imati bar jednog autora.");

            foreach (int authorId in book.AuthorIds)
            {
                if (authorDAO.GetById(authorId) == null)
                    throw new Exception($"Autor sa ID-jem {authorId} ne postoji.");
            }
        }
    }
}
