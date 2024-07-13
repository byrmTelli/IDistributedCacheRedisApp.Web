using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
        }
        public IActionResult Index()
        {

            var db = _redisService.GetDb(0);
            db.StringSet("kullanici", "Bayram Telli");
            db.StringSet("visitor", 100);


            return View();
        }

        public IActionResult Show()
        {
            var db = _redisService.GetDb(0);
            var kullanici = db.StringGetRange("kullanici",0,3);
            var visitor = db.StringIncrement("visitor",15);



            ViewBag.kullanici = kullanici;
            ViewBag.visitor = visitor;


            return View();
        }
    }
}
