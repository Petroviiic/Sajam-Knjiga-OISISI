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
        private readonly Storage<Purchase> _storage;
        private readonly List<Purchase> _purchases;
        public PurchaseDAO()
        {
            _storage = new Storage<Purchase>("purchases.txt");
            _purchases = _storage.Load();
        }

        private int GenerateId()
        {
            return _purchases.Count == 0 ? 1 : _purchases.Max(p => p.Id) + 1;
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

            _storage.Save(_purchases);
            return existingPurchase;
        }

        public Purchase? Remove(int Id)
        {
            Purchase? purchase = GetById(Id);

            if (purchase == null)
                return null;

            _purchases.Remove(purchase);
            _storage.Save(_purchases);
            return purchase;
        }

        public Purchase? GetById(int Id)
        {
            return _purchases.Find(p => p.Id == Id);
        }

        public List<Purchase> GetAll()
        {
            return _purchases;
        }

        
    }
}
