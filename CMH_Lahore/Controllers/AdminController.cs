using CMH_Lahore.DB;
using CMH_Lahore.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMH_Lahore.Controllers
{
    public class AdminController : Controller
    {
        private readonly Database _DB;
        static Admin? SignedIn;

        public AdminController(Database DB)
        {
            if (SignedIn != null)
            {
                Console.WriteLine('H');
            }
            _DB = DB;
        }

        [HttpGet]
        public IActionResult Login()
        {
            AdminDT obj = new();

            //TEMP
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminDT admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var objs1 = _DB.Admins.ToList();
                    var objs = _DB.Admins.FirstOrDefault(u => u.ID == admin.ID && u.Password == admin.Password);

                    if (objs != null)
                    {
                        SignedIn = AdminFactory.GetAdmin(objs);
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
                Admin AdminAccount = _DB.Admins.Find(SignedIn.ID);
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
        public IActionResult Complaints(int id,IEnumerable<Complaint> Listing ,string filter = "")
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

        [HttpPost]
        public IActionResult ComplaintDetail(int id, int DepartmentSelection, string Explain, string copmlainttype, int CurrentComplaintStatus)
        {
            if (SignedIn != null)
            {
                var DbObj = _DB.Complaints.Find(id);

                if (DbObj != null)
                {
                    DbObj.ComplaintType = copmlainttype;
                    DbObj.status = CurrentComplaintStatus;
                    DbObj.Department = DepartmentSelection;

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

        public IActionResult savecommandentcomment(int id, string CommandentComment)
        {
            if (SignedIn != null)
            {
                var DbObj = _DB.Complaints.Find(id);

                if (DbObj != null)
                {
                    comment Obj = new(id, SignedIn.Name, CommandentComment);
                    _DB.comments.Add(Obj);

                    _DB.SaveChanges();
                    return RedirectToAction("Complaints", "Admin");
                }
            }
            return Redirect(Request.GetTypedHeaders().Referer.ToString());
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
