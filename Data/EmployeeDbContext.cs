using Employee.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee.API.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {


        }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Designation> Designations { get; set; }

        public DbSet<EmployeeL> Employees { get; set; }

    }


}
