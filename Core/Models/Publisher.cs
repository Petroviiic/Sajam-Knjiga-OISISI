using System;
using System.Collections.Generic;
using System.Linq;
using Core.Storage.Serialization;

using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Publisher : ISerializable
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";

        // za serijalizaciju - foreign key
        public int ChiefEditorId { get; set; }
        public List<int> AuthorIds { get; set; } = new();
        public List<int> PublishedBookIds { get; set; } = new();

        public Publisher() { }

        public Publisher(int id, string code, string name, int chiefEditorID)
        {
            Id = id;
            Code = code;
            Name = name;
            ChiefEditorId = chiefEditorID;
        }

        public string[] ToCSV()
        {
            string authorsCsv = string.Join(";", AuthorIds);
            string booksCsv = string.Join(";", PublishedBookIds);
            string[] csvValues =
            {
                Id.ToString(),
                Code,
                Name,
                ChiefEditorId.ToString(),
                authorsCsv,
                booksCsv
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Code = values[1];
            Name = values[2];
            ChiefEditorId = int.Parse(values[3]);

            AuthorIds = new List<int>();
            if (!string.IsNullOrEmpty(values[4]))
            {
                string[] authorIdStrings = values[4].Split(';');
                foreach (string idStr in authorIdStrings)
                {
                    AuthorIds.Add(int.Parse(idStr));
                }
            }

            PublishedBookIds = new List<int>();
            if (!string.IsNullOrEmpty(values[5]))
            {
                string[] bookIdStrings = values[5].Split(';');
                foreach (string idStr in bookIdStrings)
                {
                    PublishedBookIds.Add(int.Parse(idStr));
                }
            }
        }

        public override string ToString()
        {
            return
                $"Publisher [ID: {Id}, Code: {Code}, Name: {Name})\n" +
                $"ChiefEditorId: {ChiefEditorId}\n" +
                $"Authors count: {AuthorIds.Count}\n" +
                $"Books count: {PublishedBookIds.Count}";
        }

        /*
        public void AddAuthor(int authorId)
        {
            if (!AuthorIds.Contains(authorId))
                AuthorIds.Add(authorId);
        }

        public void AddBook(int bookId)
        {
            if (!PublishedBookIds.Contains(bookId))
                PublishedBookIds.Add(bookId);
        }

        public void RemoveAuthor(int authorId)
        {
            AuthorIds.Remove(authorId);
        }

        public void RemoveBook(int bookId)
        {
            PublishedBookIds.Remove(bookId);
        }
        */
    }
}
