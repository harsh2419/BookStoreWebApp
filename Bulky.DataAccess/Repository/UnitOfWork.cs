using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository CategoryRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext db) {
            _db = db;
            CategoryRepository = new CategoryRepository(_db);
            ProductRepository = new ProductRepository(_db);
        }
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
