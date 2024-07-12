using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json.Serialization;
using Newtonsoft.Json;



namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductController(IDistributedCache disributedCache)
        {
            _distributedCache = disributedCache;
        }
        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddSeconds(15);

            Product product = new Product { Id=1,Name="Kalem",Price = 100};
            string jsonProduct = JsonConvert.SerializeObject(product);

            _distributedCache.SetStringAsync("product:1", jsonProduct);
            return View();
        }


        public IActionResult Show()
        {
            var jsonProduct = _distributedCache.GetString("product:1");
            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.jsonProduct = product;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("product:1");
            return View();
        }
    }
}
