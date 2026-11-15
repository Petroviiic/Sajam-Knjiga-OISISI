using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    internal interface IPublisherDAO
    {
        int GenerateId();
        Publisher Add(Publisher publisher);
        Publisher? Update(Publisher publisher);
        Publisher? Remove(int id);
        Publisher? GetById(int id);
        List<Publisher> GetAll();

    }
}
