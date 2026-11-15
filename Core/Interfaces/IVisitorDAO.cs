using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    internal interface IVisitorDAO
    {
        // ========== CRUD operations ========== //
        Visitor Add(Visitor visitor);
        Visitor? Update(Visitor visitor);
        Visitor? Remove(int Id);

        // ========== Prikaz metode ========== //
        Visitor? GetById(int Id);
        List<Visitor> GetAll();

        //za paginaciju i sortiranje
        List<Visitor> GetAll(int page, int pageSize, string sortCriteria, SortDirection sortDirection);


    }
}
