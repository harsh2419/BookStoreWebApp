using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        void SaveChanges();
    }
}
