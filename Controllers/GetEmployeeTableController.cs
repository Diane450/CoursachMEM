using coursach.Helpers;
using coursach.Models;
using coursach.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult GetTable(string sortOrder, string searchString, string roleFilter)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["RoleSortParm"] = sortOrder == "Role" ? "role_desc" : "Role";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentRoleFilter"] = roleFilter;

            var roles = _dbContext.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();
            ViewBag.Roles = new SelectList(roles, "Value", "Text");

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
            
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.FullName.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(roleFilter) && roleFilter != "Все")
            {
                employees = employees.Where(s => s.Role == roleFilter).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "Role":
                    employees = employees.OrderBy(s => s.Role).ToList();
                    break;
                case "role_desc":
                    employees = employees.OrderByDescending(s => s.Role).ToList();
                    break;
                default:
                    employees = employees.OrderBy(s => s.FullName).ToList();
                    break;
            }
            return View(employees.ToList());
        }
        
        //возвращает вьюшку изменения данных о сотруднике
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var einformation = _dbContext.EmployeeInformations.SingleOrDefault(einformation => einformation.Id == id);
            if (einformation == null)
            {
                TempData["ErrorMessage"] = $"Сотрудника с порядковым номер {id} не существует";
                return RedirectToAction("MainPage", "AdminMainPage");
            }
            else
            {
                var e = _dbContext.Employees.SingleOrDefault(e => e.Id == einformation.EmployeeId);
                var role = _dbContext.Roles.SingleOrDefault(r => r.Id == e.RoleId);
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
                ViewBag.RoleName = "";
                if (employee.RoleId == 1)
                {
                    ViewBag.RoleName = new SelectList(_dbContext.Roles.Where(r => r.Id == 1).ToList(), "Id", "Name", employee.Role);
                }
                else
                {
                    ViewBag.RoleName = new SelectList(_dbContext.Roles.ToList(), "Id", "Name", employee.Role);
                }
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
                Sallt = e.Sallt,
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

            bool IsCurrentUser = CurrentUser.currentUserData.Id == employee.Id;
            _dbContext.EmployeeInformations.Remove(employeeInfo);
            _dbContext.SaveChanges();

            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();

            if (IsCurrentUser)
            {
                CurrentUser.currentUserData = null;
                return RedirectToAction("Auth", "Authorization");
            }
            TempData["SuccessUpdate"] = $"Пользователь удален";
            return RedirectToAction("GetTable");
        }
    }
}
