using Company.FirstProject.BLL.Interfaces;
using Company.FirstProject.DAL.Data.Context;
using Company.FirstProject.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.FirstProject.BLL.Repositoris
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetByNameOrPhoneAsync(string searchTerm)
        {
            //return _context.Employees.Include(E => E.Department).Where(E => E.Name.Contains(name.ToLower())).ToList();
            searchTerm = searchTerm.Trim().ToLower(); // Remove spaces & make it case insensitive
            return await _context.Employees
                .Include(E => E.Department)
                .Where(E => E.Name.ToLower().Contains(searchTerm) ||
                            E.Phone.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
