using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Storage.Serialization;

namespace Core.Models
{
    public class Address : ISerializable
    {
        public int Id { get; set; }
        public string StreetName { get; set; } = string.Empty;
        public int Number { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public Address()
        {

        }
        public Address(string s, int n, string ci, string co)
        {
            StreetName = s;
            Number = n;
            City = ci;
            Country = co;
        }

        public Address(int id, string s, int n, string ci, string co) : this(s, n, ci, co)
        {
            Id = id;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            StreetName = values[1];
            Number = int.Parse(values[2]);
            City = values[3];
            Country = values[4];
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                StreetName ?? "",
                Number.ToString(),
                City ?? "",
                Country ?? ""
            };
        }

        public override string ToString()
        {
            return $"{StreetName} {Number}, {City}";
        }
    }
}
