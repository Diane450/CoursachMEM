using coursach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Xml;

namespace coursach.Controllers
{
    public class AdminMainPageController : Controller
    {
        [HttpGet]
        public IActionResult MainPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DownloadExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var dataList = GetDataFromDatabase();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");



                string[] headers = new string [] {"Имя клиента", "Техническое задание", "Дата создания заявки", "Дата начала обработки заявки", "Статус", "Исполнитель" };

                for (int col = 1; col <= headers.Length; col++)
                {
                    worksheet.Cells[1, col].Value = headers[col - 1];
                    worksheet.Cells[1, col].Style.Font.Bold = true;
                    worksheet.Cells[1, col].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Column(col).Width = 20;
                }



                for (int row = 0; row < dataList.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = dataList[row].FullNameClient;
                    worksheet.Cells[row + 2, 2].Value = dataList[row].TechnicalTask;
                    worksheet.Cells[row + 2, 3].Value = DateOnly.FromDateTime(dataList[row].CreationDate).ToString();
                    worksheet.Cells[row + 2, 4].Value = DateOnly.FromDateTime((DateTime)dataList[row].TakeDate).ToString();
                    worksheet.Cells[row + 2, 5].Value = dataList[row].Status.Name;
                    worksheet.Cells[row + 2, 6].Value = dataList[row].EmployeeInf.FullName;
                }


                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Отчет-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        private List<Request> GetDataFromDatabase()
        {
            using (var context = new Ispr2438MageramovEmCoursachContext())
            {
                return context.Requests
                    .Include(r => r.Status)
                    .Include(r => r.EmployeeInf).ToList();
            }
        }
    }
}
