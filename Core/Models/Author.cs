using Core.Storage.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Models
{
    public class Author : ISerializable
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        public Address? Address { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int AddressId { get; set; }
        public string IdCardNumber { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
        public Author()
        {
            
        }

        public Author(int id, string fn, string ln, DateTime dob, Address? address, string ph, string em, string icn, int yearsExp)
        {
            Id = id;
            FirstName = fn;
            LastName = ln;
            DateOfBirth = dob;
            Address = address;
            AddressId = address?.Id ?? 0;
            Phone = ph;
            Email = em;
            IdCardNumber = icn;
            YearsOfExperience = yearsExp;
        }

        public Author(string fn, string ln, DateTime dob, int addressId, string ph, string em, string icn, int yearsExp)
            : this(0, fn, ln, dob, null, ph, em, icn, yearsExp)
        {
            AddressId = addressId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            FirstName = values[1];
            LastName = values[2];
            DateOfBirth = DateTime.ParseExact(values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            AddressId = int.Parse(values[4]);
            Phone = values[5];
            Email = values[6];
            IdCardNumber = values[7];
            YearsOfExperience = int.Parse(values[8]);
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                FirstName ?? "",
                LastName ?? "",
                DateOfBirth.ToString("yyyy-MM-dd"),
                AddressId.ToString(),
                Phone ?? "",
                Email ?? "",
                IdCardNumber ?? "",
                YearsOfExperience.ToString()
            };
        }

        public override string ToString()
        {
            string addressStr = Address != null ? Address.ToString() : $"AddressId: {AddressId}";
            return $"Author [ID: {Id}, Name: {FirstName} {LastName}, DOB: {DateOfBirth:yyyy-MM-dd}, " +
                   $"Address: {addressStr}, Phone: {Phone}, Email: {Email}, " +
                   $"IdCard: {IdCardNumber}, YearsOfExperience: {YearsOfExperience}]";
        }
    }
}
