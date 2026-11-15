using Core.DAO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PublisherService
    {
        private readonly PublisherDAO publisherDAO;
        private readonly BookDAO bookDAO;
        private readonly AuthorDAO authorDAO;

        public PublisherService()
        {
            publisherDAO = new PublisherDAO();
            bookDAO = new BookDAO();
            authorDAO = new AuthorDAO();
        }


        public Publisher AddPublisher(Publisher publisher)
        {
            validate(publisher);
            return publisherDAO.Add(publisher);
        }

        public Publisher? UpdatePublisher(Publisher publisher)
        {
            validate(publisher);
            return publisherDAO.Update(publisher);
        }

        public Publisher? RemovePublisher(int id)
        {
            foreach (Book book in bookDAO.GetAll().Where(b => b.PublisherId == id))
            {
                book.PublisherId = 0;
                bookDAO.Update(book);
            }

            return publisherDAO.Remove(id);
        }

        public Publisher? GetById(int id)
        {
            return publisherDAO.GetById(id);
        }

        public Author? GetAuthorById(int id)
        {
            return authorDAO.GetById(id);
        }

        public Book? GetBookById(int id)
        {
            return bookDAO.GetById(id);
        }

        public List<Publisher> GetAll()
        {
            return publisherDAO.GetAll();
        }

        // relation book publisher
        public bool AddBook(int publisherId, int bookId)
        {
            Publisher? publisher = publisherDAO.GetById(publisherId);
            if (publisher == null) 
                return false;

            if (!bookDAO.BookExists(bookId)) 
                return false;

            if (!publisher.PublishedBookIds.Contains(bookId))
            {
                publisher.PublishedBookIds.Add(bookId);
                publisherDAO.Update(publisher);
            }

            return true;
        }

        public bool RemoveBook(int publisherId, int bookId)
        {
            Publisher? publisher = publisherDAO.GetById(publisherId);
            if (publisher == null) return false;

            bool removed = publisher.PublishedBookIds.Remove(bookId);

            if (removed)
                publisherDAO.Update(publisher);

            return removed;
        }

        public List<Book> GetBooksForPublisher(int publisherId)
        {
            Publisher? publisher = publisherDAO.GetById(publisherId);
            if (publisher == null)
                return new List<Book>();

            return publisher.PublishedBookIds
                .Select(id => bookDAO.GetById(id))
                .Where(b => b != null)
                .Cast<Book>()
                .ToList();
        }

        public List<Author> GetAuthorsForPublisher(int publisherId)
        {
            Publisher? publisher = publisherDAO.GetById(publisherId);
            if (publisher == null) 
                return new List<Author>();

            HashSet<int> authorIds = getPublisherAuthorIds(publisher);

            return authorIds
                .Select(id => authorDAO.GetById(id))
                .Where(a => a != null)
                .Cast<Author>()
                .ToList();
        }

      

        private HashSet<int> getPublisherAuthorIds(Publisher publisher)
        {
            var authorIds = new HashSet<int>(publisher.AuthorIds);
            foreach (int bookId in publisher.PublishedBookIds)
            {
                Book? book = bookDAO.GetById(bookId);
                if (book != null)
                {
                    foreach (int authorId in book.AuthorIds)
                        authorIds.Add(authorId);
                }
            }

            return authorIds;
        }

        private void validate(Publisher publisher)
        {
            if (string.IsNullOrWhiteSpace(publisher.Code))
                throw new Exception("Sifra izdavaca ne moze biti prazna.");

            if (string.IsNullOrWhiteSpace(publisher.Name))
                throw new Exception("Naziv izdavaca je obavezan.");

            if (publisher.ChiefEditorId == 0)
                return;

            Author? author = authorDAO.GetById(publisher.ChiefEditorId);
            if (author == null)
                throw new Exception("Izabrani sef izdavaca ne postoji.");

            if (author.YearsOfExperience < 5)
                throw new Exception("Sef izdavača mora biti autor sa najmanje 5 godina iskustva.");

            if (!getPublisherAuthorIds(publisher).Contains(publisher.ChiefEditorId))
                throw new Exception("Sef izdavaca mora biti jedan od autora tog izdavaca.");
        }
    }
}
