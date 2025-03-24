using AutoMapper;
using Company.FirstProject.BLL.Interfaces;
using Company.FirstProject.BLL.Repositoris;
using Company.FirstProject.DAL.Models;
using Company.FirstProject.PL.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace Company.FirstProject.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
                                  IEmployeeRepository employeeRepository ,
                                  IDepartmentRepository departmentRepository,
                                  IMapper mapper
                                  )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _employeeRepository.GetAll();
            }
            else
            {
                employees = _employeeRepository.GetByNameOrPhone(SearchInput);
            }

            return View(employees); // Always return the full Index view
        }
        //public IActionResult Index(string? SearchInput)
        //{
        //    IEnumerable<Employee> employee;
        //    if (string.IsNullOrEmpty(SearchInput))
        //    {
        //        employee = _employeeRepository.GetAll();

        //    }
        //    else
        //    {
        //        employee = _employeeRepository.GetByNameOrPhone(SearchInput);

        //    }
        //    return View(employee);
        //}



        [HttpGet]
        public IActionResult Create()
        {
            var department = _departmentRepository.GetAll();
            ViewData["departments"] = department;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(CreateEmployeeDtos model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var employee = new Employee()
                    //{
                    //    Name = model.Name,
                    //    Age = model.Age,
                    //    Email = model.Email,
                    //    Adress = model.Adress,
                    //    Phone = model.Phone,
                    //    Salary = model.Salary,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    HiringDate = model.HiringDate,
                    //    CreateAt = model.CreateAt,
                    //    DepartmentId = model.DepartmentId
                    //};
                    var employee = _mapper.Map<Employee>(model);
                    var Count = _employeeRepository.Add(employee);
                    if (Count > 0)
                    {
                        TempData["Message"] = "Employy is Created";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                }
            }
            return View(model);
        }

        [HttpGet]

        public IActionResult Details(int? id ,string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id ");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { statscode = 400, message = $"Departmen With ID :{id} is not found" });

            return View(ViewName,employee);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var department = _departmentRepository.GetAll();
            ViewData["departments"] = department;
            if (id is null) return BadRequest("Invalid Id ");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { statscode = 400, message = $"employee With ID :{id} is not found" });     
            var dto = _mapper.Map<CreateEmployeeDtos>(employee);

            return View(dto);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, Employee model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var employee = new Employee()
        //            {
        //                Id=id,
        //                Name = model.Name,
        //                Age = model.Age,
        //                Email = model.Email,
        //                Adress = model.Adress,
        //                Phone = model.Phone,
        //                Salary = model.Salary,
        //                IsActive = model.IsActive,
        //                IsDeleted = model.IsDeleted,
        //                HiringDate = model.HiringDate,
        //                CreateAt = model.CreateAt,
        //                DepartmentId = model.DepartmentId
        //            };
        //            var Count = _employeeRepository.Update(employee);
        //            if (Count > 0)
        //            {
        //                TempData["Message"] = "Employy is Created";
        //                return RedirectToAction(nameof(Index));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", ex.Message);

        //        }
        //    }
        //    return View(model);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDtos model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = _mapper.Map<Employee>(model); // AutoMapper Conversion
                    employee.Id = id; // Ensure the ID remains correct

                    var count = _employeeRepository.Update(employee);
                    if (count > 0)
                    {
                        TempData["Message"] = "Employee is Updated";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var employee = _employeeRepository.Get(id);
            if (employee != null)
            {
                var count = _employeeRepository.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index)); // Redirect instead of returning a view
                }
            }

            return RedirectToAction(nameof(Index)); // Redirect even if the delete fails
        }
    }
}
