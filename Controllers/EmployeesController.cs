using GP_Certifieringsuppgift_del_1.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;



namespace GP_Certifieringsuppgift_del_1.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Employee>("employees");

            List<Employee> employees = collection.Find(e => true).ToList();

            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {

            if (DateTime.TryParse(employee.Birthyear.ToString("yyyy-MM-dd"), out DateTime birthdate))
            {
                employee.Birthyear = birthdate;
            }
            else
            {
               
                ModelState.AddModelError("Birthyear", "Invalid date format");
                return View(employee);
            }

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Employee>("employees");

            collection.InsertOne(employee);

            return Redirect("/Employees/Index");
        }

        public IActionResult ShowEmployee(string Id)
        {
            ObjectId employeeId = new ObjectId(Id); 
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Employee>("employees");

            Employee employee = collection.Find(e => e.Id == employeeId).FirstOrDefault();

            var workCollection = database.GetCollection<Work>("works");
            ViewBag.Works = workCollection.Find(w => true).ToList();

            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;

            return View(employee);
        }

        public IActionResult DeleteEmployee(string Id)
        {
            ObjectId employeeId = new ObjectId(Id);
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Employee>("employees");

            collection.DeleteOne(e => e.Id == employeeId);
            return RedirectToAction("Index");
        }

        public IActionResult EditEmployee(string Id)
        {
            ObjectId employeeId = new ObjectId(Id);
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Employee>("employees");

            Employee employee = collection.Find(e => e.Id == employeeId).FirstOrDefault();

            return View(employee);
        }
        [HttpPost]
        public IActionResult EditEmployee(string Id, Employee employee)
        {
            ObjectId employeeId = new ObjectId(Id);
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var collection = database.GetCollection<Employee>("employees");

            employee.Id = employeeId;
            collection.ReplaceOne(e => e.Id == employee.Id, employee);
            return Redirect("/Employees/Index");
        }

        [HttpPost]
        public IActionResult AssignWork(string id, string workId)
        {
            try
            {
                ObjectId employeeId = new ObjectId(id);
                ObjectId workObjectId = new ObjectId(workId);

                MongoClient dbClient = new MongoClient();
                var database = dbClient.GetDatabase("time_logging_service");
                var employeeCollection = database.GetCollection<Employee>("employees");

                var filter = Builders<Employee>.Filter.Eq(e => e.Id, employeeId);
                var update = Builders<Employee>.Update.Set(e => e.AssignedWorkId, workObjectId);

                employeeCollection.UpdateOne(filter, update);

                TempData["SuccessMessage"] = "Job assigned successfully";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "No jobs available to assign";
                
            }

            return RedirectToAction("ShowEmployee", new { id = id });
        }

        [HttpPost]
        public IActionResult RemoveAssignedWork(string id)
        {
            ObjectId employeeId = new ObjectId(id);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("time_logging_service");
            var employeeCollection = database.GetCollection<Employee>("employees");

            var filter = Builders<Employee>.Filter.Eq(e => e.Id, employeeId);
            var update = Builders<Employee>.Update.Set(e => e.AssignedWorkId, ObjectId.Empty);

            employeeCollection.UpdateOne(filter, update);

            return RedirectToAction("ShowEmployee", new { id = id });
        }
    }
}
