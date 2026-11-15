using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthorBookRelationDAO
    {
        bool Add(AuthorBookRelation relation);
        bool Remove(int authorId, int bookId);
        List<AuthorBookRelation> GetAll();

        List<AuthorBookRelation> GetByAuthorId(int id);

        List<AuthorBookRelation> GetByBookId(int bookId);
    }
}
