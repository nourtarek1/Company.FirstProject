using Company.FirstProject.DAL.Models;
using Company.FirstProject.PL.DTOS;
using Company.FirstProject.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Company.FirstProject.PL.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UsersDtos> users;

            if (string.IsNullOrEmpty(SearchInput))
            {
              users =  _userManager.Users.Select(U => new UsersDtos()
                {
                    Id=U.Id,
                    UserName=U.UserName,
                    FirstName=U.FirstName,
                    LastName =U.LastName,
                    Email=U.Email,
                    Roles=_userManager.GetRolesAsync(U).Result
                });
            }
            else
            {
                users = _userManager.Users.Select(U => new UsersDtos()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(users); // Always return the full Index view
        }



        [HttpGet]

        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id ");
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound(new { statscode = 400, message = $"user With ID :{id} is not found" });
            var dto=new UsersDtos(){
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View(ViewName, dto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UsersDtos model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id != model.Id) return BadRequest("Invalid operation");
                    var user = await _userManager.FindByIdAsync(id);
                    if(user is null) return BadRequest("Invalid operation");
                    user.Id = model.Id;
                    user.UserName = model.UserName;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
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
        public async Task<IActionResult> Delete(string id)
        {
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Remove user roles before deletion
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, roles);
                }

                // Delete the user
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    TempData["Message"] = "User deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete user.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting user: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
