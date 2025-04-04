using Company.FirstProject.DAL.Models;
using Company.FirstProject.PL.DTOS;
using Company.FirstProject.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.FirstProject.PL.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RolesDtos> roles;

            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(r => new RolesDtos()
                {
                    Id = r.Id,
                   Name=r.Name
                });
            }
            else
            {
                roles = _roleManager.Roles.Select(r => new RolesDtos()
                {
                    Id = r.Id,
                    Name = r.Name
                }).Where(r => r.Name.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(roles); // Always return the full Index view
        }

        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(RolesDtos model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                     var role = await _roleManager.FindByNameAsync(model.Name);
                    if(role is null)
                    {
                        role = new IdentityRole()
                        {
                            Name = model.Name
                        };
                     var result =await _roleManager.CreateAsync(role);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
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

        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id ");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound(new { statscode = 400, message = $"Role With ID :{id} is not found" });
            var dto = new RolesDtos()
            {
                Id = role.Id,
                Name=role.Name
            };
            return View(ViewName, dto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RolesDtos model)
        {
      

            try
            {
                if (id != model.Id)
                {
                    ModelState.AddModelError("", "Role ID mismatch.");
                    return View(model);
                }

                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound("Role not found.");
                }

                // Check if role name already exists (but allow if it's the same role)
                var existingRole = await _roleManager.FindByNameAsync(model.Name);
                if (existingRole != null && existingRole.Id != id)
                {
                    ModelState.AddModelError("", "Role name already exists.");
                    return View(model);
                }

                // Update role name
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["Message"] = "Role updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update role.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            // Find the role by ID
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                TempData["Error"] = "Role not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Delete the role
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Role deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete role.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting role: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
