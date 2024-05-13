using GP_Certifieringsuppgift_del_1.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GP_Certifieringsuppgift_del_1.Controllers
{
    public class JobsController : Controller
    {
        public IActionResult Index()
        {
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Work>("works");

            List<Work> works = collection.Find(w => true).ToList(); 

            return View(works);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Work work)
        {
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Work>("works");

            collection.InsertOne(work);
            return RedirectToAction("Index");
        }

        public IActionResult ShowJob(string Id)
        {
            try
            {
                ObjectId workId = new ObjectId(Id);
                MongoClient dbClient = new MongoClient();
                var database = dbClient.GetDatabase("time_logging_service");
                var collection = database.GetCollection<Work>("works");

                Work work = collection.Find(w => w.Id == workId).FirstOrDefault();

                if (work == null)
                {
                    TempData["ErrorMessage"] = "Work not found.";
                }

                return View(work);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return View(); 
            }
        }

        public IActionResult DeleteJob(string Id)
        {
            ObjectId workId = new ObjectId(Id);
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Work>("works");

            collection.DeleteOne(w => w.Id == workId);
            return RedirectToAction("Index");
        }

        public IActionResult EditJob(string Id)
        {
            ObjectId workId = new ObjectId(Id);
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Work>("works");

            Work work = collection.Find(w => w.Id == workId).FirstOrDefault();

            return View(work);
        }

        [HttpPost]
        public IActionResult EditJob(string Id, Work work)
        {
            ObjectId workId = new ObjectId(Id);
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Work>("works");

            work.Id = workId; 
            collection.ReplaceOne(w => w.Id == work.Id, work);
            return RedirectToAction("Index");
        }
    }
}
