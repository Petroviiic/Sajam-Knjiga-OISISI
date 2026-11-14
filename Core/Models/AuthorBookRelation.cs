using Core.Storage.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AuthorBookRelation : ISerializable
    {
        public int AuthorId { get; set; }

        public int BookId { get; set; }

        public AuthorBookRelation()
        {
            AuthorId = -1;
            BookId = -1;
        }

        public AuthorBookRelation(int authorId, int bookId)
        {
            AuthorId = authorId;
            BookId = bookId;
        }

        public void FromCSV(string[] values)
        {
            AuthorId = int.Parse(values[0]);
            BookId = int.Parse(values[1]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                AuthorId.ToString(),
                BookId.ToString()
            };
            return csvValues;
        }
        public override string ToString()
        {
            return "AuthorID: " + AuthorId + " BookID: " + BookId;
        }
    }
}
