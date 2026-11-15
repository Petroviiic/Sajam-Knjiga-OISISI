using Core.Interfaces;
using Core.Models;
using Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO
{
    public class WishlistDAO :  IWishlistDAO
    {
        private readonly List<Wishlist> wishlist;
        private readonly Storage<Wishlist> storage;

        public WishlistDAO()
        {
            storage = new Storage<Wishlist>(FileConstants.WishlistFile);
            wishlist = storage.Load();
        }

        public bool RemoveBookFromWishlist(int visitorId, int bookId)
        {
            var existing = wishlist.FirstOrDefault(vb => vb.VisitorId == visitorId && vb.BookId == bookId);

            if (existing == null) return false;

            wishlist.Remove(existing);
            storage.Save(wishlist);

            return true;
        }

        public List<Wishlist> GetAll()
        {
            return wishlist;
        }

        public bool AddBookToWishlist(int visitorId, int bookId)
        {
            var existing = wishlist
                .FirstOrDefault(vb => vb.VisitorId == visitorId && vb.BookId == bookId);

            if (existing != null)
                return false;

            Wishlist objekat = new Wishlist(visitorId, bookId);
            wishlist.Add(objekat);
            storage.Save(wishlist);

            return true;
        }
        public List<Wishlist> GetByVisitorId(int visitorId)
        {
            return wishlist.Where(r => r.VisitorId == visitorId).ToList();
        }

        public void RemoveByVisitorId(int visitorId)
        {
            wishlist.RemoveAll(vb => vb.VisitorId == visitorId);
            storage.Save(wishlist);
        }


        public void RemoveByBookId(int bookId)
        {
            wishlist.RemoveAll(vb => vb.BookId == bookId);
            storage.Save(wishlist);
        }
    }
}
