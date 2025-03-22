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

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]

        public IActionResult Index()
        {
            var employee = _employeeRepository.GetAll();
            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
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
                    var employee = new Employee()
                    {
                        Name = model.Name,
                        Age = model.Age,
                        Email = model.Email,
                        Adress = model.Adress,
                        Phone = model.Phone,
                        Salary = model.Salary,
                        IsActive = model.IsActive,
                        IsDeleted = model.IsDeleted,
                        HiringDate = model.HiringDate,
                        CreateAt = model.CreateAt,
                    };
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
            if (id is null) return BadRequest("Invalid Id ");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { statscode = 400, message = $"employee With ID :{id} is not found" });
            var employeeDto = new CreateEmployeeDtos()
            {
                Name = employee.Name,
                Adress = employee.Adress,
                Age = employee.Age,
                CreateAt = employee.CreateAt,
                Email = employee.Email,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                Salary = employee.Salary,
                Phone = employee.Phone
            };

            return View(employeeDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDtos model)
        {

            if (ModelState.IsValid) // Server Side Validation
            {
                try { 
                var employee = new Employee()
                {
                    Id=id,
                    Name = model.Name,
                    Adress = model.Adress,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Salary = model.Salary,
                    Phone = model.Phone
                };                
                    var count = _employeeRepository.Update(employee);
                    if (count > 0)
                    {
                        TempData["Message"] = "Employee is Edited Successfully";
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
