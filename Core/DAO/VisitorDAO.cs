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
    * Ova klasa sluzi za:
    *  - upravljanje listom Visitor objekata
    *  - dodavanje, izmenu, brisanje i prikaz posetilaca
    *  - cuvanje i ucitavanje podataka iz fajla pomocu Storage klase
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

        //8.20. za paginaciju i sortiranje
        public List<Visitor> GetAll(int page, int pageSize, string sortCriteria, SortDirection sortDirection)
        {
            IEnumerable<Visitor> visitors = _visitors;

            visitors = sortCriteria switch
            {
                "MembershipCardNumber" or "BrojKartice"
                    => sortDirection == SortDirection.Ascending
                        ? visitors.OrderBy(v => v.MembershipCardNumber)       // leksikografski
                        : visitors.OrderByDescending(v => v.MembershipCardNumber),

                "FirstName" or "Ime"
                    => sortDirection == SortDirection.Ascending
                        ? visitors.OrderBy(v => v.FirstName)
                        : visitors.OrderByDescending(v => v.FirstName),

                "LastName" or "Prezime"
                    => sortDirection == SortDirection.Ascending
                        ? visitors.OrderBy(v => v.LastName)
                        : visitors.OrderByDescending(v => v.LastName),

                "MembershipYear" or "GodinaClanstva"
                    => sortDirection == SortDirection.Ascending
                        ? visitors.OrderBy(v => v.MembershipYear)             // numericko
                        : visitors.OrderByDescending(v => v.MembershipYear),

                "Status"
                    => sortDirection == SortDirection.Ascending
                        ? visitors.OrderBy(v => v.Status)
                        : visitors.OrderByDescending(v => v.Status),

                "AverageRating" or "ProsecnaOcena"
                    => sortDirection == SortDirection.Ascending
                        ? visitors.OrderBy(v => v.AverageRating)              // numericko
                        : visitors.OrderByDescending(v => v.AverageRating),

                _ => sortDirection == SortDirection.Ascending
                        ? visitors.OrderBy(v => v.Id)
                        : visitors.OrderByDescending(v => v.Id),
            };

            //8.20 Paginacija: do 16 entiteta po stranici
            return visitors.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }


    }
}
