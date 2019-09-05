using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultipleUserTypesApp.Data;
using MultipleUserTypesApp.Models;

namespace MultipleUserTypesApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
                var applicationDbContext = _context.Users.Include(u => u.UserType);
                return View(await applicationDbContext.ToListAsync());

        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userTypeList = _context.UserType.ToList();
            var userTypeSelectList = userTypeList.Select(type => new SelectListItem
            {
                Text = type.Title,
                Value = type.UserTypeId.ToString()
            }).ToList();

            ViewData["UserType"] = userTypeSelectList;

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserTypeId")] ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            //get user from database since the only thing you are updating is the UserTypeId
            var userToEdit = await _context.Users.FindAsync(id);
            userToEdit.UserTypeId = user.UserTypeId;
            try
            {
                _context.Update(userToEdit);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}