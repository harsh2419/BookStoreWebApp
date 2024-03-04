using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.Interfaces;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using NuGet.Packaging;

namespace BulkyWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> Products = _unitOfWork.ProductRepository.GetAll("Category").ToList();
            return View(Products);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name,
                }),
                Product = new Product() { }
            };
            
            if (!(id == null || id ==0))
            {
                Product foundProduct = _unitOfWork.ProductRepository.Get(p => p.Id == id, "Category");
                if (foundProduct == null)
                {
                    return NotFound();
                }
                productVM.Product = foundProduct;
            }
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(_webHostEnvironment.WebRootPath + @"\images\product");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.CreateNew))
                    {
                        file.CopyTo(fileStream);
                    }

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl)) {
                        //Delete the old image
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(productVM.Product);
                    TempData["success"] = "Product added successfully!";
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully!";
                }
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.CategoryRepository.GetAll("Category").Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name,
                });
                TempData["error"] = "Error! One or more Validation errors occured!";
            }
            return View(productVM);
        }

        #region APICalls
        public IActionResult GetAll()
        {
            List<Product> Products = _unitOfWork.ProductRepository.GetAll("Category").ToList();
            return Json(new {data = Products, isSucess = true});
        }

        public IActionResult DeleteProduct(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { isSuccess = false, Message = "Invalid product Id !" });
            }

            Product? foundProduct = _unitOfWork.ProductRepository.Get(p => p.Id == id, "Category");

            if (foundProduct == null)
            {
                return Json(new { isSuccess = false, Message = "No Product found !" });
            }

            if (!string.IsNullOrEmpty(foundProduct.ImageUrl))
            {
                //Delete the old image
                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, foundProduct.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _unitOfWork.ProductRepository.Remove(foundProduct);
            _unitOfWork.SaveChanges();
            return Json(new { isSuccess = true, Message = "Product deleted succssfully" });
        }
        #endregion
    }
}
