using GpReg.Data;
using GpReg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GpReg.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        //GET: User/Register
        public IActionResult Register()
        {
            return View();
        }
        //POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid) {
                //simple check for existing user
                var exists = await _context.Users.AnyAsync(u => u.Email == user.Email);
                if (exists) {
                    ModelState.AddModelError("Email", "Email already Registered");
                    return View(user);
                }
                //Save User
                await _context.Users.AddAsync(user); // TODO: Hash password
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(user);
        }
        //GET:User/Login
        public IActionResult Login()
        {
            return View();
        }

        //POST:User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);
            if (user != null) { 
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserName", user.FullName);

                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid emial or password";
            return View();
        }
    }
}
