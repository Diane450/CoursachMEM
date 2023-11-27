using coursach.Models;
using coursach.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Xml.Linq;

namespace coursach.Controllers
{
    public class NewEmployeeController : Controller
    {
        private Ispr2438MageramovEmCoursachContext _dbContext;
        public NewEmployeeController(Ispr2438MageramovEmCoursachContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        //возвращает вьюшку добавления нового сотрудника
        [HttpGet]
        public IActionResult AddNewEmployee()
        {
            ViewBag.RoleName = new SelectList(_dbContext.Roles.ToList(), "Id", "Name");
            return View();
        }
        //добавление нового пользователя
        [HttpPost]
        public IActionResult AddNewEmployee(EmployeeDataViewModel model)
        {
            Employee newEmployee = new Employee()
            {
                Login = model.Login,
                Password = model.Password,
                RoleId = model.RoleId
            };
            _dbContext.Employees.Add(newEmployee);
            _dbContext.SaveChanges();

            EmployeeInformation employee = new EmployeeInformation()
            {
                FullName = model.FullName,
                Phone = model.Phone,
                Email = model.Email,
                EmployeeId = newEmployee.Id
            };
            _dbContext.EmployeeInformations.Add(employee);
            _dbContext.SaveChanges();

            TempData["Success"] = "Новый сотрудник добавлен";
            return RedirectToAction("AddNewEmployee", "NewEmployee");
        }
    }
}
