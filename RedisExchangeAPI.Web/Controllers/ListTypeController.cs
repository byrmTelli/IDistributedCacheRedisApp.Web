using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;

        private string listKey = "usernames";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> usernames  = new List<string>();
            if(_db.KeyExists(listKey))
            {

                _db.ListRange(listKey).ToList().ForEach(x =>
                {
                    usernames.Add(x.ToString());
                });
            }


            return View(usernames);
        }


        [HttpPost]
        public IActionResult Add(string name)
        {
            //adds data to end, listLeftPush adds data to first index
            _db.ListRightPush(listKey,name);


            return RedirectToAction("Index");
        }

        public IActionResult Delete(string name)
        {
            _db.ListRemove(listKey,name);
            return RedirectToAction("Index");
        }
    }
}
