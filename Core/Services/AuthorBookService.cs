using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO;
using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class AuthorBookService
    {
        private readonly AuthorBookRelationDAO _authorBookDAO;
        private readonly BookDAO _bookDAO;

        public AuthorBookService()
        {
            _authorBookDAO = new AuthorBookRelationDAO();
            _bookDAO = new BookDAO();
        }


        public bool AddAuthorToBook(int bookId, int authorId)
        {
            Book? book = _bookDAO.GetById(bookId);
            if (book == null)
                return false;

            bool added = _authorBookDAO.Add(new AuthorBookRelation(authorId, bookId));

            if (!book.AuthorIds.Contains(authorId))
            {
                book.AuthorIds.Add(authorId);
                _bookDAO.Update(book);
            }

            return added;
        }

        public bool RemoveAuthorFromBook(int bookId, int authorId)
        {
            bool removed = _authorBookDAO.Remove(authorId, bookId);

            Book? book = _bookDAO.GetById(bookId);

            if (book != null && book.AuthorIds.Remove(authorId))
                _bookDAO.Update(book);

            return removed;
        }

        //#uklanjanje_knjige_sa_autora
        public bool RemoveBookFromAuthor(int bookId, int authorId)
        {
            return RemoveAuthorFromBook(bookId, authorId);
        }


        //  8.21 prikaz knjiga koje je autor napisao 
        public List<AuthorBookRelation> GetByAuthorId(int authorId)
        {
            return _authorBookDAO.GetByAuthorId(authorId);
        }

        //  8.22 dodavanje knjige autoru 
        public List<Book> GetAvailableBooksForAuthor(int authorId)
        {
            BookDAO bookDAO = new BookDAO();

            var authorBookIds = _authorBookDAO
                .GetByAuthorId(authorId)
                .Select(ab => ab.BookId)
                .ToHashSet();

            return bookDAO.GetAll()
                .Where(b => !authorBookIds.Contains(b.Id))
                .ToList();
        }
    }
}
