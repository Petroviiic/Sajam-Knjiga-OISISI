using Core.DAO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class AddressService
    {
        private AddressDAO addressDAO;

        public AddressService()
        {
            addressDAO = new AddressDAO();
        }

        public Address Add(Address address)
        {
            validate(address);
            return addressDAO.Add(address);
        }

        public Address? Update(Address address)
        {
            validate(address);
            return addressDAO.Update(address);
        }

        public Address? Remove(int id)
        {
            return addressDAO.Remove(id);
        }

        public Address? GetById(int id)
        {
            return addressDAO.GetById(id);
        }

        public List<Address> GetAll()
        {
            return addressDAO.GetAll();
        }

        private void validate(Address address)
        {
            if (string.IsNullOrWhiteSpace(address.StreetName))
                throw new Exception("Ime ulice je obavezno.");

            if (string.IsNullOrWhiteSpace(address.City))
                throw new Exception("Grad je obavezan.");

            if (string.IsNullOrWhiteSpace(address.Country))
                throw new Exception("Drzava je obavezna.");

            if (address.Number < 0)
                throw new Exception("Broj ulice nije validan broj.");
        }
    }
}
