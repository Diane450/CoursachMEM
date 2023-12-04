using coursach.Helpers;
using coursach.Models;
using coursach.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace coursach.Controllers
{
    public class AuthorizationController : Controller
    {
        private Ispr2438MageramovEmCoursachContext _dbContext;
        public AuthorizationController(Ispr2438MageramovEmCoursachContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        [HttpGet]
        //возвращает вьюшку авторизации
        public IActionResult Auth()
        {
            return View();
        }
        [HttpPost]
        //авторизация
        public IActionResult Auth(AuthorizationViewModel model)
        {
            var user = _dbContext.Employees.Where(user => user.Login == model.Login).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == BCrypt.Net.BCrypt.HashPassword(model.Password, user.Sallt!))
                {
                    CurrentUser.currentUserData = user;
                    if(user.RoleId == 1)
                    {
                         return RedirectToAction("MainPage", "AdminMainPage");
                    }
                    else
                    {
                        return RedirectToAction("GetRequestTable", "RequestTable");
                    }
                }
                TempData["Error"] = "Неудачная попытка авторизации";
                return RedirectToAction("Auth", "Authorization");
            }
            TempData["Error"] = "Неудачная попытка авторизации";
            return RedirectToAction("Auth", "Authorization");
        }
    }
}