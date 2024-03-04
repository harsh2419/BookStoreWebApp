using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Interfaces;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product product)
        {
            Product? productFromDb = _db.Products.FirstOrDefault(x => x.Id == product.Id);

            if (productFromDb != null)
            {
                productFromDb.ISBN = product.ISBN;
                productFromDb.Name = product.Name;
                productFromDb.Description = product.Description;
                productFromDb.Price = product.Price;
                productFromDb.ListPrice = product.ListPrice;
                productFromDb.Price50 = product.Price50;
                productFromDb.Price100 = product.Price100;
                productFromDb.Author = product.Author;
                productFromDb.CategoryId = product.CategoryId;
                
                if(product.ImageUrl != null)
                {
                    productFromDb.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
