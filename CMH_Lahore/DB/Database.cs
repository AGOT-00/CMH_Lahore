using CMH_Lahore.Models;
using Microsoft.EntityFrameworkCore;

namespace CMH_Lahore.DB
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options)
        {

        }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<explaination> Explainations { get; set; }

        public DbSet<comment> comments { get; set; }
        
    }
}
