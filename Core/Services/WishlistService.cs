using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO;
using Core.Models;

namespace Core.Services
{
    public class VisitorBookService
    {
        private WishlistDAO wishlistDAO;
        private VisitorDAO visitorDAO;
        private PurchaseDAO purchaseDAO;
        private BookDAO bookDAO;

        public VisitorBookService()
        {
            wishlistDAO = new WishlistDAO();
            visitorDAO = new VisitorDAO();
            bookDAO = new BookDAO();
            purchaseDAO = new PurchaseDAO();
        }

        // dodaj na listu zelja 8.16
        public bool AddBookToWishlist(int visitorId, int bookId)
        {
            if (!visitorDAO.VisitorExists(visitorId))
                return false;

            if (!bookDAO.BookExists(bookId))
                return false;

            bool isAvailable = GetAvailableBooksForWishlist(visitorId).Any(b => b.Id == bookId);

            if (!isAvailable)
                return false;


            return wishlistDAO.AddBookToWishlist(visitorId, bookId);
        }

        // Ukloni SA LISTE ZELJA 8.17
        public bool RemoveBookFromWishlist(int visitorId, int bookId)
        {
            return wishlistDAO.RemoveBookFromWishlist(visitorId, bookId);
        }

        // ================= READ =================
        public List<Wishlist> GetAll()
        {
            return wishlistDAO.GetAll();
        }

        // 8.15 - lista zelja jednog posetioca
        public List<Wishlist> GetByVisitorId(int visitorId)
        {
            return wishlistDAO.GetByVisitorId(visitorId);
        }

        // Koji posetioci imaju odredjenu knjigu na listi zelja
        public List<Wishlist> GetByBookId(int bookId)
        {
            return wishlistDAO.GetByBookId(bookId);
        }

        // 8.16: knjige koje visitor MOZE dodati na listu zelja
        // (nije vec kupio i nije vec na listi zelja)
        public List<Book> GetAvailableBooksForWishlist(int visitorId)
        {
            // ID-evi vec kupljenih knjiga
            var purchaseDAO = new PurchaseDAO();
            var boughtBookIds = new HashSet<int>(
                purchaseDAO.GetByVisitor(visitorId)
                    .ConvertAll(p => p.BookId)
            );

            // ID-evi knjiga vec na listi zelja
            var wishlistBookIds = new HashSet<int>(
                wishlistDAO.GetByVisitorId(visitorId)
                    .ConvertAll(vb => vb.BookId)
            );

            // Vraca sve knjige koje nisu ni kupljene ni na listi zelja
            return bookDAO.GetAll()
                .FindAll(b => !boughtBookIds.Contains(b.Id) &&
                              !wishlistBookIds.Contains(b.Id));
        }
    }
}
