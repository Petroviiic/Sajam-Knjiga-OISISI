using Core.Storage.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{

    public enum VisitorStatus{Regular, VIP}
    public class Visitor : ISerializable
    {
        //Osnovni podaci
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        // Adresa se ne serijalizuje direktno, već se koristi AddressId u podklasama
        public Address? Address { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;


        public int Id { get; set; }
        public string MembershipCardNumber { get; set; } = string.Empty;
        public int MembershipYear { get; set; }
        public VisitorStatus Status { get; set; }
        public double AverageRating { get; set; }

        // Foreign Key koji se čuva u visitor.txt
        public int AddressId { get; set; }

       
        // Kupovine (NE SERIJALIZUJU SE)
        public List<Purchase> Purchases { get; set; } = new();

        // ================= CONSTRUCTORS =================


        public Visitor() : this(0, "", "", DateTime.MinValue, 0, "", "", "", 0, VisitorStatus.Regular, 0.0) { }

        public Visitor(
            int id,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            int addressId,
            string phone,
            string email,
            string membershipCardNumber,
            int membershipYear,
            VisitorStatus status,
            double averageRating)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Address = null;
            AddressId = addressId;
            Phone = phone;
            Email = email;
            MembershipCardNumber = membershipCardNumber;
            MembershipYear = membershipYear;
            Status = status;
            AverageRating = averageRating;
        }


        // ================= CSV SERIALIZATION =================

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                FirstName,
                LastName,
                DateOfBirth.ToString("yyyy-MM-dd"),
                AddressId.ToString(), // SAMO ID
                Phone,
                Email,
                MembershipCardNumber,
                MembershipYear.ToString(),
                Status.ToString(),
                AverageRating.ToString(CultureInfo.InvariantCulture)
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            FirstName = values[1];
            LastName = values[2];
            DateOfBirth = DateTime.ParseExact(values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (!int.TryParse(values[4], out int addressId))
                addressId = 0;

            AddressId = addressId;

            Phone = values[5];
            Email = values[6];
            MembershipCardNumber = values[7];
            MembershipYear = int.Parse(values[8]);

            Status = Enum.Parse<VisitorStatus>(values[9]);
            AverageRating = double.Parse(values[10], CultureInfo.InvariantCulture);
        }

        // ================= DEBUG PRINT =================

        public override string ToString()
        {
            string addressStr = Address != null
                ? Address.ToString()
                : $"AddressId: {AddressId}";

            string purchaseStr = Purchases.Count == 0
                ? "None"
                : string.Join(", ",
                    Purchases.Select(p => $"[BookID: {p.BookId}, Rating: {p.Rating}]"));

            return $"Visitor [ID: {Id}, Name: {FirstName} {LastName}, DOB: {DateOfBirth:yyyy-MM-dd}, " +
                   $"Address: {addressStr}, Phone: {Phone}, Email: {Email}, " +
                   $"Membership Card: {MembershipCardNumber}, Membership Year: {MembershipYear}, " +
                   $"Status: {Status}, Average Rating: {AverageRating:F2}]\n" +
                   $"Purchased Books: {purchaseStr}";
        }

    }
}
