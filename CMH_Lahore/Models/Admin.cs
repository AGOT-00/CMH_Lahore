using CMH_Lahore.DB;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace CMH_Lahore.Models
{
    public partial class Admin
    {
        [Key] 
        public string ID { get; set; }

        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Phone { get; set; }
        
        public int rank { get; set; }
        
        public int department { get; set; }

        //Admin | Care
        public string AdminType { get; set; } = "NA";

        public bool verifypassword(string pass)
        {
            if (Password == pass)
            {
                return true;
            }
            return false;
        }
        
        public Admin(){}
        
        public Admin(string _id, string _name, string _password, string _phone, int _department, string _type,int _rank)
        {
            this.ID = _id;
            this.Name = _name;
            this.department = _department;
            this.Password = _password;
            this.Phone = _phone;
            this.AdminType = _type;
            this.rank = _rank;
        }

        public string getadmintype()
        {
            return rank switch
            {
                0 => "Commandent",
                1 => "DeputyCommandent",
                2 => "AssistantCommandent",
                3 => "ComplaintOfficer",
                _ => "None"
            };

        }

        public virtual Complaint? GetSingleComplaint(Database _DB, int cid)
        {
            return _DB.Complaints.Find(cid);
        }

        public virtual IEnumerable<Complaint> GetListofComplaint(Database _DB)
        {
            return _DB.Complaints.ToList();
        }

    }

    public class ComplaintOfficer : Admin
    {
        public ComplaintOfficer(Admin Obj) : base(Obj.ID, Obj.Name, Obj.Password, Obj.Phone, Obj.department,Obj.AdminType,Obj.rank=0)
        {

        }

        public override Complaint? GetSingleComplaint(Database _DB, int cid)
        {
            return _DB.Complaints.Find(cid);
        }

        public override IEnumerable<Complaint> GetListofComplaint(Database _DB)
        {
            return _DB.Complaints.ToList().Where(u => u.status == 0);
            //Here Complaint Officer can only see the complaints which are not yet assigned to any department or Forwaded
        }
    }

    public class AssistantCommandent : Admin
    {
        public string DepartmentType;

        public AssistantCommandent(Admin Obj) : base(Obj.ID, Obj.Name, Obj.Password, Obj.Phone, Obj.department,Obj.AdminType,Obj.rank)
        {
            DepartmentType = "Admin";
        }

        public override Complaint? GetSingleComplaint(Database _DB, int cid)
        {
            return _DB.Complaints.Find(cid);
        }

        public override IEnumerable<Complaint> GetListofComplaint(Database _DB)
        {
            return _DB.Complaints.ToList().Where(u => u.ComplaintType == this.AdminType);
        }
    }
    
    public class DeputyCommandent : Admin
    {
        public DeputyCommandent(Admin Obj) : base(Obj.ID, Obj.Name, Obj.Password, Obj.Phone, Obj.department,Obj.AdminType,Obj.rank) { }

        public override Complaint? GetSingleComplaint(Database _DB, int cid)
        {
            return _DB.Complaints.Find(cid);
        }

        public override IEnumerable<Complaint> GetListofComplaint(Database _DB)
        {
            return _DB.Complaints.ToList().Where(u => u.ComplaintType == this.AdminType);
        }
        //Deputy Commandent can See Complaint Related to their Nature of Complaint either Admin or Care
    }

    public class Commandent : Admin
    {
        public Commandent()
        {

        }
        public Commandent(Admin Obj) : base(Obj.ID, Obj.Name, Obj.Password, Obj.Phone, Obj.department,Obj.AdminType,Obj.rank){}
        
        public override Complaint? GetSingleComplaint(Database _DB, int cid)
        {
            return _DB.Complaints.Find(cid);
        }

        public override IEnumerable<Complaint> GetListofComplaint(Database _DB)
        {
            return _DB.Complaints.ToList();
        }
    }

    public class AdminFactory
    {
        public static Admin GetAdmin(Admin Obj)
        {
            return Obj.rank switch
            {
                4 => Obj,
                3 => new ComplaintOfficer(Obj),
                2 => new AssistantCommandent(Obj),
                1 => new DeputyCommandent(Obj),
                0 => new Commandent(Obj)
            };
        }

    }

    public class Department
    {
        [Key]
        //Set Identity in Migrations to 5,1 Starts from 5 increments 1
        public int id { get; set; }

        [Required]
        public string Departmentname { get; set; }
    }

}
