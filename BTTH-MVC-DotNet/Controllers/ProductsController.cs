using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using BTTH_MVC_DotNet.Models;

namespace BTTH_MVC_DotNet.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly QuanLySanPhamContext _context;
        private const string ApiBaseUrlProducts = "https://localhost:7096/api/Products";
        private const string ApiBaseUrlCatalogs = "https://localhost:7096/api/Catalogs";

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _context = new QuanLySanPhamContext();
        }
        public async Task<IActionResult> GetProductsByCatalog(int iddm)
        {
            var products = iddm == -1
                ? await _httpClient.GetFromJsonAsync<List<Product>>(ApiBaseUrlProducts) // Fetch all products
                : await _httpClient.GetFromJsonAsync<List<Product>>($"{ApiBaseUrlProducts}?catalogId={iddm}"); // Fetch by catalog ID

            return PartialView("_ListProduct", products);
        }



        public async Task<IActionResult> getListProduct(int iddm)
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>($"{ApiBaseUrlProducts}");
            return PartialView("_ListProduct", products);
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>($"{ApiBaseUrlProducts}");
            var catalogs = await _httpClient.GetFromJsonAsync<List<Catalog>>($"{ApiBaseUrlCatalogs}");


            ViewBag.listCatalog = catalogs;
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _httpClient.GetFromJsonAsync<Product>($"{ApiBaseUrlProducts}/{id}");
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            // Assuming you have a Catalog endpoint to get catalog list
            // You can replace with appropriate code for loading Catalog list
            ViewData["CatalogId"] = new SelectList(new List<SelectListItem>(), "Id", "Id");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrlProducts}", product);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CatalogId"] = new SelectList(new List<SelectListItem>(), "Id", "Id", product.CatalogId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _httpClient.GetFromJsonAsync<Product>($"{ApiBaseUrlProducts}/{id}");
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CatalogId"] = new SelectList(new List<SelectListItem>(), "Id", "Id", product.CatalogId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrlProducts}/{id}", product);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CatalogId"] = new SelectList(new List<SelectListItem>(), "Id", "Id", product.CatalogId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _httpClient.GetFromJsonAsync<Product>($"{ApiBaseUrlProducts}/{id}");
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiBaseUrlProducts}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
