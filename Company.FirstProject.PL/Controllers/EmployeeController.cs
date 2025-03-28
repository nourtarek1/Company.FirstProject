using AutoMapper;
using Company.FirstProject.BLL.Interfaces;
using Company.FirstProject.BLL.Repositoris;
using Company.FirstProject.DAL.Models;
using Company.FirstProject.PL.DTOS;
using Company.FirstProject.PL.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Company.FirstProject.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
                                  //IEmployeeRepository employeeRepository ,
                                  //IDepartmentRepository departmentRepository,
                                  IUnitOfWork unitOfWork,
                                  IMapper mapper
                                  )
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(SearchInput))
            {
                employees =await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees =await _unitOfWork.EmployeeRepository.GetByNameOrPhoneAsync(SearchInput);
            }

            return View(employees); // Always return the full Index view
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var department =await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = department;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateEmployeeDtos model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   if(model.Image is not null)
                    {
                        model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                    }
                    var employee = _mapper.Map<Employee>(model);
                    await _unitOfWork.EmployeeRepository.AddAsync(employee);
                    var Count =await _unitOfWork.CompleteAsync();
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

        public async Task<IActionResult> Details(int? id ,string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id ");
            var employee =await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { statscode = 400, message = $"Departmen With ID :{id} is not found" });

            return View(ViewName,employee);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var department =await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = department;
            if (id is null) return BadRequest("Invalid Id ");
            var employee =await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { statscode = 400, message = $"employee With ID :{id} is not found" });     
            var dto = _mapper.Map<CreateEmployeeDtos>(employee);

            return View(dto);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDtos model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ImageName is not null && model.Image is not null)
                    {
                         DocumentSettings.DeleteFile(model.ImageName, "Images");
                    }
                    if (model.Image is not null)
                    {
                        model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                    }
                    var employee = _mapper.Map<Employee>(model); // AutoMapper Conversion
                    employee.Id = id; // Ensure the ID remains correct

                     _unitOfWork.EmployeeRepository.Update(employee);
                    var count =await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Delete(int id)
        {
            var employee =await _unitOfWork.EmployeeRepository.GetAsync(id);
            if (employee != null)
            {
                // Delete image from wwwroot before deleting the employee
                if (!string.IsNullOrEmpty(employee.ImageName))
                {
                    DocumentSettings.DeleteFile(employee.ImageName, "Images");
                }

                _unitOfWork.EmployeeRepository.Delete(employee);
                var count =await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete employee.";
                }
            }
            else
            {
                TempData["Error"] = "Employee not found.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
