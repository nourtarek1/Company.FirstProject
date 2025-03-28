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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _context;

        public GenericRepository(CompanyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }
            return _context.Set<T>().ToList();
        }
        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.Department).FirstOrDefaultAsync(E =>E.Id==id) as T;
            }
            return _context.Set<T>().Find(id);
        }

        public async Task AddAsync(T model)
        {
           await  _context.AddAsync(model);
        }
        public void Update(T model)
        {
            _context.Update(model);
        }
        public void Delete(T model)
        {
            _context.Remove(model);
        }
       
    }
}
