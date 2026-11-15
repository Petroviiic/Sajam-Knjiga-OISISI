using Core.Interfaces;
using Core.Models;
using Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.DAO
{
    public class PublisherDAO : IPublisherDAO
    {
        private readonly Storage<Publisher> storage;
        private readonly List<Publisher> publishers;

        public PublisherDAO()
        {
            storage = new Storage<Publisher>("publishers.txt");
            publishers = storage.Load();
        }

        public int GenerateId()
        {
            if (publishers.Count == 0)
                return 0;
            return publishers.Max(v => v.Id) + 1;
        }

        public Publisher Add(Publisher publisher)
        {
            publisher.Id = GenerateId();
            publishers.Add(publisher);
            storage.Save(publishers);
            return publisher;
        }

        public Publisher? Update(Publisher publisher)
        {
            Publisher? existingPublisher = GetById(publisher.Id);

            if (existingPublisher == null)
                return null;

            existingPublisher.Code = publisher.Code;
            existingPublisher.Name = publisher.Name;
            existingPublisher.ChiefEditorId = publisher.ChiefEditorId;
            existingPublisher.AuthorIds = new List<int>(publisher.AuthorIds);
            existingPublisher.PublishedBookIds = new List<int>(publisher.PublishedBookIds);
            storage.Save(publishers);
            return existingPublisher;
        }

        public Publisher? Remove(int id)
        {
            Publisher? publisher = GetById(id);

            if (publisher == null)
                return null;

            publishers.Remove(publisher);
            storage.Save(publishers);
            return publisher;
        }

        public Publisher? GetById(int id)
        {
            return publishers.Find(p => p.Id == id);
        }

        public List<Publisher> GetAll()
        {
            return publishers;
        }

    }
}
