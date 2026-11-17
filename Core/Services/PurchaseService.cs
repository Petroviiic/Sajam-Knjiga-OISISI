using Core.DAO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class PurchaseService
    {
        private readonly PurchaseDAO purchaseDAO;
        private readonly VisitorDAO visitorDAO;
        private readonly BookDAO bookDAO;
        private WishlistDAO wishlistDAO;

        public PurchaseService()
        {
            purchaseDAO = new PurchaseDAO();
            visitorDAO = new VisitorDAO();
            bookDAO = new BookDAO();
            wishlistDAO = new WishlistDAO();
        }

        private void validate(Purchase purchase)
        {
            if (!visitorDAO.VisitorExists(purchase.VisitorId))
                throw new Exception("Posjetilac ne postoji.");

            if (!bookDAO.BookExists(purchase.BookId))
                throw new Exception("Knjiga ne postoji.");

            if (purchase.Rating < 1 || purchase.Rating > 5)
                throw new Exception("Ocjena mora biti izmedju 1 i 5.");
        }

        public Purchase Add(Purchase purchase)
        {
            validate(purchase);

            if (purchase.PurchaseDate == DateTime.MinValue)
                purchase.PurchaseDate = DateTime.Now;

            return purchaseDAO.Add(purchase);
        }

        public Purchase? Update(Purchase purchase)
        {
            validate(purchase);
            return purchaseDAO.Update(purchase);
        }

        public Purchase? Remove(int id)
        {
            Purchase? removed = purchaseDAO.Remove(id);

            if (removed != null)
            {
                wishlistDAO.AddBookToWishlist(removed.VisitorId, removed.Id);
            }
            return removed;
        }

        public Purchase? GetById(int id)
        {
            return purchaseDAO.GetById(id);
        }

        public List<Purchase> GetAll()
        {
            return purchaseDAO.GetAll().ToList();
        }

    }
}
