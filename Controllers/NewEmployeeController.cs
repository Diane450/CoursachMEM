using coursach.Models;
using coursach.ViewModels;
using Google.Protobuf.WellKnownTypes;
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
                Sallt = BCrypt.Net.BCrypt.GenerateSalt(),
                RoleId = model.RoleId
            };
            newEmployee.Password = BCrypt.Net.BCrypt.HashPassword(model.Password, newEmployee.Sallt);
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
