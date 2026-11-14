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

        public Author Add(Author author)
        {
            throw new NotImplementedException();
        }

        public Author? GetById(int Id)
        {
            return authorList.Find(a => a.Id == Id);
        }

        public List<Author> GetAll()
        {
            return authorList;
        }

        public Author? Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public Author? Update(Author author)
        {
            throw new NotImplementedException();
        }
    }
}
