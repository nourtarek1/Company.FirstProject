using Company.FirstProject.BLL.Repositoris;
using Company.FirstProject.DAL.Models;
using Company.FirstProject.PL.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace Company.FirstProject.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;

        }
        [HttpGet] // Get : /Department/Index
        public IActionResult Index()
        {
            var departmets = _departmentRepository.GetAll();
            return View(departmets);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                var count = _departmentRepository.Add(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]

        // Refactore Code 
        public IActionResult Details(int? id ,string ViewName="Details")
        {
            if (id is null) return BadRequest("Invalid Id "); //400
            var department = _departmentRepository.Get(id.Value);
            if (department is null) return NotFound(new { statscode = 400, message = $"Departmen With ID :{id} is not found" });
            return View( ViewName,department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id "); //400
            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { statscode = 400, message = $"Departmen With ID :{id} is not found" });
            return Details(id , "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {

            if (ModelState.IsValid) // Server Side Validation
            {
                if (id == department.Id)
                {
                    var count = _departmentRepository.Update(department);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var department = _departmentRepository.Get(id);
            if (department != null)
            {
                var count = _departmentRepository.Delete(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index)); // Redirect instead of returning a view
                }
            }

            return RedirectToAction(nameof(Index)); // Redirect even if the delete fails
        }
    }
}
