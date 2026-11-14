using Core.Storage.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Wishlist : ISerializable
    {
        public int VisitorId { get; set; }
        public int BookId { get; set; }

        public Wishlist()
        {
            VisitorId = -1;
            BookId = -1;
        }
        public Wishlist(int visitorId, int bookId)
        {
            VisitorId = visitorId;
            BookId = bookId;
        }
        public string[] ToCSV()
        {
            return new string[] { VisitorId.ToString(), BookId.ToString() };
        }

        public void FromCSV(string[] values)
        {
            VisitorId = int.Parse(values[0]);
            BookId = int.Parse(values[1]);
        }

        public override string ToString()
        {
            return $"VisitorBook [VisitorId: {VisitorId}, BookId: {BookId}]";
        }
    }
}
