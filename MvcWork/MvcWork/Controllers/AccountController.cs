using Microsoft.AspNetCore.Mvc;
using MvcWork.Models;

namespace MvcWork.Controllers
{
    public class AccountController : Controller
    {
        private readonly FileUserService _userService;

        public AccountController(FileUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var users = _userService.GetAllUsers();
                var user = users.Find(u => u.MailAddress == model.MailAddress && u.Password == model.Password && u.IsActive);
                if (user != null)
                {
                    // Başarılı giriş yapan kullanıcının e-posta adresi üzerinden UserId alınır
                    var userId = _userService.GetUserIdByMailAddress(user.MailAddress);

                    // Yönlendirme işlemi
                    if (user.IsAdmin)
                    {
                        return RedirectToAction("UserList", "User");
                    }
                    else
                    {
                        return RedirectToAction("Index", new RouteValueDictionary(
    new { controller = "User", action = "Index", userId = userId }));
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                }
            }

            return View(model);
        }


        public IActionResult SingUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SingUp(SingUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = new UserModel { FirstName= model.FirstName, LastName= model.LastName, MailAddress = model.MailAddress, Password = model.Password };
                _userService.AddUser(user);
                
                return RedirectToAction("Index", "Home"); 
            }

            return View(model);
        }
    }
}
