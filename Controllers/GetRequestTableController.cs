using coursach.Helpers;
using coursach.Models;
using coursach.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace coursach.Controllers
{
    public class RequestTableController : Controller
    {
        private Ispr2438MageramovEmCoursachContext _dbContext;
        public RequestTableController(Ispr2438MageramovEmCoursachContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        //вьюшка таблицы с заявками
        [HttpGet]
        public IActionResult GetRequestTable(string sortOrder, string searchString, string statusFilter, string employeeFilter)
        {
            ViewData["CreateDateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "CreateDate_desc" : "";
            ViewData["TakeDateSortParm"] = sortOrder == "TakeDate" ? "TakeDate_desc" : "TakeDate";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["CurrentEmployeeFilter"] = employeeFilter;

            var statuses = _dbContext.Statuses.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();
            ViewBag.Statuses = new SelectList(statuses, "Value", "Text");

            var employees = _dbContext.EmployeeInformations.Select(r => new SelectListItem
            {
                Value = r.FullName,
                Text = r.FullName
            }).ToList();
            ViewBag.Employees = new SelectList(employees, "Value", "Text");



            var requestTable = (from requests in _dbContext.Requests
                                join employee in _dbContext.EmployeeInformations on requests.EmployeeInfId equals employee.Id into empInfo
                                from employee in empInfo.DefaultIfEmpty()
                                join status in _dbContext.Statuses on requests.StatusId equals status.Id
                                select new RequestViewModel()
                                {
                                    Id = requests.Id,
                                    FullNameClient = requests.FullNameClient,
                                    ClientId = requests.ClientId,
                                    TechnicalTask = requests.TechnicalTask,
                                    Phone = requests.Phone,
                                    Email = requests.Email,
                                    CreationDate = requests.CreationDate,
                                    TakeDate = requests.TakeDate,
                                    Status = status.Name,
                                    StatusId = status.Id,
                                    EmployeeInfId = requests.EmployeeInfId,
                                    EmployeeInf = employee.FullName
                                }
                                ).ToList();
            if(requestTable.Count()==0)
                TempData["ErrorRequest"] = $"Заявки отсутствуют";

            if (!String.IsNullOrEmpty(searchString))
            {
                requestTable = requestTable.Where(s => s.FullNameClient.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(statusFilter) && statusFilter != "Все")
            {
                requestTable = requestTable.Where(s => s.Status == statusFilter).ToList();
            }

            if (!String.IsNullOrEmpty(employeeFilter) && employeeFilter != "Все")
            {
                requestTable = requestTable.Where(s => s.EmployeeInf == employeeFilter).ToList();
            }

            switch (sortOrder)
            {
                case "CreateDate_desc":
                    requestTable = requestTable.OrderByDescending(s => s.CreationDate).ToList();
                    break;
                case "TakeDate":
                    requestTable = requestTable.OrderBy(s => s.TakeDate).ToList();
                    break;
                case "TakeDate_desc":
                    requestTable = requestTable.OrderByDescending(s => s.TakeDate).ToList();
                    break;
                default:
                    requestTable = requestTable.OrderBy(s => s.CreationDate).ToList();
                    break;
            }
            return View(requestTable.ToList());
        }

        //вьюшка изменения данных по заявке
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var request = _dbContext.Requests.SingleOrDefault(r => r.Id == id);
            if (request == null)
            {
                return RedirectToAction("GetRequestTable");
            }
            else
            {
                var requestTable = new RequestViewModel()
                {
                    Id = id,
                    FullNameClient = request.FullNameClient,
                    ClientId = request.ClientId,
                    TechnicalTask = request.TechnicalTask,
                    Phone = request.Phone,
                    Email = request.Email,
                    CreationDate = request.CreationDate,
                    TakeDate = request.TakeDate,
                    StatusId = request.StatusId,
                    EmployeeInfId = request.EmployeeInfId
                };
                var list = (from empInf in _dbContext.EmployeeInformations
                            join employee in _dbContext.Employees on empInf.EmployeeId equals employee.Id
                            select new EmployeeDataViewModel()
                            {
                                Id = empInf.Id,
                                FullName = empInf.FullName,
                                Phone = empInf.Phone,
                                Email = empInf.Email,
                                Login = employee.Login,
                                Password = employee.Password,
                                Sallt = employee.Sallt,
                                RoleId = employee.RoleId
                            }).ToList();
                list.RemoveAll(x => x.RoleId == 1);
                ViewBag.EmployeeLastName = "";
                if (CurrentUser.currentUserData.RoleId == 1)
                {
                    ViewBag.EmployeeLastName = new SelectList(list, "Id", "FullName", requestTable.EmployeeInfId);
                }
                else
                {
                    EmployeeInformation ei = _dbContext.EmployeeInformations.Where(e => e.EmployeeId == CurrentUser.currentUserData.Id).First();
                    list.RemoveAll(x => x.Id != ei.Id);
                    ViewBag.EmployeeLastName = new SelectList(list, "Id", "FullName", requestTable.EmployeeInfId);
                }
                ViewBag.StatusName = new SelectList(_dbContext.Statuses.ToList(), "Id", "Name", requestTable.StatusId);
                return View(requestTable);
            }
        }
        //редактирование данных по заявке
        [HttpPost]
        public IActionResult Edit(RequestViewModel model)
        {
            var request = new Request()
            {
                Id = model.Id,
                FullNameClient = model.FullNameClient,
                ClientId = (from r in _dbContext.Requests where r.Id == model.Id select r.ClientId).First(),
                TechnicalTask = model.TechnicalTask,
                Phone = model.Phone,
                Email = model.Email,
                CreationDate = model.CreationDate,
                TakeDate = model.TakeDate,
                StatusId = model.StatusId,
                EmployeeInfId = model.EmployeeInfId
            };
            _dbContext.Requests.Update(request);
            _dbContext.SaveChanges();

            TempData["SuccessEdit"] = $"Данные успешно обновлены";
            return RedirectToAction("GetRequestTable");
        }

        //вьюшка удалить заявку
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var request = _dbContext.Requests.SingleOrDefault(r => r.Id == id);
            if (request == null)
            {
                TempData["ErrorRequest"] = $"Данного запроса не существует";
                return RedirectToAction("GetRequestTable");
            }
            else
            {
                var requestTable = new RequestViewModel()
                {
                    Id = id,
                    FullNameClient = request.FullNameClient,
                    ClientId = request.ClientId,
                    TechnicalTask = request.TechnicalTask == null? "Не определено": request.TechnicalTask,
                    Phone = request.Phone,
                    Email = request.Email,
                    CreationDate = request.CreationDate,
                    TakeDate = request.TakeDate,
                    Status = (from status in _dbContext.Statuses where status.Id== request.StatusId select status.Name).FirstOrDefault(),
                    StatusId = request.StatusId,
                    EmployeeInfId = request.EmployeeInfId,
                    EmployeeInf = request.EmployeeInfId == null? "Не определено":(from employeeInf in _dbContext.EmployeeInformations where employeeInf.Id == request.EmployeeInfId select employeeInf.FullName).FirstOrDefault()
                };
                return View(requestTable);
            }
        }
        //удаление 
        [HttpPost]
        public IActionResult Delete(RequestViewModel model)
        {
            var request = _dbContext.Requests.SingleOrDefault(r => r.Id == model.Id);

            _dbContext.Requests.Remove(request);
            _dbContext.SaveChanges();

            TempData["SuccessEdit"] = $"Данные успешно удалены";
            return RedirectToAction("GetRequestTable");
        }
    }
}
