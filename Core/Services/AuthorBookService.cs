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
