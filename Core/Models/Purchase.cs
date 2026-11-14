using Core.Storage.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Purchase : ISerializable
    {
        public int Id { get; set; }

        // za serijalizaciju 
        public int VisitorId { get; set; }
        public int BookId { get; set; }

        public DateTime PurchaseDate { get; set; }
        private int _rating;
        public int Rating
        {
            get { return _rating; }
            set
            {
                if (value < 1 || value > 5)
                {
                    throw new ArgumentOutOfRangeException("Rating must be between 1 and 5.");
                }
                _rating = value;
            }
        }
        public string Comment { get; set; } = string.Empty;

        public Purchase() { }

        public Purchase(int id, int visitorId, int bookId, DateTime purchaseDate, int rating, string comment)
        {
            Id = id;
            VisitorId = visitorId;
            BookId = bookId;
            PurchaseDate = purchaseDate;
            Rating = rating;
            Comment = comment;
        }

        // Serijalizacija -> CSV format
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                VisitorId.ToString(),
                BookId.ToString(),
                PurchaseDate.ToString("yyyy-MM-dd"),
                Rating.ToString(),
                Comment
            };
            return csvValues;
        }

        // Deserijalizacija -> iz CSV formata
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            VisitorId = int.Parse(values[1]);
            BookId = int.Parse(values[2]);
            PurchaseDate = DateTime.ParseExact(values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Rating = int.Parse(values[4]);
            Comment = values[5];
        }

        public override string ToString()
        {
            return $"Purchase Id: {Id}\n " +
                $"VisitorId: {VisitorId}\n" +
                $"BookId: {BookId}\n" +
                $"Date: {PurchaseDate: yyyy-MM-dd}\n" +
                $"Rating: {Rating}\n" +
                $"Comment: {Comment}";
        }


    }
}
