using CMH_Lahore.DB;
using CMH_Lahore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//Login Issue
//Feedback Upload Status

namespace CMH_Lahore.Controllers
{
    //Create a Complete SignINManager to Handle SignInASYNC and SignOutASYNC
    //public class AdminController : Controller
    //{
    //    private readonly Database _db;
    //    public AdminController(Database db)
    //    {
    //        _db = db;
    //    }
    //    public IActionResult Index()
    //    {
    //        return View();
    //    }
    //    public IActionResult Login()
    //    {
    //        return View();
    //    }
    //    [HttpPost]
    //    public IActionResult Login(Admin admin)
    //    {
    //        var admindata = _db.Admins.Find(admin.ID);
    //        if (admindata != null)
    //        {
    //            if (admindata.verifypassword(admin.Password))
    //            {
    //                //Create a Claim
    //                var claims = new[]
    //                {
    //                    new Claim(ClaimTypes.Name,admindata.Name),
    //                    new Claim(ClaimTypes.Role,admindata.getadmintype()),
    //                    new Claim(ClaimTypes.NameIdentifier,admindata.ID),
    //                    new Claim(ClaimTypes.MobilePhone,admindata.Phone),
    //                    new Claim(ClaimTypes.UserData,admindata.AdminType),
    //                    new Claim(ClaimTypes.GroupSid,admindata.rank.ToString()),
    //                    new Claim(ClaimTypes.Sid,admindata.department.ToString())
    //                };
    //                //Create a Identity
    //                var identity = new ClaimsIdentity(claims, "Admin");
    //                //Create a Principal
    //                var principal = new ClaimsPrincipal(identity);
    //                //Sign In
    //                HttpContext.SignInAsync(principal);
    //                return RedirectToAction("Index", "Home");
    //            }
    //            else
    //            {
    //                ViewBag.Message = "Password is Incorrect";
    //                return View();
    //            }
    //        }
    //        else
    //        {
    //            ViewBag.Message = "Admin ID is Incorrect";
    //            return View();
    //        }
    //    }
    //    public IActionResult Logout()
    //    {
    //        HttpContext.SignOutAsync();
    //        return RedirectToAction("Login", "Admin");
    //    }
    //    public IActionResult Register()
    //    {
    //        return View();
    //    }
    //    [HttpPost]
    //    public IActionResult Register(Admin admin)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _db.Admins.Add(admin);
    //            _db.SaveChanges();
    //            return RedirectToAction("Login", "Admin");
    //        }
    //        return View();
    //    }
    //    public IActionResult Feedback()
    //    {
    //        return View();
    //    }
    //    [HttpPost]
    //    public IActionResult Feedback(Feedback feedback)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _db.Feedbacks.Add(feedback);
    //            _db.SaveChanges();
    //            return RedirectToAction("Index", "Home");
    //        }
    //        return View();
    //    }
    //}




    public class AdminController : Controller
    {
        private readonly Database _DB;
        static Admin? SignedIn;

        public IActionResult Index()
        {
            Admin? SignedIna = null;
            var userJson = HttpContext.Session.GetString("loggedInUser");

            if (SignedIna == null)
            {
                if (userJson != null)
                {
                    Admin user = JsonConvert.DeserializeObject<Admin>(userJson);
                    SignedIna = AdminFactory.GetAdmin(user);

                    return View(SignedIna);
                }

            }
            return NotFound();
        }

        public AdminController(Database DB)
        {

            //if (HttpContext.Session.IsAvailable) {

            //    var userJson = HttpContext.Session.GetString("loggedInUser");

            //    if (SignedIn == null)
            //    {
            //        if (userJson != null)
            //        {
            //            Admin user =
            //            SignedIn = AdminFactory.GetAdmin(JsonConvert.DeserializeObject<Admin>(userJson));
            //        }
            //    }
            //}

            _DB = DB;
        }

        [HttpGet]
        public IActionResult Login()
        {
            AdminDT obj = new();
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminDT admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var objs = _DB.Admins.FirstOrDefault(u => u.ID == admin.ID && u.Password == admin.Password);

                    if (objs != null)
                    {
                        //var Identity = new ClaimsIdentity(new[] {
                        //    new Claim(ClaimTypes.Actor, objs),
                        //    new Claim(ClaimTypes.Role, "Admin")
                        //}, "Admin Identity"); 
                        SignedIn = AdminFactory.GetAdmin(objs);
                        HttpContext.Session.SetString("loggedInUser", JsonConvert.SerializeObject(SignedIn));

                        Admin user = JsonConvert.DeserializeObject<Admin>(HttpContext.Session.GetString("loggedInUser"));

                        return RedirectToAction("Complaints", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("Login-Error", "Wrong Credientials");
                    }
                }
                catch (Exception e)
                {
                    SignedIn = null;
                    ModelState.AddModelError("Login-Error", "Error Detected");
                }

            }
            return View(admin);
        }

        public IActionResult Logout()
        {
            SignedIn = null;
            return RedirectToAction("Login", "Admin");
        }

        public IActionResult Department()
        {
            IEnumerable<Department> DepartmentList = _DB.Departments.ToList();
            return View(DepartmentList);
        }

        public IActionResult DeleteDepartment(int? id)
        {
            if (id > 4)
            {
                var DepartmentObject = _DB.Departments.Find(id);
                if (DepartmentObject != null)
                {
                    _DB.Departments.Remove(DepartmentObject);
                    _DB.SaveChanges();
                }
            }

            return RedirectToAction("Department", "Admin");
        }

        public IActionResult AddDepartment()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Changepswd()
        {
            if (SignedIn != null)
            {
                passwordchanger Obj = new();
                return View(Obj);
            }
            return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult Changepswd(passwordchanger Obj)
        {
            if (SignedIn != null)
            {
                Admin? AdminAccount = _DB.Admins.Find(SignedIn.ID);
                if (AdminAccount != null && AdminAccount.verifypassword(Obj.OldPassword))
                {
                    if (Obj.NewPassword == Obj.ConfirmPassword)
                    {
                        AdminAccount.Password = Obj.NewPassword;
                        _DB.SaveChanges();
                        ModelState.AddModelError("Password-Changed", "Your Password Has Been Changed");

                    }
                    else
                    {
                        ModelState.AddModelError("Password-Error", "New Password and Confirm Password do not match");
                    }
                    return RedirectToAction("Logout", "Admin");
                }
                else
                {
                    ModelState.AddModelError("Password-Error", "Old Password is incorrect");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Complaints(IEnumerable<Complaint> ComplaintList, string filter = "")
        {
            if (SignedIn != null)
            {
                //IEnumerable<Complaint> 
                ComplaintList = SignedIn.GetListofComplaint(_DB);

                //_DB.Complaints.ToList();

                //switch (filter)
                //{
                //    case "DateASC":
                //        ComplaintList = ComplaintList.OrderBy(c => c.DOI);
                //        break;
                //    case "Status":
                //        ComplaintList = ComplaintList.OrderBy(c => c.status);
                //        break;
                //}

                return View(ComplaintList);
                return View();
            }
            return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult Complaints(int id, IEnumerable<Complaint> Listing, string filter = "")
        {
            if (SignedIn != null)
            {

                IEnumerable<Complaint> ComplaintList = SignedIn.GetListofComplaint(_DB);//_DB.Complaints.ToList();

                switch (filter)
                {
                    case "DateASC":
                        ComplaintList = ComplaintList.OrderBy(c => c.DOI);
                        break;
                    case "Status":
                        ComplaintList = ComplaintList.OrderBy(c => c.status);
                        break;
                }

                return View(ComplaintList);
                return View();
            }
            return RedirectToAction("Login", "Admin");
        }

        [HttpGet]
        public IActionResult ComplaintDetail(int? ID)
        {
            if (SignedIn != null)
            {
                var DbObj = _DB.Complaints.Find(ID);
                if (DbObj != null)
                {
                    IEnumerable<Department> ComplaintList = _DB.Departments.ToList();
                    complaintdetaildt Obj = new(DbObj, ComplaintList, SignedIn.getadmintype(), _DB.Explainations.Find(ID), _DB.comments.Find(ID));

                    return View(Obj);
                }
            }
            return NotFound();
        }

        protected int statuscheck(int status)
        {
            switch (SignedIn.getadmintype())
            {
                case "ComplaintOfficer":
                    if (status == 2 || status == 4)
                    {
                        status = 0;
                    }
                    return status;
                    break;
                case "AssistantCommandent":
                    return status;
                    break;
            }
            return 0;
        }

        [HttpPost]
        public IActionResult ComplaintDetail(int id, int DepartmentSelection, string Explain, string ComplaintType, int CurrentComplaintStatus)
        {
            if (SignedIn != null)
            {
                var DbObj = _DB.Complaints.Find(id);

                if (DbObj != null)
                {
                    DbObj.ComplaintType = ComplaintType;
                    DbObj.Department = DepartmentSelection;

                    DbObj.status = statuscheck(CurrentComplaintStatus);

                    explaination entry = _DB.Explainations.Find(id);

                    if (entry == null)
                    {
                        entry = new();
                        _DB.Explainations.Add(entry);
                    }

                    entry.id = id;
                    entry.explain = Explain;

                    _DB.SaveChanges();
                    return RedirectToAction("Complaints", "Admin");
                }
            }
            return NotFound();
        }

        public IActionResult savecommandentcomment(int id, string Comment)
        {
            if (SignedIn != null)
            {
                var DbObj = _DB.Complaints.Find(id);

                if (DbObj != null)
                {

                    comment entry = _DB.comments.Find(id);

                    if (entry == null)
                    {
                        entry = new();
                        _DB.comments.Add(entry);
                    }

                    entry.id = id;
                    entry.commenterid = SignedIn.ID;
                    entry.comments = Comment;

                    _DB.SaveChanges();
                }
            }
            return Redirect(Request.GetTypedHeaders().Referer.ToString());
        }

        public IActionResult sendbacktoCO(int? id)
        {
            if (SignedIn != null && SignedIn.getadmintype() == "AssistantCommandent" && id != null)
            {
                var DbObj = _DB.Complaints.Find(id);

                if (DbObj != null)
                {
                    DbObj.status = 0;
                    DbObj.ComplaintType = "None";
                    _DB.SaveChanges();
                }
            }
            return RedirectToAction("Complaints", "Admin");
        }

        public IActionResult whoamiloggedin()
        {
            return View(SignedIn);
        }

        public IActionResult complaintstatus(int? id)
        {
            if (SignedIn != null && id != null)
            {
                var DbObj = _DB.Complaints.Find(id);
                if (DbObj != null)
                {
                    if (DbObj.status == 1)
                    {
                        DbObj.status = 0;
                        _DB.SaveChanges();
                    }
                    else
                    {
                        DbObj.status = 1;
                        _DB.SaveChanges();
                    }
                    return RedirectToAction("Complaints", "Admin");
                }
            }
            return NotFound();
        }
    }
}
