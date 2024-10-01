using Microsoft.AspNetCore.Mvc;

namespace LinkTic_Test_Back.Presentation.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
