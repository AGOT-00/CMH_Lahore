
using System.ComponentModel.DataAnnotations;

namespace CMH_Lahore.Models
{
    public class passwordchanger
    {

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public passwordchanger()
        {

        }
    }

    public class commentuser {
        
    }


    public class complaintdetaildt
    {
        public Complaint Comp;
        public IEnumerable<Department> DepartmentList;
        public string Access;
        public explaination ifexplained;
        public comment ifcommented;

        public complaintdetaildt()
        {

        }

        public complaintdetaildt(Complaint _Comp, IEnumerable<Department> _DepartmentList, string _Access, explaination _Ex = null,comment _Comnt=null)
        {
            this.Comp = _Comp;
            this.DepartmentList = _DepartmentList;
            this.Access = _Access;
            this.ifexplained = _Ex;
            this.ifcommented = _Comnt;
        }
    }
    public class AdminDT
    {
        public string ID { get; set; }
        
        public string Password { get; set; }
        
    }
}
