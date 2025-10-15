using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyApp1.Data;
using MyApp1.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyApp1.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyAppContext _context;

        public AccountController(MyAppContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View(_context.UserAccounts.ToList());
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            bool isFirstUser = !_context.UserAccounts.Any();
            if (!ModelState.IsValid)
                return View(model);

            var account = new UserAccount
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Role = isFirstUser ? "Admin" : "User"
            };

            try
            {
                _context.UserAccounts.Add(account);
                await _context.SaveChangesAsync();

                ModelState.Clear();
                ViewBag.Message = $"{account.FirstName} {account.LastName} registered successfully. Please login.";
                return View();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Already registered — please login or use a different email");
                return View(model);
            }
        }

       
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.UserAccounts
                .Where(x => (x.UserName == model.UserNameOrEmail || x.Email == model.UserNameOrEmail)
                            && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Username/email or password is not correct");
                return View(model);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, string.IsNullOrWhiteSpace(user.UserName) ? user.Email : user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

          
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

         
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> LogOutGet()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = User.Identity?.Name;
            return View();
        }
        [Authorize]
        public IActionResult Profile()
        {
            var email = User.Identity.Name;
            if (email == null)
                return Unauthorized();

            var user = _context.UserAccounts.FirstOrDefault(u => u.UserName == email);
            if (user == null)
                return NotFound();

            return View(user);
        }
        [Authorize]
        [HttpGet]
        public IActionResult EditProfile()
        {
            var userNameOrEmail = User.Identity?.Name;
            if (userNameOrEmail == null)
                return Unauthorized();

            var user = _context.UserAccounts
                .FirstOrDefault(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail);

            if (user == null)
                return NotFound();

          
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Admin", Text = "Admin" },
                new SelectListItem { Value = "User", Text = "User" }
            };

               return View(user);
            }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(UserAccount model)
        {
            if (!ModelState.IsValid)
            {
                
                ViewBag.Roles = new List<SelectListItem>
        {
            new SelectListItem { Value = "Admin", Text = "Admin" },
            new SelectListItem { Value = "User", Text = "User" }
        };
                return View(model);
            }

            var userNameOrEmail = User.Identity?.Name;
            if (userNameOrEmail == null)
                return Unauthorized();

            
            var user = _context.UserAccounts
                .FirstOrDefault(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail);

            if (user == null)
                return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Role = model.Role;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.Password = model.Password; 
            }

            _context.SaveChanges();
            TempData["Message"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

    }

}
