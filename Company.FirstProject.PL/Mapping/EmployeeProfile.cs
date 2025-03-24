using AutoMapper;
using Company.FirstProject.DAL.Models;
using Company.FirstProject.PL.DTOS;

namespace Company.FirstProject.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDtos, Employee>();
            CreateMap<Employee,CreateEmployeeDtos>();
        }
    }
}
