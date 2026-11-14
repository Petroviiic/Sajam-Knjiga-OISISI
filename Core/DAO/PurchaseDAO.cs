using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Storage;
using Core.Interfaces;

namespace Core.DAO
{
    public class PurchaseDAO : IPurchaseDAO
    {
        private readonly Storage<Purchase> storage;
        private readonly List<Purchase> purchases;
        public PurchaseDAO()
        {
            storage = new Storage<Purchase>(FileConstants.PurchasedFile);
            purchases = storage.Load();
        }

        private int GenerateId()
        {
            return purchases.Count == 0 ? 1 : purchases.Max(p => p.Id) + 1;
        }
        public Purchase Add(Purchase purchase)
        {
            purchase.Id = GenerateId();
            purchases.Add(purchase);
            storage.Save(purchases);
            return purchase;
        }

        public Purchase? Update(Purchase purchase)
        {
            Purchase? existingPurchase = GetById(purchase.Id);

            if (existingPurchase == null)
                return null;

            existingPurchase.VisitorId = purchase.VisitorId;
            existingPurchase.BookId = purchase.BookId;
            existingPurchase.PurchaseDate = purchase.PurchaseDate;
            existingPurchase.Rating = purchase.Rating;
            existingPurchase.Comment = purchase.Comment;

            storage.Save(purchases);
            return existingPurchase;
        }

        public Purchase? Remove(int Id)
        {
            Purchase? purchase = GetById(Id);

            if (purchase == null)
                return null;

            purchases.Remove(purchase);
            storage.Save(purchases);
            return purchase;
        }

        public Purchase? GetById(int Id)
        {
            return purchases.Find(p => p.Id == Id);
        }

        public List<Purchase> GetAll()
        {
            return purchases;
        }

        
    }
}
