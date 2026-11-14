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
    public class AuthorService
    {
        private AuthorDAO authorDAO;
        private AuthorBookRelationDAO authorBookRelationDAO;
        private BookDAO bookDAO;

        public AuthorService()
        {
            authorDAO = new AuthorDAO();
            authorBookRelationDAO = new AuthorBookRelationDAO();
            bookDAO = new BookDAO();
        }


        private void loadAuthorRelations(Author author)
        {
            if (author == null) return;

            author.Address = author.AddressId != 0 ? addressDAO.GetById(author.AddressId): null;

            author.Books = authorBookRelationDAO
                .GetByAuthorId(author.Id)
                .Select(ab => bookDAO.GetById(ab.BookId))
                .Where(book => book != null)
                .Cast<Book>()
                .ToList();

        }

        private void LoadAllRelations(List<Author> authors)
        {
            foreach (var a in authors)
                loadAuthorRelations(a);
        }

        // #8.19
        // Jedna rijec = prezime; dve reci odvojene zarezom = prezime, ime
        public List<Author> Search(string query)
        {
            List<Author> results;

            if (string.IsNullOrWhiteSpace(query))
            {
                results = authorDAO.GetAll();
            }
            else
            {
                var parts = query.Split(',', StringSplitOptions.TrimEntries);

                if (parts.Length == 1)
                {
                    string lastName = parts[0].ToLower();

                    results = authorDAO.GetAll()
                        .Where(a => a.LastName.ToLower().Contains(lastName))
                        .ToList();
                }
                else
                {
                    string lastName = parts[0].ToLower();
                    string firstName = parts[1].ToLower();

                    results = authorDAO.GetAll()
                        .Where(a => a.LastName.ToLower().Contains(lastName) &&
                                    a.FirstName.ToLower().Contains(firstName))
                        .ToList();
                }
            }

            LoadAllRelations(results);

            return results;
        }
    }
}
