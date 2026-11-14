using Core.Models;
using Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Interfaces;


namespace Core.DAO
{
    /*
    * VisitorDAO – Data Access Object
    * --------------------------------
    * Ova klasa je zadužena za:
    *  - upravljanje listom Visitor objekata
    *  - dodavanje, izmenu, brisanje i prikaz posetilaca
    *  - čuvanje i učitavanje podataka iz fajla pomoću Storage klase
    */
    public class VisitorDAO : IVisitorDAO
    {
        private readonly Storage<Visitor> _storage;
        private readonly List<Visitor> _visitors;

        public VisitorDAO()
        {
            _storage = new Storage<Visitor>("visitors.txt");
            _visitors = _storage.Load();
        }

        public int GenerateId()
        {
            return _visitors.Count == 0 ? 1 : _visitors.Max(v => v.Id) + 1;
        }

        // ================== CRUD metode ================= //

        public Visitor Add(Visitor visitor)
        {
            visitor.Id = GenerateId();
            _visitors.Add(visitor);
            _storage.Save(_visitors);
            return visitor;
        }

        public Visitor? Update(Visitor visitor)
        {
            Visitor? oldVisitor = GetById(visitor.Id);

            if (oldVisitor == null)
                return null;

            oldVisitor.FirstName = visitor.FirstName;
            oldVisitor.LastName = visitor.LastName;
            oldVisitor.DateOfBirth = visitor.DateOfBirth;
            oldVisitor.AddressId = visitor.AddressId;
            oldVisitor.Phone = visitor.Phone;
            oldVisitor.Email = visitor.Email;
            oldVisitor.MembershipCardNumber = visitor.MembershipCardNumber;
            oldVisitor.MembershipYear = visitor.MembershipYear;
            oldVisitor.Status = visitor.Status;
            oldVisitor.AverageRating = visitor.AverageRating;

            _storage.Save(_visitors);
            return oldVisitor;
        }

        public Visitor? Remove(int id)
        {
            Visitor? visitor = GetById(id);

            if (visitor == null)
                return null;

            _visitors.Remove(visitor);
            _storage.Save(_visitors);
            return visitor;
        }

        public Visitor? GetById(int id)
        {
            return _visitors.Find(v => v.Id == id);
        }

        public List<Visitor> GetAll()
        {
            return _visitors;
        }

        public Visitor? GetByCardNumber(string cardNumber)
        {
            return _visitors.FirstOrDefault(v => v.MembershipCardNumber == cardNumber);
        }
    }
}
