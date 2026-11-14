using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    internal interface IAddressDAO
    {
        Address Add(Address address);
        Address? Update(Address address);
        Address? Remove(int Id);
        Address? GetById(int Id);
        List<Address> GetAll();


    }
}
