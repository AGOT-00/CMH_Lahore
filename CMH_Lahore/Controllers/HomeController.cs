using CMH_Lahore.DB;
using CMH_Lahore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMH_Lahore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Database _DB;

        public HomeController(ILogger<HomeController> logger, Database DB)
        {
            _logger = logger;
            this._DB = DB;
        }
        [HttpGet]

        public IActionResult Index()
        {
            //return RedirectToAction("InitialScreen", "Home");
            //return RedirectToAction("Login", "Admin");
            return View();
        }

        [HttpGet]
        public IActionResult RegisterComplaint()
        {
            Complaint Obj = new();
            return View(Obj);
        }

        [HttpPost]
        public IActionResult RegisterComplaint(Complaint Obj)
        {
            if (Obj != null)
            {
                Obj.ComplaintType = "None";
                var Check =  _DB.Complaints.Add(Obj);
                var maxceck = _DB.SaveChanges();
            }
            return View();
        }

        public IActionResult submitcomplaint(int? complaintno)
        {
            return View();
        }

        public IActionResult GetStatus()
        {
            return View();
        }

        public async Task<IActionResult> saveAsync(IFormFile audio_data)
        {
            var currentDir = Directory.GetCurrentDirectory();

            // Construct the full path to where the file should be saved
            var filePath = Path.Combine(currentDir, "audio", audio_data.FileName);
            Console.WriteLine(filePath);
            // Open a stream to write the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Copy the contents of the file to the stream
                await audio_data.CopyToAsync(fileStream);
            }

            return Redirect("");
        }

        public bool SubmitVoicedataAsync(IFormFile audio_data)
        {
            if (audio_data != null)
            {
                saveAsync(audio_data);
            }
            else
            {
                return false;
            }
            return true;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}