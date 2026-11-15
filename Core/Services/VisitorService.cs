using Core.DAO;
using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class VisitorService
    {
        private readonly VisitorDAO visitorDAO;
        private readonly PurchaseDAO purchaseDAO;
        private readonly BookDAO bookDAO;
        private readonly AddressDAO addressDAO; 
        private readonly WishlistDAO wishlistDAO;
        private readonly AuthorBookRelationDAO authorBookDAO; 

        public VisitorService()
        {
            visitorDAO = new VisitorDAO();
            purchaseDAO = new PurchaseDAO();
            bookDAO = new BookDAO();
            addressDAO = new AddressDAO();
            wishlistDAO = new WishlistDAO();
            authorBookDAO = new AuthorBookRelationDAO();
        }

        // ================= RELACIJE =================
        private void LoadRelations(Visitor visitor)
        {
            if (visitor == null) return;

            visitor.Address = visitor.AddressId != 0
                ? addressDAO.GetById(visitor.AddressId)
                : null;

            visitor.Purchases = purchaseDAO.GetByVisitor(visitor.Id);
        }

        private void LoadRelations(List<Visitor> visitors)
        {
            foreach (var v in visitors)
                LoadRelations(v);
        }

        // ================= CREATE =================
        public Visitor AddVisitor(Visitor visitor)
        {
            ValidateVisitor(visitor);

            if (visitor.Address != null && visitor.Address.Id == 0)
            {
                var saved = addressDAO.Add(visitor.Address);
                visitor.Address = saved;
                visitor.AddressId = saved.Id;
            }

            return visitorDAO.Add(visitor);
        }

        // ================= UPDATE =================
        public Visitor? UpdateVisitor(Visitor visitor)
        {
            ValidateVisitor(visitor, isUpdate: true);

            if (visitor.Address != null)
            {
                if (visitor.Address.Id == 0)
                {
                    var saved = addressDAO.Add(visitor.Address);
                    visitor.Address = saved;
                }
                else
                {
                    addressDAO.Update(visitor.Address);
                }
                visitor.AddressId = visitor.Address.Id;
            }

            return visitorDAO.Update(visitor);
        }

        // ================= DELETE =================
        public Visitor? RemoveVisitor(int id)
        {
            // Voditi racuna o referencijalnim zavisnostima 8.12
            // (kupovine i lista zelja se brisu pre posetioca)
            var purchases = purchaseDAO.GetByVisitor(id);
            foreach (var p in purchases)
                purchaseDAO.Remove(p.Id);

            wishlistDAO.RemoveByVisitorId(id);

            return visitorDAO.Remove(id);
        }

        // ================= READ =================
        public Visitor? GetById(int id)
        {
            var visitor = visitorDAO.GetById(id);
            if (visitor != null)
                LoadRelations(visitor);

            return visitor;
        }

        public List<Visitor> GetAll()
        {
            var visitors = visitorDAO.GetAll();
            LoadRelations(visitors);
            return visitors;
        }
        public Visitor? GetByCardNumber(string cardNumber)
        {
            var visitor = visitorDAO.GetByCardNumber(cardNumber);
            if (visitor != null)
                LoadRelations(visitor);
            return visitor;
        }


        // ================= PRETRAGA 8.19 =================
        // Jedna rec  -> prezime
        // Dve reci   -> prezime, ime
        // Tri reci   -> broj kartice, ime, prezime
        public List<Visitor> Search(string query)
        {
            List<Visitor> results;

            if (string.IsNullOrWhiteSpace(query))
                return visitorDAO.GetAll();

            var parts = query.Split(',', StringSplitOptions.TrimEntries);

            if (parts.Length == 1)
            {
                string lastName = parts[0].ToLower();
                results = visitorDAO.GetAll()
                    .Where(v => v.LastName.ToLower().Contains(lastName))
                    .ToList();
            }
            else if (parts.Length == 2)
            {
                string lastName = parts[0].ToLower();
                string firstName = parts[1].ToLower();
                results = visitorDAO.GetAll()
                    .Where(v => v.LastName.ToLower().Contains(lastName) &&
                                v.FirstName.ToLower().Contains(firstName))
                    .ToList();
            }
            else // tri reci
            {
                string card = parts[0].ToLower();
                string firstName = parts[1].ToLower();
                string lastName = parts[2].ToLower();
                results = visitorDAO.GetAll()
                    .Where(v => v.MembershipCardNumber.ToLower().Contains(card) &&
                                v.FirstName.ToLower().Contains(firstName) &&
                                v.LastName.ToLower().Contains(lastName))
                    .ToList();
            }

            LoadRelations(results);
            return results;
        }




        //validacija!!!!!
        private void ValidateVisitor(Visitor visitor, bool isUpdate = false)
        {
            if (string.IsNullOrWhiteSpace(visitor.FirstName))
                throw new Exception("Ime je obavezno.");

            if (string.IsNullOrWhiteSpace(visitor.LastName))
                throw new Exception("Prezime je obavezno.");

            if (string.IsNullOrWhiteSpace(visitor.MembershipCardNumber))
                throw new Exception("Broj clanske karte je obavezan.");

            var freshDAO = new VisitorDAO();
            var all = freshDAO.GetAll();

            bool cardExists = all.Any(v =>
                v.MembershipCardNumber.Equals(visitor.MembershipCardNumber, StringComparison.OrdinalIgnoreCase)
                && (!isUpdate || v.Id != visitor.Id));

            if (cardExists)
                throw new Exception("Broj clanske karte vec postoji.");

            bool emailExists = all.Any(v =>
                v.Email.Equals(visitor.Email, StringComparison.OrdinalIgnoreCase)
                && (!isUpdate || v.Id != visitor.Id));

            if (emailExists)
                throw new Exception("Email vec postoji.");
        }


    }
}
