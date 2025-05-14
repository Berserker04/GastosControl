using GastosControl.Domain.Interfaces;
using GastosControl.Application.Services;
using GastosControl.Models;
using Microsoft.AspNetCore.Mvc;

namespace GastosControl.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.AuthenticateAsync(model.Login, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Credenciales inválidas");
                return View(model);
            }

            // Guardar usuario en sesión
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FirstName);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
