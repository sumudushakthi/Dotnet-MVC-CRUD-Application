using Microsoft.AspNetCore.Mvc;
using MvcCrudApp.Models;
using MvcCrudApp.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Add this using directive

namespace MvcCrudApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetAllProductsAsync());
        }

        // GET: Products/Details/P001
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price")] Product product)
        {
            Console.WriteLine($"Received Product Name: {product.Name}");
            Console.WriteLine($"Received Product Price: {product.Price}");
            if (ModelState.IsValid)
            {
                // Generate ID
                product.Id = await GenerateProductIdAsync();
                // Print the generated ID to the console
                Console.WriteLine($"Generated Product ID: {product.Id}");
                await _productService.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }

            // Log ModelState errors for debugging
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                }
            }
            return View(product);
        }


        // Method to generate a new product ID
        private async Task<string> GenerateProductIdAsync()
        {
            var allProducts = await _productService.GetAllProductsAsync();
            var lastProduct = allProducts.OrderByDescending(p => p.Id).FirstOrDefault();

            if (lastProduct == null || string.IsNullOrEmpty(lastProduct.Id))
            {
                return "P001";
            }

            var numberPart = lastProduct.Id.Substring(1);
            if (int.TryParse(numberPart, out int lastNumber))
            {
                return $"P{(lastNumber + 1).ToString("D3")}";
            }

            throw new InvalidOperationException("Unable to generate new product ID.");
        }

        // GET: Products/Edit/P001
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/P001
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateProductAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/P001
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/P001
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductExists(string id)
        {
            return await _productService.GetProductByIdAsync(id) != null;
        }
    }
}
