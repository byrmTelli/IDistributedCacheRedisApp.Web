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

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images/dashboard.png");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image", imageByte);

            return View();
        }
        
        public IActionResult ImageShow()
        {
            byte[] resimByte = _distributedCache.Get("image");
            return File(resimByte,"image/png");
        }
    }
}
