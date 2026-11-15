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
        private AddressDAO addressDAO;
        private PublisherDAO publisherDAO;

        public AuthorService()
        {
            authorDAO = new AuthorDAO();
            authorBookRelationDAO = new AuthorBookRelationDAO();
            addressDAO = new AddressDAO();
            bookDAO = new BookDAO();
            publisherDAO = new PublisherDAO();
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


        public Author Add(Author author)
        {
            validate(author);

            if (author.Address != null && author.Address.Id == 0)
            {
                var saved = addressDAO.Add(author.Address);
                author.Address = saved;
                author.AddressId = saved.Id;
            }

            return authorDAO.Add(author);
        }

        public Author? Update(Author author)
        {
            validate(author);

            if (author.Address != null)
            {
                if (author.Address.Id == 0)
                {
                    var saved = addressDAO.Add(author.Address);
                    author.Address = saved;
                }
                else
                {
                    addressDAO.Update(author.Address);
                }
                author.AddressId = author.Address.Id;
            }

            return authorDAO.Update(author);
        }

        public Author? Remove(int id)
        {
            // uklanja veze sa knjigama
            var relations = authorBookRelationDAO.GetByAuthorId(id);
            foreach (var rel in relations)
            {
                Book? book = bookDAO.GetById(rel.BookId);
                if (book != null && book.AuthorIds.Remove(id))
                    bookDAO.Update(book);

                authorBookRelationDAO.Remove(rel.AuthorId, rel.BookId);
            }

            foreach (Publisher publisher in publisherDAO.GetAll())
            {
                bool changed = publisher.AuthorIds.Remove(id);

                if (publisher.ChiefEditorId == id)
                {
                    publisher.ChiefEditorId = 0;
                    changed = true;
                }

                if (changed)
                    publisherDAO.Update(publisher);
            }

            return authorDAO.Remove(id);
        }

        public Author? GetById(int id)
        {
            var author = authorDAO.GetById(id);
            if (author != null)
                loadAuthorRelations(author);
            return author;
        }

        public List<Author> GetAll()
        {
            var authors = authorDAO.GetAll();
            LoadAllRelations(authors);
            return authors;
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



        private void validate(Author author)
        {
            if (string.IsNullOrWhiteSpace(author.FirstName))
                throw new Exception("Ime je obavezno.");

            if (string.IsNullOrWhiteSpace(author.LastName))
                throw new Exception("Prezime je obavezno.");

            if (string.IsNullOrWhiteSpace(author.IdCardNumber))
                throw new Exception("Broj licne karte je obavezan.");

            if (author.YearsOfExperience < 0)
                throw new Exception("Godine iskustva ne mogu biti negativne.");
        }
    }
}
