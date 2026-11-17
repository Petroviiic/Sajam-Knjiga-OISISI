using Core.Interfaces;
using Core.Models;
using Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace Core.DAO
{
    public class PublisherDAO : IPublisherDAO
    {
        private static readonly Storage<Publisher> storage = new Storage<Publisher>(FileConstants.PublishersFile);
        private static readonly List<Publisher> publishers = storage.Load();

        public PublisherDAO() { }

        public int GenerateId()
        {
            return publishers.Count == 0 ? 1 : publishers.Max(b => b.Id) + 1;
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
