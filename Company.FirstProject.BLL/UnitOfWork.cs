using Company.FirstProject.BLL.Interfaces;
using Company.FirstProject.BLL.Repositoris;
using Company.FirstProject.DAL.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.FirstProject.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
         private readonly CompanyDbContext _context;

    // Lazy-loaded repositories
    private readonly Lazy<DepartmentRepository> _departmentRepository;
    private readonly Lazy<EmployeeRepository> _employeeRepository;

    public UnitOfWork(CompanyDbContext context)
    {
        _context = context;
        _departmentRepository = new Lazy<DepartmentRepository>(() => new DepartmentRepository(_context));
        _employeeRepository = new Lazy<EmployeeRepository>(() => new EmployeeRepository(_context));
    }

        // Public properties to access repositories lazily
        public IDepartmentRepository DepartmentRepository => _departmentRepository.Value;
        public IEmployeeRepository EmployeeRepository => _employeeRepository.Value;

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

       
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }

}
