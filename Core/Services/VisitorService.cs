using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO;
using Core.Models;

namespace Core.Services
{
    public class VisitorService
    {
        private readonly VisitorDAO _visitorDAO;
        private readonly PurchaseDAO _purchaseDAO;
        private readonly BookDAO _bookDAO;
        private readonly AddressDAO _addressDAO;
        private readonly VisitorBookDAO _visitorBookDAO;
        private readonly AuthorBookDAO _authorBookDAO;

        public VisitorService()
        {
            _visitorDAO = new VisitorDAO();
            _purchaseDAO = new PurchaseDAO();
            _bookDAO = new BookDAO();
            _addressDAO = new AddressDAO();
            _visitorBookDAO = new VisitorBookDAO();
            _authorBookDAO = new AuthorBookDAO();
        }

        // ================= RELACIJE =================
        private void LoadRelations(Visitor visitor)
        {
            if (visitor == null) return;

            visitor.Address = visitor.AddressId != 0
                ? _addressDAO.GetById(visitor.AddressId)
                : null;

            visitor.Purchases = _purchaseDAO.GetByVisitor(visitor.Id);
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
                var saved = _addressDAO.Add(visitor.Address);
                visitor.Address = saved;
                visitor.AddressId = saved.Id;
            }

            return _visitorDAO.Add(visitor);
        }

        // ================= UPDATE =================
        public Visitor? UpdateVisitor(Visitor visitor)
        {
            ValidateVisitor(visitor, isUpdate: true);

            if (visitor.Address != null)
            {
                if (visitor.Address.Id == 0)
                {
                    var saved = _addressDAO.Add(visitor.Address);
                    visitor.Address = saved;
                }
                else
                {
                    _addressDAO.Update(visitor.Address);
                }
                visitor.AddressId = visitor.Address.Id;
            }

            return _visitorDAO.Update(visitor);
        }

        // ================= DELETE =================
        public Visitor? RemoveVisitor(int id)
        {
            // Voditi racuna o referencijalnim zavisnostima §8.12
            // (kupovine i lista zelja se brisu pre posetioca)
            var purchases = _purchaseDAO.GetByVisitor(id);
            foreach (var p in purchases)
                _purchaseDAO.Remove(p.Id);

            _visitorBookDAO.RemoveByVisitorId(id);

            return _visitorDAO.Remove(id);
        }

        // ================= READ =================
        public Visitor? GetById(int id)
        {
            var visitor = _visitorDAO.GetById(id);
            if (visitor != null)
                LoadRelations(visitor);

            return visitor;
        }

        public List<Visitor> GetAll()
        {
            var visitors = _visitorDAO.GetAll();
            LoadRelations(visitors);
            return visitors;
        }
        public Visitor? GetByCardNumber(string cardNumber)
        {
            var visitor = _visitorDAO.GetByCardNumber(cardNumber);
            if (visitor != null)
                LoadRelations(visitor);
            return visitor;
        }

        public List<Visitor> GetAll(int page, int pageSize, string sort, SortDirection dir)
        {
            var visitors = _visitorDAO.GetAll(page, pageSize, sort, dir);
            LoadRelations(visitors);
            return visitors;
        }



    }
}
