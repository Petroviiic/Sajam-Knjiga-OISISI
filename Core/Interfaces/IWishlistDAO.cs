using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IWishlistDAO
    {
        bool RemoveBookFromWishlist(int visitorId, int bookId);
        bool AddBookToWishlist(int visitorId, int bookId);
        List<Wishlist> GetAll();
        void RemoveByBookId(int bookId);


        List<VisitorBook> GetByVisitorId(int visitrId);
    }
}
