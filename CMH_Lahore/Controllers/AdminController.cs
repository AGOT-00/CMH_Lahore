using CMH_Lahore.DB;
using CMH_Lahore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
//Login UI RE
//Feedback Upload Status
//Add Department UI


namespace CMH_Lahore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly Database _DB;
        Admin? SignedIn;

        public AdminController(Database DB)
        {
            check();

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

        public IActionResult Index()
        {
            check();
            return View(SignedIn);
        }

        void check()
        {
            if (SignedIn == null)
            {
                try
                {
                    if (HttpContext.User != null)
                    {
                        var userClaim = HttpContext.User.FindFirst("UserLoggedIn");
                        if (userClaim != null)
                        {
                            var userJson = userClaim.Value;
                            var user = JsonConvert.DeserializeObject<Admin>(userJson);
                            SignedIn = AdminFactory.GetAdmin(user);
                            // do something with the user
                        }
                    }
                    else
                    {
                        SignedIn = null;
                    }
                }
                catch (Exception e)
                {
                    SignedIn = null;
                }
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            AdminDT obj = new();
            return View(obj);
        }

        [AllowAnonymous]
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
                        SignedIn = AdminFactory.GetAdmin(objs);
                        //HttpContext.Session.SetString("loggedInUser", JsonConvert.SerializeObject(SignedIn));

                        //Admin user = JsonConvert.DeserializeObject<Admin>(HttpContext.Session.GetString("loggedInUser"));


                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Role,objs.getadmintype()),
                            new Claim("UserLoggedIn", JsonConvert.SerializeObject(objs)),
                        };

                        //Create a Identity
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //Create a Principal
                        var principal = new ClaimsPrincipal(identity);
                        //Sign In
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

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
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Admin");
        }

        public IActionResult Department()
        {
            IEnumerable<Department> DepartmentList = _DB.Departments.ToList();
            return View(DepartmentList);
        }

        public IActionResult DeleteDepartment(int? id)
        {
            var DepartmentObject = _DB.Departments.Find(id);
            if (DepartmentObject != null)
            {
                _DB.Departments.Remove(DepartmentObject);
                _DB.SaveChanges();
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
            passwordchanger Obj = new();
            return View(Obj);
        }

        [HttpPost]
        public IActionResult Changepswd(passwordchanger Obj)
        {
            check();

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
            return View();
        }

        [HttpGet]
        public IActionResult Complaints(IEnumerable<Complaint> ComplaintList, string filter = "")
        {
            check();
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
        }

        [HttpPost]
        public IActionResult Complaints(int id, IEnumerable<Complaint> Listing, string filter = "")
        {

            check();

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
        }

        [HttpGet]
        public IActionResult ComplaintDetail(int? ID)
        {

            check();

            var DbObj = _DB.Complaints.Find(ID);
            if (DbObj != null)
            {
                IEnumerable<Department> ComplaintList = _DB.Departments.ToList();
                complaintdetaildt Obj = new(DbObj, ComplaintList, SignedIn.getadmintype(), _DB.Explainations.Find(ID), _DB.comments.Find(ID));

                return View(Obj);
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

            check();

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
            return NotFound();
        }

        public IActionResult savecommandentcomment(int id, string Comment)
        {

            check();
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
            return Redirect(Request.GetTypedHeaders().Referer.ToString());
        }

        public IActionResult sendbacktoCO(int? id)
        {
            check();

            if (SignedIn.getadmintype() == "AssistantCommandent" && id != null)
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


        public IActionResult complaintstatus(int? id)
        {
            if ( id != null)
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
