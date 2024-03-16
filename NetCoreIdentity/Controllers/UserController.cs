﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreIdentity.Models.Admins.AppRoles.PageVms;
using NetCoreIdentity.Models.Admins.AppRoles.ResponseModels;
using NetCoreIdentity.Models.Admins.AppUsers.RequestModels;
using NetCoreIdentity.Models.Entities;

namespace NetCoreIdentity.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        readonly UserManager<AppUser> _userManager;

        readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            List<AppUser> nonAdminUsers = _userManager.Users.Where(x => !x.UserRoles.Any(x => x.Role.Name == "Admin")).ToList();

            return View(nonAdminUsers);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequestModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(appUser,$"{model.UserName}123"); // Kullanıcı Şifresini Kullanıcı Adı ve Yanına 123 olarak tanımladık
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "Member");
                    return RedirectToAction("Index");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }

            }
            return View(model);
        }

        public async Task<IActionResult> AssignRole(int id)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);

            IList<string> userRoles = await _userManager.GetRolesAsync(appUser); // Elimize Geçen Kullanıcının Rollerini Verir.

            List<AppRole> allRoles = _roleManager.Roles.ToList(); // Bütün Roller

            List<AppRoleResponseModel> roles = new();

            foreach (AppRole role in allRoles) 
            {
                roles.Add(new()
                {
                    RoleID = id,
                    RoleName = role.Name,
                    Checked = userRoles.Contains(role.Name)
                }); 
            }

            AssignRolePgeVM arPvm = new()
            {
                UserID = id,
                Roles = roles
            };

            return View(arPvm);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRolePgeVM model)
        {
            AppUser appUser = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == model.UserID);

            IList<string> userRoles = await _userManager.GetRolesAsync(appUser);

            foreach (AppRoleResponseModel role in model.Roles)
            {
                if (role.Checked && !userRoles.Contains(role.RoleName)) await _userManager.AddToRoleAsync(appUser, role.RoleName);
                else if (!role.Checked && userRoles.Contains(role.RoleName)) await _userManager.RemoveFromRoleAsync(appUser, role.RoleName);
            }

            return RedirectToAction("Index");
        }
    }
}
