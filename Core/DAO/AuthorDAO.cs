using Core.Interfaces;
using Core.Models;
using Core.Storage;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO
{
    public class AuthorDAO : IAuthorDAO
    {
        private List<Author> authors;
        private Storage<Author> storage;

        public AuthorDAO()
        {
            storage = new Storage<Author>(FileConstants.AuthorsFile);
            authors = storage.Load();
        }

        private int GenerateId()
        {
            return authors.Count == 0 ? 1 : authors.Max(a => a.Id) + 1;
        }

        public Author Add(Author author)
        {
            author.Id = GenerateId();
            authors.Add(author);
            storage.Save(authors);
            return author;
        }

        public Author? GetById(int Id)
        {
            return authors.Find(a => a.Id == Id);
        }

        public List<Author> GetAll()
        {
            return authors;
        }

        public List<Author> GetAll(int page, int pageSize, string sortCriteria, SortDirection direction)
        {
            IEnumerable<Author> authors = this.authors;

            authors = sortCriteria switch
            {
                "FirstName" or "Ime" => direction == SortDirection.Ascending 
                                              ? authors.OrderBy(a => a.FirstName)
                                              : authors.OrderByDescending(a => a.FirstName),
                "LastName" or "Prezime" => direction == SortDirection.Ascending
                                              ? authors.OrderBy(a => a.LastName)
                                              : authors.OrderByDescending(a => a.LastName),
                "Email" => direction == SortDirection.Ascending
                                              ? authors.OrderBy(a => a.Email)
                                              : authors.OrderByDescending(a => a.Email),
                _ => direction == SortDirection.Ascending
                                              ? authors.OrderBy(a => a.Id)
                                              : authors.OrderByDescending(a => a.Id),
            };

            return authors.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public Author? Remove(int Id)
        {
            Author? author = GetById(Id);
            if (author == null) return null;

            authors.Remove(author);
            storage.Save(authors);
            return author;
        }

        public Author? Update(Author author)
        {
            Author? oldAuthor = GetById(author.Id);
            if (oldAuthor == null) return null;

            oldAuthor.FirstName = author.FirstName;
            oldAuthor.LastName = author.LastName;
            oldAuthor.DateOfBirth = author.DateOfBirth;
            oldAuthor.AddressId = author.AddressId;
            oldAuthor.Address = author.Address;
            oldAuthor.Phone = author.Phone;
            oldAuthor.Email = author.Email;
            oldAuthor.IdCardNumber = author.IdCardNumber;
            oldAuthor.YearsOfExperience = author.YearsOfExperience;


            storage.Save(authors);
            return oldAuthor;
        }
    }
}
