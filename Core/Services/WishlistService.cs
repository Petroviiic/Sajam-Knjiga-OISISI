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
        private readonly WishlistDAO _wishlistDAO;
        private readonly VisitorDAO _visitorDAO;
        private readonly BookDAO _bookDAO;

        public VisitorBookService()
        {
            _wishlistDAO = new WishlistDAO();
            _visitorDAO = new VisitorDAO();
            _bookDAO = new BookDAO();
        }

        // dodaj na listu zelja 8.16
        public bool AddBookToWishlist(int visitorId, int bookId)
        {
            if (!_visitorDAO.VisitorExists(visitorId))
                return false;

            if (!_bookDAO.BookExists(bookId))
                return false;

            return _wishlistDAO.AddBookToWishlist(visitorId, bookId);
        }

        // Ukloni SA LISTE ZELJA 8.17
        public bool RemoveBookFromWishlist(int visitorId, int bookId)
        {
            return _wishlistDAO.RemoveBookFromWishlist(visitorId, bookId);
        }

        // ================= READ =================
        public List<Wishlist> GetAll()
        {
            return _wishlistDAO.GetAll();
        }

        // 8.15 - lista zelja jednog posetioca
        public List<Wishlist> GetByVisitorId(int visitorId)
        {
            return _wishlistDAO.GetByVisitorId(visitorId);
        }

        // Koji posetioci imaju odredjenu knjigu na listi zelja
        public List<Wishlist> GetByBookId(int bookId)
        {
            return _wishlistDAO.GetByBookId(bookId);
        }

        // 8.16: knjige koje visitor MOZE dodati na listu zelja
        // (nije vec kupio i nije vec na listi zelja)
        public List<Book> GetAvailableBooksForWishlist(int visitorId)
        {
            // ID-evi vec kupljenih knjiga
            var purchaseDAO = new PurchaseDAO();
            var boughtBookIds = new System.Collections.Generic.HashSet<int>(
                purchaseDAO.GetByVisitor(visitorId)
                    .ConvertAll(p => p.BookId)
            );

            // ID-evi knjiga vec na listi zelja
            var wishlistBookIds = new System.Collections.Generic.HashSet<int>(
                _wishlistDAO.GetByVisitorId(visitorId)
                    .ConvertAll(vb => vb.BookId)
            );

            // Vraca sve knjige koje nisu ni kupljene ni na listi zelja
            return _bookDAO.GetAll()
                .FindAll(b => !boughtBookIds.Contains(b.Id) &&
                              !wishlistBookIds.Contains(b.Id));
        }
    }
}
