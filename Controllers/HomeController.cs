using GP_Certifieringsuppgift_del_1.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace GP_Certifieringsuppgift_del_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Employee>("employees");

            ViewBag.Employees = collection.Find(e => true).ToList();
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

        [HttpPost]
        public IActionResult ClockIn(ObjectId employeeId)
        {
            var filter = Builders<Employee>.Filter.Eq(e => e.Id, employeeId);
            var update = Builders<Employee>.Update.Set(e => e.ClockInTime, DateTime.Now);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Employee>("employees");

            collection.UpdateOne(filter, update);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClockOut(ObjectId employeeId)
        {
            var filter = Builders<Employee>.Filter.Eq(e => e.Id, employeeId);
            var update = Builders<Employee>.Update.Set(e => e.ClockOutTime, DateTime.Now);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Employee>("employees");

            collection.UpdateOne(filter, update);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteTimes(ObjectId employeeId)
        {
            var filter = Builders<Employee>.Filter.Eq(e => e.Id, employeeId);
            var update = Builders<Employee>.Update
                .Set(e => e.ClockInTime, null)
                .Set(e => e.ClockOutTime, null);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Employee>("employees");

            collection.UpdateOne(filter, update);

            return RedirectToAction("Index");
        }

    }
}
