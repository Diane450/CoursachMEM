using Microsoft.AspNetCore.Mvc;

namespace coursach.Controllers
{
    public class AdminMainPageController : Controller
    {
        [HttpGet]
        public IActionResult MainPage()
        {
            return View();
        }
    }
}
