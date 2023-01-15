using CMH_Lahore.DB;
using CMH_Lahore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

//Real Time Deparment Remains

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
            return View("RegisterComplaint", Obj);
        }

        [HttpPost]
        [Route("Home/RegisterComplaint")]
        public IActionResult submitcomplaint(Complaint Obj)
        {
            if (Obj != null)
            {
                try
                {
                    Obj.ComplaintType = "None";
                    var Check = _DB.Complaints.Add(Obj);
                    var maxceck = _DB.SaveChanges();
                    return View(Obj.id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return View();
        }

        public IActionResult VoiceSubmissionComplete()
        {
            //if (VoiceRedirectionSet != 0)
            //{
            //    int VoiceBasedSubmissionID = VoiceRedirectionSet;
            //    VoiceRedirectionSet = 0;
            //    return View("submitcomplaint", VoiceBasedSubmissionID);
            //}

            var status = HttpContext.Session.GetInt32("VoiceIDLatest");
            HttpContext.Session.SetInt32("VoiceIDLatest", 0);

            return View("submitcomplaint", status);
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            return View();
        }

        [Route("Home/GetStatus/")]
        [HttpPost]
        public IActionResult complaintDetails(int complaintNumber)
        {
            var Comp = _DB.Complaints.Find(complaintNumber);
            return View("complaintDetails", Comp);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task saveAsync(IFormFile audio_data, int Cid)
        {
            var currentDir = Directory.GetCurrentDirectory();

            // Construct the full path to where the file should be saved
            var filePath = Path.Combine(currentDir, "audio", Cid.ToString() + ".mp4");
            //Console.WriteLine(filePath);
            // Open a stream to write the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Copy the contents of the file to the stream
                await audio_data.CopyToAsync(fileStream);


                //VoiceRedirectionSet = Cid;
            }
        }

        public async Task<bool> SubmitVoicedataAsync(Complaint Obj, IFormFile audio_data)
        {
            using StreamWriter file = new("List.txt", append: true);
            await file.WriteLineAsync(Obj.DocName + " IS ");

            if (audio_data != null)
            {
                if (Obj != null)
                {
                    try
                    {
                        Obj.ComplaintType = "None";
                        Obj.ComplaintDesc = "VoiceData";
                        _DB.Complaints.Add(Obj);
                        _DB.SaveChanges();

                        await file.WriteLineAsync(Obj.id.ToString());

                        saveAsync(audio_data, Obj.id);

                        HttpContext.Session.SetInt32("VoiceIDLatest", Obj.id);

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        await file.WriteLineAsync(e.Message);
                        await file.WriteLineAsync(e.ToString());
                    }
                }
            }
            return false;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}