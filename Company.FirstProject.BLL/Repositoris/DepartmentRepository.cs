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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly CompanyDbContext _contex;

        public DepartmentRepository()
        {
            _contex = new CompanyDbContext();
        }
        public IEnumerable<Department> GetAll()
        {
            return _contex.Departments.ToList();
        }
        public Department? Get(int id)
        {
            return _contex.Departments.Find(id);
        }

        public int Add(Department model)
        {
            _contex.Departments.Add(model);
            return _contex.SaveChanges();
        }
        public int Update(Department model)
        {
            _contex.Departments.Update(model);
            return _contex.SaveChanges();
        }
        public int Delete(Department model)
        {
            _contex.Departments.Remove(model);
            return _contex.SaveChanges();
        }
    }
}
