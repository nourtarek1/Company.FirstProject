using Company.FirstProject.BLL.Interfaces;
using Company.FirstProject.DAL.Data.Context;
using Company.FirstProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.FirstProject.BLL.Repositoris
{
    public class EmployeeRepository : GenericRepository<Employee> ,IEmployeeRepository
    {
        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
        }
    }
}
