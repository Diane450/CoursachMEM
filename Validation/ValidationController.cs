using coursach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace coursach.Validation
{
    public class ValidationController : Controller
    {
        private Ispr2438MageramovEmCoursachContext _dbContext;
        public ValidationController(Ispr2438MageramovEmCoursachContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public IActionResult CheckFullName(string fullName)
        {
            string[] str = fullName.Split(' ');
            if (str.Length == 2 || str.Length ==3)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!Regex.IsMatch(str[i], "^[а-яА-Я]+$"))
                    {
                        return Json("Некорректное имя сотрудника");
                    }
                }
                return Json(true);
            }
            return Json("Некорректное имя сотрудника");
        }
        public IActionResult CheckLogin(string login)
        {
            string logins = (from user in _dbContext.Employees where user.Login == login select user.Login).FirstOrDefault();
            if (logins != null)
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}
