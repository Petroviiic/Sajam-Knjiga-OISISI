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
    public class AuthorBookRelationDAO: IAuthorBookRelationDAO
    {

        private readonly Storage<AuthorBookRelation> storage;
        private readonly List<AuthorBookRelation> authorBookRelation;

        public AuthorBookRelationDAO()
        {
            storage = new Storage<AuthorBookRelation>(FileConstants.AuthorBookRelationFile);
            authorBookRelation = storage.Load();
        }


        public bool Add(AuthorBookRelation relation)
        {
            if (authorBookRelation.Any(r =>
                r.AuthorId == relation.AuthorId &&
                r.BookId == relation.BookId))
            {
                return false;
            }

            authorBookRelation.Add(relation);
            storage.Save(authorBookRelation);
            return true;
        }

        public bool Remove(int authorId, int bookId)
        {
            var relation = authorBookRelation.FirstOrDefault(r => r.AuthorId == authorId && r.BookId == bookId);

            if (relation == null)
                return false;

            authorBookRelation.Remove(relation);
            storage.Save(authorBookRelation);
            return true;
        }

        public List<AuthorBookRelation> GetAll()
        {
            return authorBookRelation;
        }

        public List<AuthorBookRelation> GetByAuthorId(int authorId)
        {
            return authorBookRelation.Where(r => r.AuthorId == authorId).ToList();
        }
    }
}
