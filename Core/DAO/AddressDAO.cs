using Core.Interfaces;
using Core.Models;
using Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.DAO
{
    public class AddressDAO : IAddressDAO
    {
        
        private static readonly Storage<Address> storage = new Storage<Address>(FileConstants.AddressesFile);
        private static readonly List<Address> addresses = storage.Load();

        public AddressDAO() { }

        private int GenerateId()
        {
            return addresses.Count == 0 ? 1 : addresses.Max(a => a.Id) + 1;
        }
        public Address Add(Address address)
        {
            address.Id = GenerateId();
            addresses.Add(address);
            storage.Save(addresses);
            return address;
        }

        public Address? Update(Address address)
        {
            Address? oldAddress = GetById(address.Id);
            if (oldAddress == null) return null;

            oldAddress.StreetName = address.StreetName;
            oldAddress.City = address.City;
            oldAddress.Country = address.Country;
            oldAddress.Number = address.Number;

            storage.Save(addresses);
            return oldAddress;
        }
        public Address? Remove(int Id)
        {
            Address? address = GetById(Id);
            if (address == null) return null;

            addresses.Remove(address);
            storage.Save(addresses);
            return address;

        }
        public Address? GetById(int Id)
        {
            return addresses.Find(a => a.Id == Id);
        }
        public List<Address> GetAll()
        {
            return addresses;
        }
    }
}
