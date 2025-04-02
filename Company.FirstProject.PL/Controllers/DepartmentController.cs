using AutoMapper;
using Company.FirstProject.BLL.Interfaces;
using Company.FirstProject.BLL.Repositoris;
using Company.FirstProject.DAL.Models;
using Company.FirstProject.PL.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.FirstProject.PL.Controllers
{
    [Authorize]

    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentController(
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper Mapper

            )
        {
            _unitOfWork = unitOfWork;
            //_departmentRepository = departmentRepository;
            _mapper = Mapper;
        }
        [HttpGet] // Get : /Department/Index
        public async Task<IActionResult> Index()
        {
            var departmets = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departmets);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var department = _mapper.Map<Department>(model);
                     await _unitOfWork.DepartmentRepository.AddAsync(department);
                    var count =await _unitOfWork.CompleteAsync();
                    if (count > 0)
                    {
                        TempData["Message"] = "Department is Created";
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

        [HttpGet]

        // Refactore Code 
        public async Task<IActionResult> Details(int? id ,string ViewName="Details")
        {
            if (id is null) return BadRequest("Invalid Id "); //400
            var department =await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department is null) return NotFound(new { statscode = 400, message = $"Departmen With ID :{id} is not found" });
            return View( ViewName,department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id ");
            var department =await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department is null) return NotFound(new { statscode = 400, message = $"employee With ID :{id} is not found" });
            var dto = _mapper.Map<CreateDepartmentDto>(department);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var department = _mapper.Map<Department>(model); // AutoMapper Conversion
                    department.Id = id; // Ensure the ID remains correct

                    _unitOfWork.DepartmentRepository.Update(department);
                    var count =await _unitOfWork.CompleteAsync();
                    if (count > 0)
                    {
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
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id);
            if (department != null)
            {
                 _unitOfWork.DepartmentRepository.Delete(department);
                var count =await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index)); // Redirect instead of returning a view
                }
            }

            return RedirectToAction(nameof(Index)); // Redirect even if the delete fails
        }
    }
}
