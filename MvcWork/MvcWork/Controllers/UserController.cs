using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MvcWork.Models;

namespace MvcWork.Controllers
{
    public class UserController : Controller
    {
        private readonly FileUserService _userService;

        public UserController(FileUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index(int userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        public IActionResult UserList()
        {
            
            var allUsers = _userService.GetAllUsers();

            
            var nonAdminUsers = allUsers.Where(user => !user.IsAdmin).ToList();

            return View(nonAdminUsers);
        }
       
        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {
           
            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

           
            if (!user.IsAdmin)
            {
                try
                {
                   
                    user.IsActive = false;

                    
                    _userService.UpdateUser(user);

                    return RedirectToAction("UserList");
                }
                catch (Exception ex)
                {
                   
                    return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
                }
            }
            else
            {
                return Forbid("Yalnızca yönetici olmayan kullanıcılar silinebilir.");
            }
        }


        public IActionResult UserEdit()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserEdit(string userId)
        {
            var user = _userService.GetUserById(Convert.ToInt32(userId));
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult UserEdit(UserModel updatedUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userService.UpdateUser(updatedUser);
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                    return RedirectToAction("Index", new RouteValueDictionary(
                    new { controller = "User", action = "Index", userId = updatedUser.UserId }));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Güncelleme sırasında bir hata oluştu: {ex.Message}");
                }
            }

            return View(updatedUser);
        }

    }
}
