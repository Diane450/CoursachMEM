using coursach.Models;
using coursach.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using System.Xml.Linq;

namespace coursach.Controllers
{
    public class GetEmployeeTableController : Controller
    {
        private Ispr2438MageramovEmCoursachContext _dbContext;
        public GetEmployeeTableController(Ispr2438MageramovEmCoursachContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        //вывод таблицы сотрудников
        [HttpGet]
        public IActionResult GetTable()
        {
            var employees = new List<EmployeeDataViewModel>();
            employees = (from ei in _dbContext.EmployeeInformations
                         join e in _dbContext.Employees on ei.EmployeeId equals e.Id
                         join role in _dbContext.Roles on e.RoleId equals role.Id
                         select new EmployeeDataViewModel
                         {
                             Id = ei.Id,
                             FullName = ei.FullName,
                             Phone = ei.Phone,
                             Email = ei.Email,
                             Login = e.Login,
                             Password = e.Password,
                             Role = role.Name
                         }).ToList();
            return View(employees);
        }
        
        //возвращает вьюшку изменения данных о сотруднике
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var einformation = _dbContext.EmployeeInformations.SingleOrDefault(einformation => einformation.Id == id);
            var e = _dbContext.Employees.SingleOrDefault(e => e.Id == einformation.EmployeeId);
            var role = _dbContext.Roles.SingleOrDefault(r => r.Id == e.RoleId);
            if (einformation == null || e == null || role == null)
            {
                TempData["ErrorMessage"] = $"Сотрудника с порядковым номер {id} не существует";
                return RedirectToAction("GetTable");
            }
            else
            {
                var employee = new EmployeeDataViewModel()
                {
                    Id = id,
                    FullName = einformation.FullName,
                    Phone = einformation.Phone,
                    Email = einformation.Email,
                    Login = e.Login,
                    Password = e.Password,
                    RoleId = role.Id
                };
                ViewBag.RoleName = new SelectList(_dbContext.Roles.ToList(), "Id", "Name", employee.Role);
                return View(employee);
            }
        }
        
        //изменение данных о сотруднике
        [HttpPost]
        public IActionResult Edit(EmployeeDataViewModel model)
        {
            var employeeInfo = _dbContext.EmployeeInformations.Where(e => e.Id == model.Id).First();
            employeeInfo.FullName = model.FullName;
            employeeInfo.Phone = model.Phone;
            employeeInfo.Email = model.Email;
            _dbContext.EmployeeInformations.Update(employeeInfo);
            _dbContext.SaveChanges();

            var employee = _dbContext.Employees.Where(e => e.Id == employeeInfo.EmployeeId).First();
            employee.Login = model.Login;
            employee.RoleId = model.RoleId;

            _dbContext.Employees.Update(employee);
            _dbContext.SaveChanges();

            TempData["SuccessUpdate"] = $"Данные успешно обновлены";
            return RedirectToAction("GetTable");
        }

        //вьюшка удаления сотрудника
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var einformation = _dbContext.EmployeeInformations.Where(ei => ei.Id == id).FirstOrDefault();
            if (einformation == null)
            {
                return RedirectToAction("GetTable");
            }
            var e = _dbContext.Employees.Where(e => e.Id == einformation.EmployeeId).FirstOrDefault();
            var role = _dbContext.Roles.Where(r => r.Id == e.RoleId).FirstOrDefault();
                
            var employee = new EmployeeDataViewModel()
            {
                Id = id,
                FullName = einformation.FullName,
                Phone = einformation.Phone,
                Email = einformation.Email,
                Login = e.Login,
                Password = e.Password,
                Role = role.Name
            };
            return View(employee);
        }
        
        //удаление сотрудника
        [HttpPost]
        public IActionResult Delete(EmployeeDataViewModel model)
        {
            var requsts = _dbContext.Requests.Where(r => r.EmployeeInfId == model.Id).ToList();
            for (int i = 0; i < requsts.Count(); i++)
            {
                requsts[i].EmployeeInfId = null;
                _dbContext.Requests.Update(requsts[i]);
                _dbContext.SaveChanges();
            }
            var employeeInfo = _dbContext.EmployeeInformations.Where(e => e.Id == model.Id).First();
            var employee = _dbContext.Employees.Where(e => e.Id == employeeInfo.EmployeeId).First();
            _dbContext.EmployeeInformations.Remove(employeeInfo);
            _dbContext.SaveChanges();

            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();

            TempData["SuccessUpdate"] = $"Пользователь удален";
            return RedirectToAction("GetTable");
        }
    }
}
