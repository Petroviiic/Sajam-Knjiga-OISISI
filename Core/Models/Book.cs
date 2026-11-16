using Core.Storage.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Book : ISerializable
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int YearPublished { get; set; }
        public double Price { get; set; }
        public int PageNumber { get; set; }
        public int PublisherId { get; set; }

        public List<int> AuthorIds { get; set; } = new();


        public Book() 
        {
            
        }

        public Book(int id, string isbn, string title, string genre, int year, double price, int pagenum, int publisherid)
        {
            Id = id;
            ISBN = isbn;
            Title = title;
            Genre = genre;
            YearPublished = year;
            Price = price;
            PageNumber = pagenum;
            PublisherId = publisherid;
        }
        public Book(string isbn, string title, string genre, int year, double price, int pagenum, int publisherid)
        {
            ISBN = isbn;
            Title = title;
            Genre = genre;
            YearPublished = year;
            Price = price;
            PageNumber = pagenum;
            PublisherId = publisherid;
        }

        public string[] ToCSV()
        {
            string authorsCsv = string.Join(";", AuthorIds);
            return new string[]
            {
                Id.ToString(),
                ISBN ?? "",
                Title ?? "",
                Genre ?? "",
                YearPublished.ToString(),
                Price.ToString(),
                PageNumber.ToString(),
                PublisherId.ToString(),
                authorsCsv
            };
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ISBN = values[1];
            Title = values[2];
            Genre = values[3];
            YearPublished = int.Parse(values[4]);
            Price = double.Parse(values[5]);
            PageNumber = int.Parse(values[6]);
            PublisherId = int.Parse(values[7]);

            AuthorIds = new List<int>();
            if (!string.IsNullOrEmpty(values[8]))
            {
                foreach (string idStr in values[8].Split(';'))
                {
                    if (int.TryParse(idStr, out int authorId))
                        AuthorIds.Add(authorId);
                }
            }
        }

        public override string ToString()
        {
            return $"Book [ID: {Id}, ISBN: {ISBN}, Title: {Title}, Genre: {Genre}, " +
                   $"Year: {YearPublished}, Price: {Price:F2}, Pages: {PageNumber}, PublisherId: {PublisherId}]";
        }
    }
}
