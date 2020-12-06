using Bogus;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalog.Domain;
using NerdStore.WebApplication.MVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.WebApplication.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("add-products")]
        public async Task AddProducts()
        {
            var faker = new Faker();

            var images = new Dictionary<int, string>
            {
                { 2, "camiseta1.jpg" },
                { 3, "camiseta2.jpg" },
                { 4, "camiseta3.jpg" },
                { 5, "camiseta4.jpg" },
                { 6, "caneca1.jpg" },
                { 7, "caneca2.jpg" },
                { 8, "caneca3.jpg" },
                { 9, "caneca4.jpg" }
            };

            for (int code = 2; code < 9; code++)
            {
                var category = new Category(faker.Random.Word(), code);

                var product = new Product(
                    faker.Commerce.Product(),
                    faker.Commerce.ProductDescription(),
                    true,
                    faker.Random.Decimal(50, 1000),
                    category.Id,
                    DateTime.UtcNow,
                    images.Where(o => o.Key == code).Select(x => x.Value).Single(),
                    new Dimensions(faker.Random.Decimal(1, 10), faker.Random.Decimal(1, 10), faker.Random.Decimal(1, 10))
                );

                product.Restock(faker.Random.Int(1, 15));

                _productRepository.Add(category);
                _productRepository.Add(product);
            }

            await _productRepository.UnitOfWork.Commit();
        }
    }
}
