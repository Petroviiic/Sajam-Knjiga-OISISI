using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    internal interface IPurchaseDAO
    {
        Purchase? Update(Purchase purchase);
        Purchase? Remove(int Id);
        Purchase? GetById(int Id);
        List<Purchase> GetAll(); //prikaz kupljenih knjiga

        Purchase Add(Purchase purchase);
    }
}
